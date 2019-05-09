// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Requests token by using the Refresh token.
    /// </summary>
    public partial interface IByRefreshToken
    {
        /// <summary>
        /// Acquires an access token from an existing refresh token and stores it, and the refresh token, in
        /// the user token cache, where it will be available for further AcquireTokenSilent calls.
        /// This method can be used in migration to MSAL from ADAL v2, and in various integration
        /// scenarios where you have a RefreshToken available.
        /// See https://aka.ms/msal-net-migration-adal2-msal2.
        /// </summary>
        /// <param name="scopes">Scope to request from the token endpoint.
        /// Setting this to null or empty will request an access token, refresh token and ID token with default scopes.</param>
        /// <param name="refreshToken">The refresh token from ADAL 2.x.</param>
        /// <returns>A builder enabling you to add optional parameters before executing the token request.</returns>
        AcquireTokenByRefreshTokenParameterBuilder AcquireTokenByRefreshToken(IEnumerable<string> scopes, string refreshToken);
    }
}
