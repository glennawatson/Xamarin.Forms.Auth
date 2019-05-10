// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class ByRefreshTokenRequest : RequestBase
    {
        private readonly AcquireTokenByRefreshTokenParameters _refreshTokenParameters;

        public ByRefreshTokenRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            AcquireTokenByRefreshTokenParameters refreshTokenParameters)
            : base(serviceBundle, authenticationRequestParameters, refreshTokenParameters)
        {
            _refreshTokenParameters = refreshTokenParameters;
        }

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (TokenCache == null)
            {
                throw new AuthUiRequiredException(
                    AuthError.TokenCacheNullError,
                    AuthErrorMessage.NullTokenCacheError);
            }

            AuthenticationRequestParameters.RequestContext.Logger.Verbose(LogMessages.BeginningAcquireByRefreshToken);
            var authTokenResponse = await SendTokenRequestAsync(
                                        GetBodyParameters(_refreshTokenParameters.RefreshToken),
                                        cancellationToken).ConfigureAwait(false);

            if (authTokenResponse.RefreshToken == null)
            {
                AuthenticationRequestParameters.RequestContext.Logger.Info(AuthErrorMessage.NoRefreshTokenInResponse);
                throw new AuthServiceException(authTokenResponse.Error, authTokenResponse.ErrorDescription, null);
            }

            return CacheTokenResponseAndCreateAuthenticationResult(authTokenResponse);
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
