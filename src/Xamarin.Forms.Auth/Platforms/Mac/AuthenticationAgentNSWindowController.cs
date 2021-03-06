﻿// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using AppKit;

using CoreGraphics;

using Foundation;

using WebKit;

namespace Xamarin.Forms.Auth
{
    [Register("AuthenticationAgentNSWindowController")]
    internal class AuthenticationAgentNSWindowController
        : NSWindowController, IWebPolicyDelegate, IWebFrameLoadDelegate, INSWindowDelegate
    {
        private const int DefaultWindowWidth = 420;
        private const int DefaultWindowHeight = 650;

        private readonly string _url;
        private readonly string _callback;
        private readonly ReturnCodeCallback _callbackMethod;

        private WebView _webView;
        private NSProgressIndicator _progressIndicator;
        private NSWindow _callerWindow;

        public AuthenticationAgentNSWindowController(string url, string callback, ReturnCodeCallback callbackMethod)
            : base("PlaceholderNibNameToForceWindowLoad")
        {
            _url = url;
            _callback = callback;
            _callbackMethod = callbackMethod;
            NSUrlProtocol.RegisterClass(new ObjCRuntime.Class(typeof(CoreCustomUrlProtocol)));
        }

        public delegate void ReturnCodeCallback(AuthorizationResult result);

        [Export("windowWillClose:")]
        public void WillClose(NSNotification notification)
        {
            NSApplication.SharedApplication.StopModal();

            NSUrlProtocol.UnregisterClass(new ObjCRuntime.Class(typeof(CoreCustomUrlProtocol)));
        }

        public void Run(NSWindow callerWindow)
        {
            _callerWindow = callerWindow;

            RunModal();
        }

        /// <summary>
        /// Loads the window.
        /// </summary>
        public override void LoadWindow()
        {
            var parentWindow = _callerWindow ?? NSApplication.SharedApplication.MainWindow;

            CGRect windowRect;
            if (parentWindow != null)
            {
                windowRect = parentWindow.Frame;
            }
            else
            {
                // If we didn't get a parent window then center it in the screen
                windowRect = NSScreen.MainScreen.Frame;
            }

            // Calculate the center of the current main window so we can position our window in the center of it
            CGRect centerRect = CenterRect(windowRect, new CGRect(0, 0, DefaultWindowWidth, DefaultWindowHeight));

            var window = new NSWindow(centerRect, NSWindowStyle.Titled | NSWindowStyle.Closable, NSBackingStore.Buffered, true)
            {
                BackgroundColor = NSColor.WindowBackground,
                WeakDelegate = this,
                AccessibilityIdentifier = "SIGN_IN_WINDOW"
            };

            var contentView = window.ContentView;
            contentView.AutoresizesSubviews = true;

            _webView = new WebView(contentView.Frame, null, null)
            {
                FrameLoadDelegate = this,
                PolicyDelegate = this,
                AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable,
                AccessibilityIdentifier = "SIGN_IN_WEBVIEW"
            };

            contentView.AddSubview(_webView);

            // On macOS there's a noticeable lag between the window showing and the page loading, so starting with the spinner
            // at least make it looks like something is happening.
            _progressIndicator = new NSProgressIndicator(
                new CGRect(
                    (DefaultWindowWidth / 2) - 16,
                    (DefaultWindowHeight / 2) - 16,
                    32,
                    32))
            {
                Style = NSProgressIndicatorStyle.Spinning,

                // Keep the item centered in the window even if it's resized.
                AutoresizingMask = NSViewResizingMask.MinXMargin | NSViewResizingMask.MaxXMargin | NSViewResizingMask.MinYMargin | NSViewResizingMask.MaxYMargin
            };

            _progressIndicator.Hidden = false;
            _progressIndicator.StartAnimation(null);

            contentView.AddSubview(_progressIndicator);

            Window = window;

            _webView.MainFrameUrl = _url;
        }

        [Export("webView:decidePolicyForNavigationAction:request:frame:decisionListener:")]
        public void DecidePolicyForNavigation(WebView webView, NSDictionary actionInformation, NSUrlRequest request, WebFrame frame, NSObject decisionToken)
        {
            if (request == null)
            {
                WebView.DecideUse(decisionToken);
                return;
            }

            string requestUrlString = request.Url.ToString();

            if (requestUrlString.StartsWith(BrokerConstants.BrowserExtPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var result = new AuthorizationResult(AuthorizationStatus.ProtocolError)
                {
                    Error = "Unsupported request",
                    ErrorDescription = "Server is redirecting client to browser. This behavior is not yet defined on Mac OS X."
                };
                _callbackMethod(result);
                WebView.DecideIgnore(decisionToken);
                Close();
                return;
            }

            if (requestUrlString.ToLower(CultureInfo.InvariantCulture).StartsWith(_callback.ToLower(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase) ||
                requestUrlString.StartsWith(BrokerConstants.BrowserExtInstallPrefix, StringComparison.OrdinalIgnoreCase))
            {
                _callbackMethod(new AuthorizationResult(AuthorizationStatus.Success, request.Url.ToString()));
                WebView.DecideIgnore(decisionToken);
                Close();
                return;
            }

            if (requestUrlString.StartsWith(BrokerConstants.DeviceAuthChallengeRedirect, StringComparison.CurrentCultureIgnoreCase))
            {
                var uri = new Uri(requestUrlString);
                string query = uri.Query;
                if (query.StartsWith("?", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Substring(1);
                }

                Dictionary<string, string> keyPair = CoreHelpers.ParseKeyValueList(query, '&', true, false, null);
                string responseHeader = DeviceAuthHelper.CreateDeviceAuthChallengeResponseAsync(keyPair).Result;

                var newRequest = (NSMutableUrlRequest)request.MutableCopy();
                newRequest.Url = new NSUrl(keyPair["SubmitUrl"]);
                newRequest[BrokerConstants.ChallengeResponseHeader] = responseHeader;
                webView.MainFrame.LoadRequest(newRequest);
                WebView.DecideIgnore(decisionToken);
                return;
            }

            if (!request.Url.AbsoluteString.Equals("about:blank", StringComparison.CurrentCultureIgnoreCase) &&
                !request.Url.Scheme.Equals("https", StringComparison.CurrentCultureIgnoreCase))
            {
                var result = new AuthorizationResult(AuthorizationStatus.ErrorHttp);
                result.Error = AuthError.NonHttpsRedirectNotSupported;
                result.ErrorDescription = AuthErrorMessage.NonHttpsRedirectNotSupported;
                _callbackMethod(result);
                WebView.DecideIgnore(decisionToken);
                Close();
            }

            WebView.DecideUse(decisionToken);
        }

        [Export("webView:didFinishLoadForFrame:")]
        public void FinishedLoad(WebView sender, WebFrame forFrame)
        {
            Window.Title = _webView.MainFrameTitle ?? "Sign in";

            _progressIndicator.Hidden = true;
            _progressIndicator.StopAnimation(null);
        }

        [Export("windowShouldClose:")]
        public bool WindowShouldClose(NSObject sender)
        {
            CancelAuthentication();
            return true;
        }

        private static CGRect CenterRect(CGRect rect1, CGRect rect2)
        {
            nfloat x = rect1.X + ((rect1.Width - rect2.Width) / 2);
            nfloat y = rect1.Y + ((rect1.Height - rect2.Height) / 2);

            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;

            rect2.X = x;
            rect2.X = y;

            return rect2;
        }

        private void RunModal()
        {
            // webview only works on main runloop, not nested, so set up manual modal runloop
            var window = Window;
            IntPtr session = NSApplication.SharedApplication.BeginModalSession(window);
            NSRunResponse result = NSRunResponse.Continues;

            while (result == NSRunResponse.Continues)
            {
                using (var pool = new NSAutoreleasePool())
                {
                    var nextEvent = NSApplication.SharedApplication.NextEvent(
                        NSEventMask.AnyEvent,
                        NSDate.DistantFuture,
                        NSRunLoopMode.Default,
                        true);

                    // discard events that are for other windows, else they remain somewhat interactive
                    if (nextEvent.Window != null && nextEvent.Window != window)
                    {
                        continue;
                    }

                    NSApplication.SharedApplication.SendEvent(nextEvent);

                    // Run the window modally until there are no events to process
                    result = (NSRunResponse)(long)NSApplication.SharedApplication.RunModalSession(session);

                    // Give the main loop some time
                    NSRunLoop.Current.LimitDateForMode(NSRunLoopMode.Default);
                }
            }

            NSApplication.SharedApplication.EndModalSession(session);
        }

        private void CancelAuthentication()
        {
            _callbackMethod(new AuthorizationResult(AuthorizationStatus.UserCancel, null));
        }
    }
}
