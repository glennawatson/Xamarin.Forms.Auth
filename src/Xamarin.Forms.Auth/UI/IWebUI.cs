// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Represents a UI which is hosted in the Web for authentication.
    /// </summary>
    internal interface IWebUI
    {
        /// <summary>
        /// Azures a authentication result from the specified URIs.
        /// </summary>
        /// <param name="authorizationUri">The URI to the authorization server.</param>
        /// <param name="redirectUri">The URI where to redirect with the results.</param>
        /// <param name="requestContext">The context which contains the request details.</param>
        /// <returns>The results of the authorization attempt.</returns>
        Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext);

        /// <summary>
        /// Extra validations on the redirect uri, for example system web views cannot work with the urn:oob... uri because
        /// there is no way of knowing which app to get back to.
        /// Throws if uri is invalid.
        /// </summary>
        /// <param name="redirectUri">The URI where to redirect.</param>
        void ValidateRedirectUri(Uri redirectUri);
    }
}
