// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class ClientCredentialRequest : RequestBase
    {
        public ClientCredentialRequest(
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
            if (!ForceRefresh && TokenCache != null)
            {
                var msalAccessTokenItem =
                    await TokenCache.GetAccessToken(AuthenticationRequestParameters.Authority, AuthenticationRequestParameters.ClientId).ConfigureAwait(false);
                if (msalAccessTokenItem != null)
                {
                    return msalAccessTokenItem;
                }
            }

            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(false);
            await CacheTokenResponse(msalTokenResponse).ConfigureAwait(false);
            return msalTokenResponse;
        }

        private Dictionary<string, string> GetBodyParameters()
        {
            var dict = new Dictionary<string, string>
            {
                [OAuth2Parameter.GrantType] = OAuth2GrantType.ClientCredentials,
                [OAuth2Parameter.Scope] = AuthenticationRequestParameters.Scope.AsSingleString()
            };
            return dict;
        }
    }
}
