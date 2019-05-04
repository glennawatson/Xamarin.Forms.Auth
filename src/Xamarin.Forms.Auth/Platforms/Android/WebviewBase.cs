// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal abstract class WebviewBase : IWebUI
    {
        protected static SemaphoreSlim returnedUriReady;
        protected static AuthorizationResult authorizationResult;

        public static void SetAuthorizationResult(AuthorizationResult authorizationResultInput, RequestContext requestContext)
        {
            if (returnedUriReady != null)
            {
                authorizationResult = authorizationResultInput;
                returnedUriReady.Release();
            }
            else
            {
                requestContext.Logger.Info("No pending request for response from web ui.");
            }
        }

        public static void SetAuthorizationResult(AuthorizationResult authorizationResultInput)
        {
            authorizationResult = authorizationResultInput;
            returnedUriReady.Release();
        }

        public abstract Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext);

        public abstract void ValidateRedirectUri(Uri redirectUri);
    }
}
