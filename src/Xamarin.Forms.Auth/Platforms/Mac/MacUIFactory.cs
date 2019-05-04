// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class MacUIFactory : IWebUIFactory
    {
        public IWebUI CreateAuthenticationDialog(CoreUIParent coreUIParent, RequestContext requestContext)
        {
            return new MacEmbeddedWebUI()
            {
                CoreUIParent = coreUIParent,
                RequestContext = requestContext
            };
        }
    }
}
