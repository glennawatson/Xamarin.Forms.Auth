// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// The executor for a client application.
    /// </summary>
    internal interface IClientApplicationBaseExecutor
    {
        /// <summary>
        /// Refresh by cached silent token.
        /// </summary>
        /// <param name="commonParameters">The common parameters.</param>
        /// <param name="silentParameters">The parameters specifically for the silent request.</param>
        /// <param name="cancellationToken">A cancellation token for the ability to cancel the request.</param>
        /// <returns>The authentication result.</returns>
        Task<AuthenticationResult> ExecuteAsync(
            AcquireTokenCommonParameters commonParameters,
            AcquireTokenSilentParameters silentParameters,
            CancellationToken cancellationToken);

        /// <summary>
        /// Refresh by refresh token.
        /// </summary>
        /// <param name="commonParameters">The common parameters.</param>
        /// <param name="byRefreshTokenParameters">The parameters specifically for the refresh token request.</param>
        /// <param name="cancellationToken">A cancellation token for the ability to cancel the request.</param>
        /// <returns>The authentication result.</returns>
        Task<AuthenticationResult> ExecuteAsync(
            AcquireTokenCommonParameters commonParameters,
            AcquireTokenByRefreshTokenParameters byRefreshTokenParameters,
            CancellationToken cancellationToken);
    }
}
