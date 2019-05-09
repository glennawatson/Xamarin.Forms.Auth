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
        private readonly MsalAuthenticationAgentUIViewController _authenticationAgentUIViewController;

        public WKWebNavigationDelegate(MsalAuthenticationAgentUIViewController authUIViewController)
        {
            _authenticationAgentUIViewController = authUIViewController;
        }

        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            if (!navigationAction.Request.Url.AbsoluteString.Equals(AboutBlankUri, StringComparison.OrdinalIgnoreCase)
                && !navigationAction.Request.Url.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                AuthorizationResult result = new AuthorizationResult(AuthorizationStatus.ErrorHttp)
                {
                    Error = AuthError.NonHttpsRedirectNotSupported,
                    ErrorDescription = AuthErrorMessage.NonHttpsRedirectNotSupported
                };
                _authenticationAgentUIViewController.DismissViewController(true, () => _authenticationAgentUIViewController.CallbackMethod(result));
                decisionHandler(WKNavigationActionPolicy.Cancel);
                return;
            }

            decisionHandler(WKNavigationActionPolicy.Allow);
        }

        internal class WKWebViewUIDelegate : WKUIDelegate
        {
            private readonly MsalAuthenticationAgentUIViewController _controller;

            public WKWebViewUIDelegate(MsalAuthenticationAgentUIViewController c)
            {
                _controller = c;
            }
        }
    }
}
