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
    internal abstract class RequestBase
    {
        private TokenCache _tokenCache;

        protected RequestBase(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters)
        {
            ServiceBundle = serviceBundle;
            TokenCache = authenticationRequestParameters.TokenCache;

            AuthenticationRequestParameters = authenticationRequestParameters;
            if (authenticationRequestParameters.Scope == null || authenticationRequestParameters.Scope.Count == 0)
            {
                throw new ArgumentNullException(nameof(authenticationRequestParameters.Scope));
            }

            ValidateScopeInput(authenticationRequestParameters.Scope);

            AuthenticationRequestParameters.LogState();
        }

        internal AuthenticationRequestParameters AuthenticationRequestParameters { get; }

        internal TokenCache TokenCache
        {
            get => _tokenCache;
            set
            {
                _tokenCache = value;
                if (_tokenCache != null)
                {
                    _tokenCache.ServiceBundle = ServiceBundle;
                }
            }
        }

        protected IServiceBundle ServiceBundle { get; }

        public async Task<AuthenticationResult> RunAsync(CancellationToken cancellationToken)
        {
            var authenticationResult = await ExecuteAsync(cancellationToken).ConfigureAwait(false);
            LogReturnedToken(authenticationResult);
            return authenticationResult;
        }

        internal abstract Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken);

        protected AuthenticationResult CacheTokenResponseAndCreateAuthenticationResult(OAuth2TokenResponse tokenResponse)
        {
            // developer passed in user object.
            AuthenticationRequestParameters.RequestContext.Logger.Info("Checking client info returned from the server..");

            ClientInfo fromServer = null;

            if (!AuthenticationRequestParameters.IsClientCredentialRequest && !AuthenticationRequestParameters.IsRefreshTokenRequest)
            {
                // client_info is not returned from client credential flows because there is no user present.
                fromServer = ClientInfo.CreateFromJson(tokenResponse.ClientInfo);
            }

            ValidateAccountIdentifiers(fromServer);

            IdToken idToken = IdToken.Parse(tokenResponse.IdToken);

            AuthenticationRequestParameters.TenantUpdatedCanonicalAuthority = Authority.UpdateTenantId(
                AuthenticationRequestParameters.Authority.CanonicalAuthority, idToken?.TenantId);

            if (TokenCache != null)
            {
                AuthenticationRequestParameters.RequestContext.Logger.Info("Saving Token Response to cache..");

                var tuple = TokenCache.SaveAccessAndRefreshToken(AuthenticationRequestParameters, tokenResponse);
                return new AuthenticationResult(tuple.Item1, tuple.Item2);
            }
            else
            {
                return new AuthenticationResult(
                    new MsalAccessTokenCacheItem(
                        AuthenticationRequestParameters.Authority.Host,
                        AuthenticationRequestParameters.ClientId,
                        tokenResponse,
                        idToken?.TenantId),
                    new MsalIdTokenCacheItem(
                        AuthenticationRequestParameters.Authority.Host,
                        AuthenticationRequestParameters.ClientId,
                        tokenResponse,
                        idToken?.TenantId));
            }
        }

        protected SortedSet<string> GetDecoratedScope(SortedSet<string> inputScope)
        {
            SortedSet<string> set = new SortedSet<string>(inputScope.ToArray());
            set.UnionWith(ScopeHelper.CreateSortedSetFromEnumerable(OAuth2Value.ReservedScopes));
            return set;
        }

        protected void ValidateScopeInput(SortedSet<string> scopesToValidate)
        {
            // Check if scope or additional scope contains client ID.
            if (scopesToValidate.Intersect(ScopeHelper.CreateSortedSetFromEnumerable(OAuth2Value.ReservedScopes)).Any())
            {
                throw new ArgumentException("MSAL always sends the scopes 'openid profile offline_access'. " +
                                            "They cannot be suppressed as they are required for the " +
                                            "library to function. Do not include any of these scopes in the scope parameter.");
            }

            if (scopesToValidate.Contains(AuthenticationRequestParameters.ClientId))
            {
                throw new ArgumentException("API does not accept client id as a user-provided scope");
            }
        }

        protected async Task<OAuth2TokenResponse> SendTokenRequestAsync(
            IDictionary<string, string> additionalBodyParameters,
            CancellationToken cancellationToken)
        {
            OAuth2Client client = new OAuth2Client(ServiceBundle.HttpManager);
            client.AddBodyParameter(OAuth2Parameter.ClientId, AuthenticationRequestParameters.ClientId);
            client.AddBodyParameter(OAuth2Parameter.ClientInfo, "1");

            client.AddBodyParameter(OAuth2Parameter.Scope,
                GetDecoratedScope(AuthenticationRequestParameters.Scope).AsSingleString());

            foreach (var kvp in additionalBodyParameters)
            {
                client.AddBodyParameter(kvp.Key, kvp.Value);
            }

            return await SendHttpMessageAsync(client).ConfigureAwait(false);
        }

        private async Task<OAuth2TokenResponse> SendHttpMessageAsync(OAuth2Client client)
        {
            UriBuilder builder = new UriBuilder(AuthenticationRequestParameters.Authority);
            builder.AppendQueryParameters(AuthenticationRequestParameters.SliceParameters);
            OAuth2TokenResponse tokenResponse =
                await client
                    .GetTokenAsync(builder.Uri,
                        AuthenticationRequestParameters.RequestContext)
                    .ConfigureAwait(false);

            if (string.IsNullOrEmpty(tokenResponse.Scope))
            {
                tokenResponse.Scope = AuthenticationRequestParameters.Scope.AsSingleString();
                AuthenticationRequestParameters.RequestContext.Logger.Info("ScopeSet was missing from the token response, so using developer provided scopes in the result");
            }

            return tokenResponse;
        }

        private void LogReturnedToken(AuthenticationResult result)
        {
            if (result.AccessToken != null)
            {
                AuthenticationRequestParameters.RequestContext.Logger.Info(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "=== Token Acquisition finished successfully. An access token was returned with Expiration Time: {0} ===",
                        result.ExpiresOn));
            }
        }
    }
}
