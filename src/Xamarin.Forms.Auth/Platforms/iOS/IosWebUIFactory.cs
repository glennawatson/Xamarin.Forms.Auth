// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class IosWebUIFactory : IWebUIFactory
    {
        public IWebUI CreateAuthenticationDialog(CoreUIParent coreUIParent, RequestContext requestContext)
        {
            if (coreUIParent.UseEmbeddedWebview)
            {
                return new EmbeddedWebUI()
                {
                    RequestContext = requestContext,
                    CoreUIParent = coreUIParent
                };
            }

            //there is no need to pass UIParent.
            return new SystemWebUI()
            {
                RequestContext = requestContext
            };
        }
    }
}