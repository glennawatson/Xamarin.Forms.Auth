// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
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

        internal override async Task<OAuth2TokenResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (TokenCache == null)
            {
                throw new AuthUiRequiredException(
                    AuthUiRequiredException.TokenCacheNullError,
                    "Token cache is set to null. Silent requests cannot be executed.");
            }

            OAuth2TokenResponse tokenResponse = null;

            // Look for access token
            if (!ForceRefresh)
            {
                tokenResponse =
                    await TokenCache.GetAccessToken(AuthenticationRequestParameters.Authority, AuthenticationRequestParameters.ClientId).ConfigureAwait(false);
            }

            if (tokenResponse != null && tokenResponse.AccessTokenExpiresOn < DateTimeOffset.UtcNow)
            {
                return tokenResponse;
            }

            var refreshToken = tokenResponse?.RefreshToken;

            if (refreshToken == null)
            {
                AuthenticationRequestParameters.RequestContext.Logger.Verbose("No Refresh Token was found in the cache");

                throw new AuthUiRequiredException(
                    AuthUiRequiredException.NoTokensFoundError,
                    "No Refresh Token found in the cache");
            }

            AuthenticationRequestParameters.RequestContext.Logger.Verbose("Refreshing access token...");
            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(refreshToken), cancellationToken)
                                        .ConfigureAwait(false);

            if (msalTokenResponse.RefreshToken == null)
            {
                msalTokenResponse.RefreshToken = refreshToken;
                AuthenticationRequestParameters.RequestContext.Logger.Info(
                    "Refresh token was missing from the token refresh response, so the refresh token in the request is returned instead");
            }

            await CacheTokenResponse(msalTokenResponse).ConfigureAwait(false);

            return msalTokenResponse;
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
