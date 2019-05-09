// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A factory which will generate the web ui.
    /// </summary>
    internal interface IWebUIFactory
    {
        /// <summary>
        /// Generates a web UI.
        /// </summary>
        /// <param name="coreUIParent">The parent of the UI.</param>
        /// <param name="requestContext">The request being made.</param>
        /// <returns>The web UI.</returns>
        IWebUI CreateAuthenticationDialog(CoreUIParent coreUIParent, RequestContext requestContext);
    }
}
