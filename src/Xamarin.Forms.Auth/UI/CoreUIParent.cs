// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if ANDROID
using System;
using Android.App;
#endif
#if iOS
using UIKit;
#endif
#if MAC
using AppKit;
#endif

using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Xamarin.Forms.Auth
{
    [SuppressMessage("Design", "SA1201: Correct order", Justification = "Lot of macros")]
    internal class CoreUIParent
    {
        [SuppressMessage("Design", "RCS1074: Remove default", Justification = "Needed for platforms.")]
        public CoreUIParent()
        {
        }

        internal bool UseEmbeddedWebview { get; set; }

        internal SynchronizationContext SynchronizationContext { get; set; }

#if MAC
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreUIParent"/> class.
        /// Initializes an instance for a provided caller window.
        /// </summary>
        /// <param name="callerWindow">Caller window. OPTIONAL.</param>
        public CoreUIParent(NSWindow callerWindow)
        {
            CallerWindow = callerWindow;
        }

        /// <summary>
        /// Gets or sets caller NSWindow.
        /// </summary>
        public NSWindow CallerWindow { get; set; }
#endif

#if iOS
                              /// <summary>
                              /// Initializes a new instance of the <see cref="CoreUIParent"/> class.
                              /// Initializes an instance for a provided caller window.
                              /// </summary>
                              /// <param name="callerWindow">Caller window. OPTIONAL.</param>
        public CoreUIParent(UIViewController callerWindow)
        {
            CallerViewController = callerWindow;
        }

        /// <summary>
        /// Gets or sets caller UIViewController.
        /// </summary>
        public UIViewController CallerViewController { get; set; }

        internal static UIViewController FindCurrentViewController(UIViewController callerViewController)
        {
            if (callerViewController is UITabBarController)
            {
                UITabBarController tabBarController = (UITabBarController)callerViewController;
                return FindCurrentViewController(tabBarController.SelectedViewController);
            }
            else if (callerViewController is UINavigationController)
            {
                UINavigationController navigationController = (UINavigationController)callerViewController;
                return FindCurrentViewController(navigationController.VisibleViewController);
            }
            else if (callerViewController.PresentedViewController != null)
            {
                UIViewController presentedViewController = callerViewController.PresentedViewController;
                return FindCurrentViewController(presentedViewController);
            }
            else
            {
                return callerViewController;
            }
        }

        /// <summary>
        /// Gets or sets the preferred status bar style for the login form view controller presented.
        /// </summary>
        /// <value>The preferred status bar style.</value>
        public UIStatusBarStyle PreferredStatusBarStyle { get; set; }

        /// <summary>
        /// Gets or sets set the transition style used when the login form view is presented.
        /// </summary>
        /// <value>The modal transition style.</value>
        public UIModalTransitionStyle ModalTransitionStyle { get; set; }

        /// <summary>
        /// Gets or sets the presentation style used when the login form view is presented.
        /// </summary>
        /// <value>The modal presentation style.</value>
        public UIModalPresentationStyle ModalPresentationStyle { get; set; }

        /// <summary>
        /// Gets or sets a custom transitioning delegate to the login form view controller.
        /// </summary>
        /// <value>The transitioning delegate.</value>
        public UIViewControllerTransitioningDelegate TransitioningDelegate { get; set; }
#endif

#if ANDROID
        internal Activity Activity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreUIParent"/> class.
        /// Initializes an instance for a provided activity.
        /// </summary>
        /// <param name="activity">parent activity for the call. REQUIRED.</param>
        public CoreUIParent(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentException("passed in activity is null", nameof(activity));
            }

            Activity = activity;
            CallerActivity = activity;
        }

        /// <summary>
        /// Gets or sets the caller Android Activity.
        /// </summary>
        public Activity CallerActivity { get; set; }
#endif

#if DESKTOP || WINDOWS_APP
        //hidden webview can be used in both WinRT and desktop applications.
        internal bool UseHiddenBrowser { get; set; }
#endif

#if WINDOWS_APP
        internal bool UseCorporateNetwork { get; set; }
#endif

#if DESKTOP
        internal object OwnerWindow { get; set; }

        /// <summary>
        /// Initializes an instance for a provided parent window.
        /// </summary>
        /// <param name="ownerWindow">Parent window object reference. OPTIONAL.</param>
        public CoreUIParent(object ownerWindow)
        {
            OwnerWindow = ownerWindow;
        }
#endif
    }
}
