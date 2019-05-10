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
        private readonly IWebUI _webUi;
        private readonly AcquireTokenInteractiveParameters _interactiveParameters;
        private AuthorizationResult _authorizationResult;
        private string _codeVerifier;
        private string _state;
        private OAuth2TokenResponse _authTokenResponse;

        public InteractiveRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            AcquireTokenInteractiveParameters interactiveParameters,
            IWebUI webUi)
            : base(serviceBundle, authenticationRequestParameters, interactiveParameters)
        {
            _interactiveParameters = interactiveParameters;
            RedirectUriHelper.Validate(authenticationRequestParameters.RedirectUri);
            webUi?.ValidateRedirectUri(authenticationRequestParameters.RedirectUri);

            // todo(migration): can't this just come directly from interactive parameters instead of needing do to this?
            _extraScopesToConsent = new SortedSet<string>();
            if (!_interactiveParameters.ExtraScopesToConsent.IsNullOrEmpty())
            {
                _extraScopesToConsent = ScopeHelper.CreateSortedSetFromEnumerable(_interactiveParameters.ExtraScopesToConsent);
            }

            ValidateScopeInput(_extraScopesToConsent);

            _webUi = webUi;
            _interactiveParameters.LogParameters(authenticationRequestParameters.RequestContext.Logger);
        }

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            await AcquireAuthorizationAsync(cancellationToken).ConfigureAwait(false);
            VerifyAuthorizationResult();

            _authTokenResponse = await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(false);

            return CacheTokenResponseAndCreateAuthenticationResult(_authTokenResponse);
        }

        internal Task<Uri> CreateAuthorizationUriAsync()
        {
            return Task.FromResult(CreateAuthorizationUri());
        }

        private static void CheckForDuplicateQueryParameters(
            IDictionary<string, string> queryParamsDictionary,
            IDictionary<string, string> requestParameters)
        {
            foreach (KeyValuePair<string, string> kvp in queryParamsDictionary)
            {
                if (requestParameters.ContainsKey(kvp.Key))
                {
                    throw new AuthClientException(
                        AuthError.DuplicateQueryParameterError,
                        string.Format(
                            CultureInfo.InvariantCulture,
                            AuthErrorMessage.DuplicateQueryParameterTemplate,
                            kvp.Key));
                }

                requestParameters[kvp.Key] = kvp.Value;
            }
        }

        private async Task AcquireAuthorizationAsync(CancellationToken cancellationToken)
        {
            var authorizationUri = CreateAuthorizationUri(true);

            _authorizationResult = await _webUi.AcquireAuthorizationAsync(
                                       authorizationUri,
                                       AuthenticationRequestParameters.RedirectUri,
                                       AuthenticationRequestParameters.RequestContext,
                                       cancellationToken).ConfigureAwait(false);
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

        private Uri CreateAuthorizationUri(bool addPkceAndState = false)
        {
            IDictionary<string, string> requestParameters = CreateAuthorizationRequestParameters();

            if (addPkceAndState)
            {
                _codeVerifier = ServiceBundle.PlatformProxy.CryptographyManager.GenerateCodeVerifier();
                requestParameters[OAuth2Parameter.CodeChallenge] = ServiceBundle.PlatformProxy.CryptographyManager.CreateBase64UrlEncodedSha256Hash(_codeVerifier);
                requestParameters[OAuth2Parameter.CodeChallengeMethod] = OAuth2Value.CodeChallengeMethodValue;

                _state = Guid.NewGuid().ToString() + Guid.NewGuid();
                requestParameters[OAuth2Parameter.State] = _state;
            }

            CheckForDuplicateQueryParameters(AuthenticationRequestParameters.ExtraQueryParameters, requestParameters);

            string qp = requestParameters.ToQueryParameter();

#pragma warning disable CA2234 // Pass system uri objects instead of strings
            if (!AuthenticationRequestParameters.Authority.TryCombine(ServiceBundle.Config.AuthorizeEndpointSuffix, out var result))
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            {
                throw new ArgumentException("Invalid authority URI or authorize endpoint suffix, Authority cannot be combined with the AuthorizeEndPointSuffix");
            }

            var builder = new UriBuilder(result);
            builder.AppendQueryParameters(qp);

            return builder.Uri;
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

            if (!string.IsNullOrWhiteSpace(_interactiveParameters.LoginHint))
            {
                authorizationRequestParameters[OAuth2Parameter.LoginHint] = _interactiveParameters.LoginHint;
            }

            if (_interactiveParameters.Prompt.PromptValue != Prompt.NoPrompt.PromptValue)
            {
                authorizationRequestParameters[OAuth2Parameter.Prompt] = _interactiveParameters.Prompt.PromptValue;
            }

            return authorizationRequestParameters;
        }

        private void VerifyAuthorizationResult()
        {
            if (_authorizationResult.Status == AuthorizationStatus.Success &&
                !_state.Equals(
                    _authorizationResult.State,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new AuthClientException(
                    AuthError.StateMismatchError,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Returned state({0}) from authorize endpoint is not the same as the one sent({1})",
                        _authorizationResult.State,
                        _state));
            }

            if (_authorizationResult.Error == OAuth2Error.LoginRequired)
            {
                throw new AuthUiRequiredException(
                    AuthError.NoPromptFailedError,
                    AuthErrorMessage.NoPromptFailedErrorMessage);
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
