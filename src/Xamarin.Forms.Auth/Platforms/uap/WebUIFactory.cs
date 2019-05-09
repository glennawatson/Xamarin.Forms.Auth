// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.




namespace Xamarin.Forms.Auth
{
    internal class WebUIFactory : IWebUIFactory
    {
        public IWebUI CreateAuthenticationDialog(CoreUIParent parent, RequestContext requestContext)
        {
            return new WebUI(parent, requestContext);
        }
    }
}
