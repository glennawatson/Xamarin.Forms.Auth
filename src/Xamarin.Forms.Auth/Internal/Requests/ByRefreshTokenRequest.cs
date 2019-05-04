// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class ByRefreshTokenRequest : RequestBase
    {
        private string _userProvidedRefreshToken;

        public ByRefreshTokenRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            string userProvidedRefreshToken)
        : base(serviceBundle, authenticationRequestParameters)
        {
            _userProvidedRefreshToken = userProvidedRefreshToken;
        }

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (TokenCache == null)
            {
                throw new AuthUiRequiredException(
                    AuthUiRequiredException.TokenCacheNullError,
                    CoreErrorMessages.NullTokenCacheError);
            }

            AuthenticationRequestParameters.RequestContext.Logger.Info(LogMessages.BeginningAcquireByRefreshToken);
            await ResolveAuthorityEndpointsAsync().ConfigureAwait(false);
            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(_userProvidedRefreshToken), cancellationToken)
                                        .ConfigureAwait(false);

            if (msalTokenResponse.RefreshToken == null)
            {
                AuthenticationRequestParameters.RequestContext.Logger.Info(
                    CoreErrorMessages.NoRefreshTokenInResponse);
                throw new AuthServiceException(msalTokenResponse.Error, msalTokenResponse.ErrorDescription, null);
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
