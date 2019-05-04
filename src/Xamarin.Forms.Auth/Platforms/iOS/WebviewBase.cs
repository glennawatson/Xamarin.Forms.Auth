// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using SafariServices;
using UIKit;

namespace Xamarin.Forms.Auth
{
    internal abstract class WebviewBase : NSObject, IWebUI, ISFSafariViewControllerDelegate
    {
        private nint _taskId = UIApplication.BackgroundTaskInvalid;

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

        protected nint TaskId { get => _taskId; set => _taskId = value; }

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

        public abstract Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext);

        public abstract void ValidateRedirectUri(Uri redirectUri);

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
