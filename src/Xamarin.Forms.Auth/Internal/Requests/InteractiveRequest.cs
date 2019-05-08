// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JWT;

namespace Xamarin.Forms.Auth
{
    internal class InteractiveRequest : RequestBase
    {
        private const int CodeVerifierByteSize = 32;
        private static readonly IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
        private readonly SortedSet<string> _extraScopesToConsent;
        private readonly UIBehavior _uiBehavior;
        private readonly IWebUI _webUi;
        private string _codeVerifier;
        private string _state;
        private AuthorizationResult _authorizationResult;

        public InteractiveRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            IReadOnlyCollection<string> extraScopesToConsent,
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

        internal override async Task<OAuth2TokenResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            await AcquireAuthorizationAsync().ConfigureAwait(false);
            VerifyAuthorizationResult();
            var tokenResponse = await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(false);
            await CacheTokenResponse(tokenResponse).ConfigureAwait(false);
            return tokenResponse;
        }

        private static string GenerateCodeVerifier()
        {
            byte[] buffer = new byte[CodeVerifierByteSize];
            using (var randomSource = new RNGCryptoServiceProvider())
            {
                randomSource.GetBytes(buffer);
            }

            return _urlEncoder.Encode(buffer);
        }

        private static string CreateBase64UrlEncodedSha256Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            using (SHA256Managed sha = new SHA256Managed())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                return _urlEncoder.Encode(sha.ComputeHash(encoding.GetBytes(input)));
            }
        }

        private async Task AcquireAuthorizationAsync()
        {
            var authorizationUri = CreateAuthorizationUri(true, true);

            _authorizationResult = await _webUi.AcquireAuthorizationAsync(
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
                _codeVerifier = GenerateCodeVerifier();
                requestParameters[OAuth2Parameter.CodeChallenge] = CreateBase64UrlEncodedSha256Hash(_codeVerifier);
                requestParameters[OAuth2Parameter.CodeChallengeMethod] = OAuth2Value.CodeChallengeMethodValue;
            }

            if (addState)
            {
                _state = Guid.NewGuid().ToString();
                requestParameters[OAuth2Parameter.State] = _state;
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
                    AuthenticationRequestParameters.RequestContext?.Logger?.CorrelationId.ToString();
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
