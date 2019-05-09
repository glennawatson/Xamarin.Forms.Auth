// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.




namespace Xamarin.Forms.Auth
{
    internal class NetDesktopWebUIFactory : IWebUIFactory
    {
        public IWebUI CreateAuthenticationDialog(CoreUIParent parent, RequestContext requestContext)
        {
            if (parent.UseHiddenBrowser)
            {
                return new SilentWebUI(parent, requestContext);
            }

            return new InteractiveWebUI(parent, requestContext);
        }
    }
}
