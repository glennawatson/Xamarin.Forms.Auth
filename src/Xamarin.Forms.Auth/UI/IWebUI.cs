// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Represents a UI that is hosted in the web.
    /// </summary>
    internal interface IWebUI
    {
        /// <summary>
        /// Attempts to acquire authorization.
        /// </summary>
        /// <param name="authorizationUri">The authorization URI.</param>
        /// <param name="redirectUri">The redirection URI.</param>
        /// <param name="requestContext">The context of the request to process.</param>
        /// <param name="cancellationToken">A cancellation token to perform early cancellation.</param>
        /// <returns>The results of the operation.</returns>
        Task<AuthorizationResult> AcquireAuthorizationAsync(
            Uri authorizationUri,
            Uri redirectUri,
            RequestContext requestContext,
            CancellationToken cancellationToken);

        /// <summary>
        /// Extra validations on the redirect uri, for example system web views cannot work with the urn:oob... uri because
        /// there is no way of knowing which app to get back to.
        /// Throws if uri is invalid.
        /// </summary>
        /// <param name="redirectUri">The URI to validate.</param>
        void ValidateRedirectUri(Uri redirectUri);
    }
}
