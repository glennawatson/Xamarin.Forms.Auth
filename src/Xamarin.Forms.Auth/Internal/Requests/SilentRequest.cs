// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class SilentRequest : RequestBase
    {
        public SilentRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            bool forceRefresh)
            : base(serviceBundle, authenticationRequestParameters)
        {
            ForceRefresh = forceRefresh;
        }

        public bool ForceRefresh { get; }

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (TokenCache == null)
            {
                throw new MsalUiRequiredException(
                    MsalUiRequiredException.TokenCacheNullError,
                    "Token cache is set to null. Silent requests cannot be executed.");
            }

            MsalAccessTokenCacheItem msalAccessTokenItem = null;

            // Look for access token
            if (!ForceRefresh)
            {
                msalAccessTokenItem =
                    await TokenCache.FindAccessTokenAsync(AuthenticationRequestParameters).ConfigureAwait(false);
            }

            if (msalAccessTokenItem != null)
            {
                var msalIdTokenItem = TokenCache.GetIdTokenCacheItem(
                    msalAccessTokenItem.GetIdTokenItemKey(),
                    AuthenticationRequestParameters.RequestContext);

                return new AuthenticationResult(msalAccessTokenItem, msalIdTokenItem);
            }

            var msalRefreshTokenItem =
                await TokenCache.FindRefreshTokenAsync(AuthenticationRequestParameters).ConfigureAwait(false);

            if (msalRefreshTokenItem == null)
            {
                AuthenticationRequestParameters.RequestContext.Logger.Verbose("No Refresh Token was found in the cache");

                throw new MsalUiRequiredException(
                    MsalUiRequiredException.NoTokensFoundError,
                    "No Refresh Token found in the cache");
            }

            AuthenticationRequestParameters.RequestContext.Logger.Verbose("Refreshing access token...");
            await ResolveAuthorityEndpointsAsync().ConfigureAwait(false);
            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(msalRefreshTokenItem.Secret), cancellationToken)
                                        .ConfigureAwait(false);

            if (msalTokenResponse.RefreshToken == null)
            {
                msalTokenResponse.RefreshToken = msalRefreshTokenItem.Secret;
                AuthenticationRequestParameters.RequestContext.Logger.Info(
                    "Refresh token was missing from the token refresh response, so the refresh token in the request is returned instead");
            }

            return CacheTokenResponseAndCreateAuthenticationResult(msalTokenResponse);
        }

        private Dictionary<string, string> GetBodyParameters(string refreshTokenSecret)
        {
            var dict = new Dictionary<string, string>
            {
                [OAuth2Parameter.GrantType] = OAuth2GrantType.RefreshToken,
                [OAuth2Parameter.RefreshToken] = refreshTokenSecret
            };

            return dict;
        }
    }
}
