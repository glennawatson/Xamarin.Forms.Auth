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

        internal override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!ForceRefresh && TokenCache != null)
            {
                var msalAccessTokenItem =
                    await TokenCache.FindAccessTokenAsync(AuthenticationRequestParameters).ConfigureAwait(false);
                if (msalAccessTokenItem != null)
                {
                    return new AuthenticationResult(msalAccessTokenItem, null);
                }
            }

            await ResolveAuthorityEndpointsAsync().ConfigureAwait(false);
            var msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(false);
            return CacheTokenResponseAndCreateAuthenticationResult(msalTokenResponse);
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
