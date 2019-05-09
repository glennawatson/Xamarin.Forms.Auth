// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;



namespace Xamarin.Forms.Auth
{
    internal class WebUIFactory : IWebUIFactory
    {
        public IWebUI CreateAuthenticationDialog(CoreUIParent parent, RequestContext requestContext)
        {
            throw new PlatformNotSupportedException(MsalErrorMessage.InteractiveAuthNotSupported);
        }
    }
}
