// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Android.App;
using Android.Content;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Static class that consumes the response from the Authentication flow and continues token acquisition. This class should be called in OnActivityResult() of the activity doing authentication.
    /// </summary>
    public static class AuthenticationContinuationHelper
    {
        /// <summary>
        /// Sets authentication response from the webview for token acquisition continuation.
        /// </summary>
        /// <param name="requestCode">Request response code</param>
        /// <param name="resultCode">Result code from authentication</param>
        /// <param name="data">Response data from authentication</param>
        [CLSCompliant(false)]
        public static void SetAuthenticationContinuationEventArgs(int requestCode, Result resultCode, Intent data)
        {
            RequestContext requestContext = new RequestContext(null, new MsalLogger(Guid.Empty, null));

            requestContext.Logger.Info(string.Format(CultureInfo.InvariantCulture, "Received Activity Result({0})", (int)resultCode));
            AuthorizationResult authorizationResult = null;

            int code = (int)resultCode;

            if (data.Action != null && data.Action.Equals("ReturnFromEmbeddedWebview", StringComparison.OrdinalIgnoreCase))
            {
                authorizationResult = ProcessFromEmbeddedWebview(requestCode, resultCode, data);
            }
            else
            {
                authorizationResult = ProcessFromSystemWebview(requestCode, resultCode, data);
            }

            WebviewBase.SetAuthorizationResult(authorizationResult, requestContext);
        }

        private static AuthorizationResult ProcessFromEmbeddedWebview(int requestCode, Result resultCode, Intent data)
        {
            switch ((int)resultCode)
            {
                case (int)Result.Ok:
                    return new AuthorizationResult(AuthorizationStatus.Success, data.GetStringExtra("ReturnedUrl"));

                case (int)Result.Canceled:
                    return new AuthorizationResult(AuthorizationStatus.UserCancel, null);

                default:
                    return new AuthorizationResult(AuthorizationStatus.UnknownError, null);
            }
        }

        private static AuthorizationResult ProcessFromSystemWebview(int requestCode, Result resultCode, Intent data)
        {
            switch ((int)resultCode)
            {
                case AndroidConstants.AuthCodeReceived:
                    return CreateResultForOkResponse(data.GetStringExtra("com.Xamarin.Auth.Forms.finalUrl"));

                case AndroidConstants.Cancel:
                    return new AuthorizationResult(AuthorizationStatus.UserCancel, null);

                default:
                    return new AuthorizationResult(AuthorizationStatus.UnknownError, null);
            }
        }

        private static AuthorizationResult CreateResultForOkResponse(string url)
        {
            AuthorizationResult result = new AuthorizationResult(AuthorizationStatus.Success);

            if (!string.IsNullOrEmpty(url))
            {
                result.ParseAuthorizeResponse(url);
            }

            return result;
        }
    }
}
