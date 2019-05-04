// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class InteractiveRequest : RequestBase
    {
        private readonly SortedSet<string> _extraScopesToConsent;
        private readonly UIBehavior _uiBehavior;
        private readonly IWebUI _webUi;
        private AuthorizationResult _authorizationResult;
        private string _codeVerifier;
        private string _state;

        public InteractiveRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            IEnumerable<string> extraScopesToConsent,
            UIBehavior uiBehavior,
            IWebUI webUi)
            : this(
                serviceBundle,
                authenticationRequestParameters,
                extraScopesToConsent,
                authenticationRequestParameters.Account?.Username,
                uiBehavior,
                webUi)
        {
        }

        public InteractiveRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            IEnumerable<string> extraScopesToConsent,
            string loginHint,
            UIBehavior uiBehavior,
            IWebUI webUi)
            : base(serviceBundle, authenticationRequestParameters)
        {
            RedirectUriHelper.Validate(authenticationRequestParameters.RedirectUri);
            webUi?.ValidateRedirectUri(authenticationRequestParameters.RedirectUri);

            _extraScopesToConsent = new SortedSet<string>();
            if (!extraScopesToConsent.IsNullOrEmpty())
            {
                _extraScopesToConsent = ScopeHelper.CreateSortedSetFromEnumerable(extraScopesToConsent);
            }

            ValidateScopeInput(_extraScopesToConsent);

            authenticationRequestParameters.LoginHint = loginHint;
            if (!string.IsNullOrWhiteSpace(authenticationRequestParameters.ExtraQueryParameters) &&
                authenticationRequestParameters.ExtraQueryParameters[0] == '&')
            {
                authenticationRequestParameters.ExtraQueryParameters =
                    authenticationRequestParameters.ExtraQueryParameters.Substring(1);
            }

            _webUi = webUi;
            _uiBehavior = uiBehavior;
            AuthenticationRequestParameters.RequestContext.Logger.Info(
                "Additional scopes - " + _extraScopesToConsent.AsSingleString() + ";" +
                "UIBehavior - " + _uiBehavior.PromptValue);
        }

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            await ResolveAuthorityEndpointsAsync().ConfigureAwait(false);
            await AcquireAuthorizationAsync().ConfigureAwait(false);
            VerifyAuthorizationResult();
            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(false);
            return CacheTokenResponseAndCreateAuthenticationResult(msalTokenResponse);
        }

        private async Task AcquireAuthorizationAsync()
        {
            var authorizationUri = CreateAuthorizationUri(true, true);

            await _webUi.AcquireAuthorizationAsync(
                                       authorizationUri,
                                       AuthenticationRequestParameters.RedirectUri,
                                       AuthenticationRequestParameters.RequestContext).ConfigureAwait(false);
        }

        private Dictionary<string, string> GetBodyParameters()
        {
            var dict = new Dictionary<string, string>
            {
                [OAuth2Parameter.GrantType] = OAuth2GrantType.AuthorizationCode,
                [OAuth2Parameter.Code] = _authorizationResult.Code,
                [OAuth2Parameter.RedirectUri] = AuthenticationRequestParameters.RedirectUri.OriginalString,
                [OAuth2Parameter.CodeVerifier] = _codeVerifier
            };

            return dict;
        }

        private Uri CreateAuthorizationUri(bool addVerifier = false, bool addState = false)
        {
            IDictionary<string, string> requestParameters = CreateAuthorizationRequestParameters();

            if (addVerifier)
            {
                _codeVerifier = ServiceBundle.PlatformProxy.CryptographyManager.GenerateCodeVerifier();
                string codeVerifierHash = ServiceBundle.PlatformProxy.CryptographyManager.CreateBase64UrlEncodedSha256Hash(_codeVerifier);

                requestParameters[OAuth2Parameter.CodeChallenge] = codeVerifierHash;
                requestParameters[OAuth2Parameter.CodeChallengeMethod] = OAuth2Value.CodeChallengeMethodValue;
            }

            if (addState)
            {
                _state = Guid.NewGuid().ToString();
                requestParameters[OAuth2Parameter.State] = _state;
            }

            // Add uid/utid values to QP if user object was passed in.
            if (AuthenticationRequestParameters.Account != null)
            {
                if (!string.IsNullOrEmpty(AuthenticationRequestParameters.Account.Username))
                {
                    requestParameters[OAuth2Parameter.LoginHint] = AuthenticationRequestParameters.Account.Username;
                }
            }

            CheckForDuplicateQueryParameters(AuthenticationRequestParameters.ExtraQueryParameters, requestParameters);
            CheckForDuplicateQueryParameters(AuthenticationRequestParameters.SliceParameters, requestParameters);

            string qp = requestParameters.ToQueryParameter();
            var builder = new UriBuilder(AuthenticationRequestParameters.Authority);
            builder.AppendQueryParameters(qp);

            return builder.Uri;
        }

        private void CheckForDuplicateQueryParameters(string queryParams, IDictionary<string, string> requestParameters)
        {
            if (!string.IsNullOrWhiteSpace(queryParams))
            {
                // Checks for _extraQueryParameters duplicating standard parameters
                Dictionary<string, string> kvps = CoreHelpers.ParseKeyValueList(
                    queryParams,
                    '&',
                    false,
                    AuthenticationRequestParameters.RequestContext);

                foreach (KeyValuePair<string, string> kvp in kvps)
                {
                    if (requestParameters.ContainsKey(kvp.Key))
                    {
                        throw new AuthClientException(
                            AuthClientException.DuplicateQueryParameterError,
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Duplicate query parameter '{0}' in extraQueryParameters",
                                kvp.Key));
                    }

                    requestParameters[kvp.Key] = kvp.Value;
                }
            }
        }

        private Dictionary<string, string> CreateAuthorizationRequestParameters()
        {
            SortedSet<string> unionScope = GetDecoratedScope(
                new SortedSet<string>(AuthenticationRequestParameters.Scope.Union(_extraScopesToConsent)));

            var authorizationRequestParameters = new Dictionary<string, string>
            {
                [OAuth2Parameter.Scope] = unionScope.AsSingleString(),
                [OAuth2Parameter.ResponseType] = OAuth2ResponseType.Code,

                [OAuth2Parameter.ClientId] = AuthenticationRequestParameters.ClientId,
                [OAuth2Parameter.RedirectUri] = AuthenticationRequestParameters.RedirectUri.OriginalString
            };

            if (!string.IsNullOrWhiteSpace(AuthenticationRequestParameters.LoginHint))
            {
                authorizationRequestParameters[OAuth2Parameter.LoginHint] = AuthenticationRequestParameters.LoginHint;
            }

            if (AuthenticationRequestParameters.RequestContext?.Logger?.CorrelationId != Guid.Empty)
            {
                authorizationRequestParameters[OAuth2Parameter.CorrelationId] =
                    AuthenticationRequestParameters.RequestContext.Logger.CorrelationId.ToString();
            }

            if (_uiBehavior.PromptValue != UIBehavior.NoPrompt.PromptValue)
            {
                authorizationRequestParameters[OAuth2Parameter.Prompt] = _uiBehavior.PromptValue;
            }

            return authorizationRequestParameters;
        }

        private void VerifyAuthorizationResult()
        {
            if (_authorizationResult.Status == AuthorizationStatus.Success && !_state.Equals(
                    _authorizationResult.State,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new AuthException(
                    AuthClientException.StateMismatchError,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Returned state({0}) from authorize endpoint is not the same as the one sent({1})",
                        _authorizationResult.State,
                        _state));
            }

            if (_authorizationResult.Error == OAuth2Error.LoginRequired)
            {
                throw new AuthUiRequiredException(
                    AuthUiRequiredException.NoPromptFailedError,
                    "One of two conditions was encountered:\r\n1. The UiBehavior.Never flag was passed, but the constraint could not be honored, because user interaction was required.\r\n2. An error occurred during a silent web authentication that prevented the http authentication flow from completing in a short enough time frame");
            }

            if (_authorizationResult.Status == AuthorizationStatus.UserCancel)
            {
                throw new AuthClientException(_authorizationResult.Error, _authorizationResult.ErrorDescription);
            }

            if (_authorizationResult.Status != AuthorizationStatus.Success)
            {
                throw new AuthServiceException(_authorizationResult.Error, _authorizationResult.ErrorDescription, null);
            }
        }
    }
}
