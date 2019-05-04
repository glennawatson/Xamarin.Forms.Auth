// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Android specific UI properties for interactive flows, such as the parent activity and
    /// which browser to use.
    /// </summary>
    public sealed class UIParent
    {
        private const string ChromePackage = "com.android.chrome";
        private const string CustomTabService = "android.support.customtabs.action.CustomTabsService";

        static UIParent()
        {
            ModuleInitializer.EnsureModuleInitialized();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// </summary>
        [Obsolete("This constructor should not be used because this object requires a parameters of type Activity. ")]
        public UIParent() // do not delete this ctor because it exists on NetStandard
        {
            throw new AuthClientException(MsalError.ActivityRequired, "Constructor should not be used because this object requires a parameters of type Activity");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// </summary>
        /// <param name="activity">The parent activity to call.</param>
        public UIParent(Activity activity)
        {
            CoreUIParent = new CoreUIParent(activity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// Initializes an instance for a provided activity with flag directing the application
        /// to use the embedded webview instead of the system browser.
        /// </summary>
        /// <param name="activity">The parent activity to call.</param>
        /// <param name="useEmbeddedWebview">Flag to determine between embedded vs system browser.</param>
        public UIParent(Activity activity, bool useEmbeddedWebview)
            : this(activity)
        {
            CoreUIParent.UseEmbeddedWebview = useEmbeddedWebview;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// Platform agnostic constructor that allows building an UIParent from a NetStandard assembly.
        /// On Android, the parent is expected to be an Activity.
        /// </summary>
        /// <remarks>This constructor is only avaiable at runtime, to provide support for NetStandard.</remarks>
        /// <param name="parent">Android Activity on which to parent the web UI. Cannot be null.</param>
        /// <param name="useEmbeddedWebview">Flag to determine between embedded vs system browser.</param>
        public UIParent(object parent, bool useEmbeddedWebview)
        : this(ValidateParentObject(parent), useEmbeddedWebview)
        {
        }

        /// <summary>
        /// Gets the core ui parent.
        /// </summary>
        internal CoreUIParent CoreUIParent { get; }

        /// <summary>
        /// Checks Android device for chrome packages.
        /// </summary>
        /// <returns>
        /// Returns true if chrome package for launching system webview is enabled on device.
        /// Returns false if chrome package is not found.
        /// </returns>
        /// <example>
        /// The following code decides, in a Xamarin.Forms app, which browser to use based on the presence of the
        /// required packages.
        /// <code>
        /// bool useSystemBrowser = UIParent.IsSystemWebviewAvailable();
        /// App.UIParent = new UIParent(Xamarin.Forms.Forms.Context as Activity, !useSystemBrowser);
        /// </code>
        /// </example>
        public static bool IsSystemWebviewAvailable() // This is part of the NetStandard "interface"
        {
            bool isBrowserWithCustomTabSupportAvailable = IsBrowserWithCustomTabSupportAvailable();
            return (isBrowserWithCustomTabSupportAvailable || IsChromeEnabled()) &&
                   isBrowserWithCustomTabSupportAvailable;
        }

        private static Activity ValidateParentObject(object parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent), nameof(parent) + " cannot be null on Android platforms. Please pass in an Activity to which to attach a web UI.");
            }

            if (!(parent is Activity parentActivity))
            {
                throw new ArgumentException(nameof(parent) +
                                            " is expected to be of type Android.App.Activity but is of type " +
                                            parent.GetType());
            }

            return parentActivity;
        }

        private static bool IsBrowserWithCustomTabSupportAvailable()
        {
            Intent customTabServiceIntent = new Intent(CustomTabService);

            IEnumerable<ResolveInfo> resolveInfoListWithCustomTabs =
                Application.Context.PackageManager.QueryIntentServices(
                    customTabServiceIntent, PackageInfoFlags.MatchAll);

            // queryIntentServices could return null or an empty list if no matching service existed.
            if (resolveInfoListWithCustomTabs == null || !resolveInfoListWithCustomTabs.Any())
            {
                return false;
            }

            return true;
        }

        private static bool IsChromeEnabled()
        {
            ApplicationInfo applicationInfo = Application.Context.PackageManager.GetApplicationInfo(ChromePackage, 0);

            // Chrome is difficult to uninstall on an Android device. Most users will disable it, but the package will still
            // show up, therefore need to check application.Enabled is false
            return string.IsNullOrEmpty(ChromePackage) || applicationInfo.Enabled;
        }
    }
}
