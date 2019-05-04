// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    [global::Android.Runtime.Preserve(AllMembers = true)]
    internal class AndroidWebUIFactory : IWebUIFactory
    {
        public IWebUI CreateAuthenticationDialog(CoreUIParent coreUIParent, RequestContext requestContext)
        {
            if (coreUIParent.UseEmbeddedWebview)
            {
                return new EmbeddedWebUI(coreUIParent)
                {
                    RequestContext = requestContext
                };
            }

            return new SystemWebUI(coreUIParent)
            {
                RequestContext = requestContext
            };
        }
    }
}
