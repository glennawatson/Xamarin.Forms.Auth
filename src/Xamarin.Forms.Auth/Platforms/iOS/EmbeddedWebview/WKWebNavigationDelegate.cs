// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using CoreFoundation;
using Foundation;
using UIKit;
using WebKit;

namespace Xamarin.Forms.Auth
{
    internal class WKWebNavigationDelegate : WKNavigationDelegate
    {
        private const string AboutBlankUri = "about:blank";
        private MsalAuthenticationAgentUIViewController AuthenticationAgentUIViewController = null;

        public WKWebNavigationDelegate(MsalAuthenticationAgentUIViewController AuthUIViewController)
        {
            AuthenticationAgentUIViewController = AuthUIViewController;
            return;
        }

        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            string requestUrlString = navigationAction.Request.Url.ToString();
            // If the URL has the browser:// scheme then this is a request to open an external browser
            if (requestUrlString.StartsWith(BrokerConstants.BrowserExtPrefix, StringComparison.OrdinalIgnoreCase))
            {
                DispatchQueue.MainQueue.DispatchAsync(() => AuthenticationAgentUIViewController.CancelAuthentication(null, null));

                // Build the HTTPS URL for launching with an external browser
                var httpsUrlBuilder = new UriBuilder(requestUrlString)
                {
                    Scheme = Uri.UriSchemeHttps
                };
                requestUrlString = httpsUrlBuilder.Uri.AbsoluteUri;

                DispatchQueue.MainQueue.DispatchAsync(
                    () => UIApplication.SharedApplication.OpenUrl(new NSUrl(requestUrlString)));
                AuthenticationAgentUIViewController.DismissViewController(true, null);
                decisionHandler(WKNavigationActionPolicy.Cancel);
                return;
            }

            if (requestUrlString.StartsWith(AuthenticationAgentUIViewController.callback, StringComparison.OrdinalIgnoreCase) ||
                   requestUrlString.StartsWith(BrokerConstants.BrowserExtInstallPrefix, StringComparison.OrdinalIgnoreCase))
            {
                AuthenticationAgentUIViewController.DismissViewController(true, () =>
                    AuthenticationAgentUIViewController.callbackMethod(new AuthorizationResult(AuthorizationStatus.Success, requestUrlString)));
                decisionHandler(WKNavigationActionPolicy.Cancel);
                return;
            }

            if (requestUrlString.StartsWith(BrokerConstants.DeviceAuthChallengeRedirect, StringComparison.OrdinalIgnoreCase))
            {
                Uri uri = new Uri(requestUrlString);
                string query = uri.Query;
                if (query.StartsWith("?", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Substring(1);
                }

                Dictionary<string, string> keyPair = CoreHelpers.ParseKeyValueList(query, '&', true, false, null);
                string responseHeader = DeviceAuthHelper.CreateDeviceAuthChallengeResponseAsync(keyPair).Result;

                NSMutableUrlRequest newRequest = (NSMutableUrlRequest)navigationAction.Request.MutableCopy();
                newRequest.Url = new NSUrl(keyPair["SubmitUrl"]);
                newRequest[BrokerConstants.ChallengeResponseHeader] = responseHeader;
                webView.LoadRequest(newRequest);
                decisionHandler(WKNavigationActionPolicy.Cancel);
                return;
            }

            if (!navigationAction.Request.Url.AbsoluteString.Equals(AboutBlankUri, StringComparison.OrdinalIgnoreCase)
                && !navigationAction.Request.Url.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                AuthorizationResult result = new AuthorizationResult(AuthorizationStatus.ErrorHttp)
                {
                    Error = CoreErrorCodes.NonHttpsRedirectNotSupported,
                    ErrorDescription = CoreErrorMessages.NonHttpsRedirectNotSupported
                };
                AuthenticationAgentUIViewController.DismissViewController(true, () => AuthenticationAgentUIViewController.callbackMethod(result));
                decisionHandler(WKNavigationActionPolicy.Cancel);
                return;
            }
            decisionHandler(WKNavigationActionPolicy.Allow);
            return;
        }

        internal class WKWebViewUIDelegate : WKUIDelegate
        {
            private readonly MsalAuthenticationAgentUIViewController _controller = null;

            public WKWebViewUIDelegate(MsalAuthenticationAgentUIViewController c)
            {
                _controller = c;
                return;
            }
        }
    }
}