// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal static class AndroidConstants
    {
        public const string RequestUrlKey = "com.microsoft.identity.request.url.key";
        public const string RequestId = "com.microsoft.identity.request.id";
        public const string CustomTabRedirect = "com.microsoft.identity.customtab.redirect";
        public const string AuthorizationFinalUrl = "com.Xamarin.Auth.Forms.finalUrl";
        public const int Cancel = 2001;
        public const int AuthCodeError = 2002;
        public const int AuthCodeReceived = 2003;
        public const int AuthCodeReceivedFromEmbeddedWebview = -1;
    }
}
