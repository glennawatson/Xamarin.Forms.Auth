// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Foundation;

using UIKit;
using WebKit;

namespace Xamarin.Forms.Auth
{
    [Register("MsalAuthenticationAgentUIViewController")]
    internal class MsalAuthenticationAgentUIViewController : UIViewController
    {
        private readonly string _url;
        private WKWebView _wkWebView;

        public MsalAuthenticationAgentUIViewController(string url, string callback, ReturnCodeCallback callbackMethod)
        {
            _url = url;
            Callback = callback;
            CallbackMethod = callbackMethod;
            NSUrlProtocol.RegisterClass(new ObjCRuntime.Class(typeof(CoreCustomUrlProtocol)));
        }

        public delegate void ReturnCodeCallback(AuthorizationResult result);

        public string Callback { get; }

        public ReturnCodeCallback CallbackMethod { get; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            _wkWebView = PrepareWKWebView();

            EvaluateJava();

            View.AddSubview(_wkWebView);

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelAuthentication);

            _wkWebView.LoadRequest(new NSUrlRequest(new NSUrl(_url)));
        }

        public void CancelAuthentication(object sender, EventArgs e)
        {
            DismissViewController(true, () => CallbackMethod(new AuthorizationResult(AuthorizationStatus.UserCancel, null)));
        }

        public override void DismissViewController(bool animated, Action completionHandler)
        {
            NSUrlProtocol.UnregisterClass(new ObjCRuntime.Class(typeof(CoreCustomUrlProtocol)));
            base.DismissViewController(animated, completionHandler);
        }

        protected WKWebView PrepareWKWebView()
        {
            WKWebViewConfiguration wkconfg = new WKWebViewConfiguration() { };

            _wkWebView = new WKWebView(View.Bounds, wkconfg)
            {
                UIDelegate = new WKWebNavigationDelegate.WKWebViewUIDelegate(this),
                NavigationDelegate = new WKWebNavigationDelegate(this),
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
            };

            return _wkWebView;
        }

        private static void HandleWKJavascriptEvaluationResult(NSObject result, NSError err)
        {
            if (err != null)
            {
                // TODO(migration): figure out how to get logger into this class: MsalLogger.Default.Info(err.LocalizedDescription);
            }

            if (result != null)
            {
                // TODO(migration): figure out how to get logger into this class: MsalLogger.Default.Info(result.ToString());
            }
        }

        private void EvaluateJava()
        {
            WKJavascriptEvaluationResult handler = HandleWKJavascriptEvaluationResult;

            _wkWebView.EvaluateJavaScript((NSString)@"navigator.userAgent", handler);
        }
    }
}
