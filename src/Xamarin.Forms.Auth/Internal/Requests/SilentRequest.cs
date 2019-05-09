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
        private readonly AcquireTokenSilentParameters _silentParameters;

        public SilentRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            AcquireTokenSilentParameters silentParameters)
            : base(serviceBundle, authenticationRequestParameters, silentParameters)
        {
            _silentParameters = silentParameters;
        }

        internal override Task PreRunAsync()
        {
            AuthenticationRequestParameters.Authority = AuthenticationRequestParameters.AuthorityOverride == null
                                                            ? ServiceBundle.Config.AuthorityInfo
                                                            : AuthenticationRequestParameters.AuthorityOverride;

            return Task.CompletedTask;
        }

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (TokenCache == null)
            {
                throw new AuthUiRequiredException(
                    AuthError.TokenCacheNullError,
                    AuthErrorMessage.NullTokenCacheForSilentError);
            }

            var tokenResponse = await TokenCache.GetAccessToken(AuthenticationRequestParameters.Authority, AuthenticationRequestParameters.ClientId).ConfigureAwait(false);

            // Look for access token
            if (!_silentParameters.ForceRefresh && tokenResponse != null)
            {
                return new AuthenticationResult(tokenResponse, ServiceBundle.Config.IsExtendedTokenLifetimeEnabled);
            }

            if (tokenResponse?.RefreshToken == null)
            {
                AuthenticationRequestParameters.RequestContext.Logger.Verbose("No Refresh Token was found in the cache");

                throw new AuthUiRequiredException(
                    AuthError.NoTokensFoundError,
                    AuthErrorMessage.NoTokensFoundError);
            }

            tokenResponse = await RefreshAccessTokenAsync(tokenResponse.RefreshToken, cancellationToken).ConfigureAwait(false);
            return CacheTokenResponseAndCreateAuthenticationResult(tokenResponse);
        }

        private async Task<OAuth2TokenResponse> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            AuthenticationRequestParameters.RequestContext.Logger.Verbose("Refreshing access token...");

            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(refreshToken), cancellationToken)
                                    .ConfigureAwait(false);

            if (msalTokenResponse.RefreshToken == null)
            {
                msalTokenResponse.RefreshToken = refreshToken;
                AuthenticationRequestParameters.RequestContext.Logger.Info(
                    "Refresh token was missing from the token refresh response, so the refresh token in the request is returned instead");
            }

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
