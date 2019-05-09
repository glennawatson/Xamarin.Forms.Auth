// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
#if !IS_APPCENTER_BUILD
using AuthenticationServices;
#endif
using Foundation;

using SafariServices;
using UIKit;

namespace Xamarin.Forms.Auth
{
    internal abstract class WebviewBase : NSObject, IWebUI, ISFSafariViewControllerDelegate
    {
        public WebviewBase()
        {
            DidEnterBackgroundNotification = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidEnterBackgroundNotification, OnMoveToBackground);
            WillEnterForegroundNotification = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillEnterForegroundNotification, OnMoveToForeground);
        }

        protected static SemaphoreSlim ReturnedUriReady { get; set; }

        protected static AuthorizationResult AuthorizationResult { get; set; }

        protected static UIViewController ViewController { get; set; }

        protected SFSafariViewController SafariViewController { get; set; }

        protected SFAuthenticationSession SfAuthenticationSession { get; set; }

#if !IS_APPCENTER_BUILD
        /* For app center builds, this will need to build on a hosted mac agent. The mac agent does not have the latest SDK's required to build 'ASWebAuthenticationSession'
        * Until the agents are updated, appcenter build will need to ignore the use of 'ASWebAuthenticationSession' for iOS 12.*/
        protected ASWebAuthenticationSession AsWebAuthenticationSession { get; set; }
#endif

        protected nint TaskId { get; set; } = UIApplication.BackgroundTaskInvalid;

        protected NSObject DidEnterBackgroundNotification { get; set; }

        protected NSObject WillEnterForegroundNotification { get; set; }

        public static bool ContinueAuthentication(string url)
        {
            if (ReturnedUriReady == null)
            {
                return false;
            }

            ViewController.InvokeOnMainThread(() =>
            {
                AuthorizationResult = new AuthorizationResult(AuthorizationStatus.Success, url);
                ReturnedUriReady.Release();
            });

            return true;
        }

        public abstract void ValidateRedirectUri(Uri redirectUri);

        public abstract Task<AuthorizationResult> AcquireAuthorizationAsync(
        Uri authorizationUri,
        Uri redirectUri,
        RequestContext requestContext,
        CancellationToken cancellationToken);

        protected void OnMoveToBackground(NSNotification notification)
        {
            // After iOS 11.3, it is neccesary to keep a background task running while moving an app to the background
            // in order to prevent the system from reclaiming network resources from the app.
            // This will prevent authentication from failing while the application is moved to the background while waiting for MFA to finish.
            TaskId = UIApplication.SharedApplication.BeginBackgroundTask(() =>
            {
                if (TaskId != UIApplication.BackgroundTaskInvalid)
                {
                    UIApplication.SharedApplication.EndBackgroundTask(TaskId);
                    TaskId = UIApplication.BackgroundTaskInvalid;
                }
            });
        }

        protected void OnMoveToForeground(NSNotification notification)
        {
            if (TaskId != UIApplication.BackgroundTaskInvalid)
            {
                UIApplication.SharedApplication.EndBackgroundTask(TaskId);
                TaskId = UIApplication.BackgroundTaskInvalid;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DidEnterBackgroundNotification != null)
                {
                    DidEnterBackgroundNotification.Dispose();
                    DidEnterBackgroundNotification = null;
                }

                if (WillEnterForegroundNotification != null)
                {
                    WillEnterForegroundNotification.Dispose();
                    WillEnterForegroundNotification = null;
                }
            }
        }
    }
}
