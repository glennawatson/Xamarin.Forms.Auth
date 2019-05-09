// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A executor which will execute authentication actions.
    /// </summary>
    internal interface IPublicClientApplicationExecutor
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="commonParameters">Common parameters to run with.</param>
        /// <param name="interactiveParameters">The interactive parameters.</param>
        /// <param name="cancellationToken">A cancellation token to stop the action.</param>
        /// <returns>The results from the authentication.</returns>
        Task<AuthenticationResult> ExecuteAsync(
            AcquireTokenCommonParameters commonParameters,
            AcquireTokenInteractiveParameters interactiveParameters,
            CancellationToken cancellationToken);
    }
}
