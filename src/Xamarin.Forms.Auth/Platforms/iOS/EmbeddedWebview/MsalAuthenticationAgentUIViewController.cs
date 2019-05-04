// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Foundation;
using UIKit;
using WebKit;

namespace Xamarin.Forms.Auth
{
    [Foundation.Register("MsalAuthenticationAgentUIViewController")]
    internal class MsalAuthenticationAgentUIViewController : UIViewController
    {
        private readonly string url;
        public readonly string callback;
        private WKWebView wkWebView;

        public readonly ReturnCodeCallback callbackMethod;

        public delegate void ReturnCodeCallback(AuthorizationResult result);

        public MsalAuthenticationAgentUIViewController(string url, string callback, ReturnCodeCallback callbackMethod)
        {
            this.url = url;
            this.callback = callback;
            this.callbackMethod = callbackMethod;
            NSUrlProtocol.RegisterClass(new ObjCRuntime.Class(typeof(CoreCustomUrlProtocol)));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            wkWebView = PrepareWKWebView();

            EvaluateJava();

            this.View.AddSubview(wkWebView);

            this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
                CancelAuthentication);

            wkWebView.LoadRequest(new NSUrlRequest(new NSUrl(this.url)));
        }

        protected WKWebView PrepareWKWebView()
        {
            WKWebViewConfiguration wkconfg = new WKWebViewConfiguration() { };

            wkWebView = new WKWebView(View.Bounds, wkconfg)
            {
                UIDelegate = new WKWebNavigationDelegate.WKWebViewUIDelegate(this),
                NavigationDelegate = new WKWebNavigationDelegate(this),
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
            };

            return wkWebView;
        }

        private void EvaluateJava()
        {
            WKJavascriptEvaluationResult handler = HandleWKJavascriptEvaluationResult;

            wkWebView.EvaluateJavaScript((NSString)@"navigator.userAgent", handler);
        }

        private static void HandleWKJavascriptEvaluationResult(NSObject result, NSError err)
        {
            if (err != null)
            {
                MsalLogger.Default.Info(err.LocalizedDescription);
            }
            if (result != null)
            {
                MsalLogger.Default.Info(result.ToString());
            }
            return;
        }

        public void CancelAuthentication(object sender, EventArgs e)
        {
            this.DismissViewController(true, () =>
                    callbackMethod(new AuthorizationResult(AuthorizationStatus.UserCancel, null)));
        }

        public override void DismissViewController(bool animated, Action completionHandler)
        {
            NSUrlProtocol.UnregisterClass(new ObjCRuntime.Class(typeof(CoreCustomUrlProtocol)));
            base.DismissViewController(animated, completionHandler);
        }
    }
}
