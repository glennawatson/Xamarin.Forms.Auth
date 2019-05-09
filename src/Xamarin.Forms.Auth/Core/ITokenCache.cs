// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A token cache where tokens are stored away.
    /// </summary>
    internal interface ITokenCache
    {
        /// <summary>
        /// Saves both the access and refresh tokens.
        /// </summary>
        /// <param name="authority">The authority associated with the token..</param>
        /// <param name="clientId">The client id associated with the token.</param>
        /// <param name="tokenResponse">The token request that stores the data.</param>
        /// <returns>A task to monitor the progress.</returns>
        Task SaveAccessAndRefreshToken(Uri authority, string clientId, OAuth2TokenResponse tokenResponse);

        /// <summary>
        /// Gets a token value from the token cache.
        /// </summary>
        /// <param name="authority">The authority associated with the token..</param>
        /// <param name="clientId">The client id associated with the token.</param>
        /// <returns>The token response if there is one, or null otherwise.</returns>
        Task<OAuth2TokenResponse> GetAccessToken(Uri authority, string clientId);

        /// <summary>
        /// Removes details about the specified account.
        /// </summary>
        /// <param name="authority">The authority associated with the token..</param>
        /// <param name="clientId">The client id associated with the token.</param>
        /// <returns>A task to monitor the progress.</returns>
        Task RemoveAccount(Uri authority, string clientId);
    }
}
