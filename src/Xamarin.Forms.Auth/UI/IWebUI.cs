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
