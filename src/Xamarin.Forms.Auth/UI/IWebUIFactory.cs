// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A factory which creates web based authentication dialogs.
    /// </summary>
    internal interface IWebUIFactory
    {
        /// <summary>
        /// Creates the web authentication dialog.
        /// </summary>
        /// <param name="coreUIParent">The parent of the form.</param>
        /// <param name="requestContext">The request.</param>
        /// <returns>The web authentication dialog handle.</returns>
        IWebUI CreateAuthenticationDialog(CoreUIParent coreUIParent, RequestContext requestContext);
    }
}
