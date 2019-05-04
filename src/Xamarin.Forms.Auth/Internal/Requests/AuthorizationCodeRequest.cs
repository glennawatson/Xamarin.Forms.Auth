// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class AuthorizationCodeRequest : RequestBase
    {
        public AuthorizationCodeRequest(
            IServiceBundle serviceBundle,
            AuthenticationRequestParameters authenticationRequestParameters)
            : base(serviceBundle, authenticationRequestParameters)
        {
            if (string.IsNullOrWhiteSpace(authenticationRequestParameters.AuthorizationCode))
            {
                throw new ArgumentNullException(nameof(authenticationRequestParameters.AuthorizationCode));
            }

            RedirectUriHelper.Validate(authenticationRequestParameters.RedirectUri);
        }


        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            await ResolveAuthorityEndpointsAsync().ConfigureAwait(false);
            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(false);
            return CacheTokenResponseAndCreateAuthenticationResult(msalTokenResponse);
        }

        private Dictionary<string, string> GetBodyParameters()
        {
            var dict = new Dictionary<string, string>
            {
                [OAuth2Parameter.GrantType] = OAuth2GrantType.AuthorizationCode,
                [OAuth2Parameter.Code] = AuthenticationRequestParameters.AuthorizationCode,
                [OAuth2Parameter.RedirectUri] = AuthenticationRequestParameters.RedirectUri.OriginalString
            };
            return dict;
        }
    }
}
