// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class AuthorizationCodeRequest : RequestBase
    {
        private readonly AcquireTokenByAuthorizationCodeParameters _authorizationCodeParameters;

        public AuthorizationCodeRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters,
            AcquireTokenByAuthorizationCodeParameters authorizationCodeParameters)
            : base(serviceBundle, authenticationRequestParameters, authorizationCodeParameters)
        {
            _authorizationCodeParameters = authorizationCodeParameters;
            RedirectUriHelper.Validate(authenticationRequestParameters.RedirectUri);
        }

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(false);
            return CacheTokenResponseAndCreateAuthenticationResult(msalTokenResponse);
        }

        private Dictionary<string, string> GetBodyParameters()
        {
            var dict = new Dictionary<string, string>
            {
                [OAuth2Parameter.GrantType] = OAuth2GrantType.AuthorizationCode,
                [OAuth2Parameter.Code] = _authorizationCodeParameters.AuthorizationCode,
                [OAuth2Parameter.RedirectUri] = AuthenticationRequestParameters.RedirectUri.OriginalString
            };
            return dict;
        }
    }
}
