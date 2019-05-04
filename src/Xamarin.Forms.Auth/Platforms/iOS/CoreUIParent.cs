// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Xamarin.Forms.Auth
{
    internal class CoreUIParent
    {
        /// <summary>
        /// Gets or sets the caller view controller.
        /// </summary>
        public UIViewController CallerViewController { get; set; }

        /// <summary>
        /// Gets or sets the preferred status bar style for the login form view controller presented.
        /// </summary>
        public UIStatusBarStyle PreferredStatusBarStyle { get; set; }

        /// <summary>
        /// Gets or sets the transition style used when the login form view is presented.
        /// </summary>
        public UIModalTransitionStyle ModalTransitionStyle { get; set; }

        /// <summary>
        /// Gets or sets the presentation style used when the login form view is presented.
        /// </summary>
        public UIModalPresentationStyle ModalPresentationStyle { get; set; }

        /// <summary>
        /// Gets or sets a custom transitioning delegate to the login form view controller.
        /// </summary>
        public UIViewControllerTransitioningDelegate TransitioningDelegate { get; set; }

        internal bool UseEmbeddedWebview { get; set; }

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
    }
}
