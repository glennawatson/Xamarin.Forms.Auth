// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Platform / OS specific logic.  No library (ADAL / MSAL) specific code should go in here.
    /// </summary>
    [global::Android.Runtime.Preserve(AllMembers = true)]
    internal class PlatformProxy : AbstractPlatformProxy
    {
        private const string ChromePackage = "com.android.chrome";
        private const string CustomTabService = "android.support.customtabs.action.CustomTabsService";

        public PlatformProxy(ICoreLogger logger)
            : base(logger)
        {
        }

        /// <inheritdoc />
        public override bool IsSystemWebViewAvailable
        {
            get
            {
                bool isBrowserWithCustomTabSupportAvailable = IsBrowserWithCustomTabSupportAvailable();
                return (isBrowserWithCustomTabSupportAvailable || IsChromeEnabled()) && isBrowserWithCustomTabSupportAvailable;
            }
        }

        /// <inheritdoc />
        public override string GetEnvironmentVariable(string variable)
        {
            return null;
        }

        /// <inheritdoc />
        protected override string InternalGetProcessorArchitecture()
        {
            if (global::Android.OS.Build.VERSION.SdkInt < global::Android.OS.BuildVersionCodes.Lollipop)
            {
                return global::Android.OS.Build.CpuAbi;
            }

            IList<string> supportedABIs = global::Android.OS.Build.SupportedAbis;
            if (supportedABIs != null && supportedABIs.Count > 0)
            {
                return supportedABIs[0];
            }

            return null;
        }

        /// <inheritdoc />
        protected override string InternalGetOperatingSystem()
        {
            return global::Android.OS.Build.VERSION.Sdk;
        }

        /// <inheritdoc />
        protected override string InternalGetDeviceModel()
        {
            return global::Android.OS.Build.Model;
        }

        /// <inheritdoc />
        protected override string InternalGetProductName()
        {
            return "MSAL.Xamarin.Android";
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Name of the calling application.</returns>
        protected override string InternalGetCallingApplicationName()
        {
            return Application.Context.ApplicationInfo?.LoadLabel(Application.Context.PackageManager);
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Version of the calling application.</returns>
        protected override string InternalGetCallingApplicationVersion()
        {
            return Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0)?.VersionName;
        }

        /// <summary>
        /// Considered PII. Please ensure that it is hashed.
        /// </summary>
        /// <returns>Device identifier.</returns>
        protected override string InternalGetDeviceId()
        {
            return global::Android.Provider.Settings.Secure.GetString(
                Application.Context.ContentResolver,
                global::Android.Provider.Settings.Secure.AndroidId);
        }

        /// <inheritdoc />
        protected override IWebUIFactory CreateWebUiFactory()
        {
            return new AndroidWebUIFactory();
        }

        /// <inheritdoc />
        protected override ICryptographyManager InternalGetCryptographyManager() => new AndroidCryptographyManager();

        /// <inheritdoc />
        protected override IPlatformLogger InternalGetPlatformLogger() => new AndroidPlatformLogger();

        /// <inheritdoc />
        protected override ITokenCache InternalGetTokenCache() => new EssentialsTokenCache();

        /// <inheritdoc />
        protected override IFeatureFlags CreateFeatureFlags() => new AndroidFeatureFlags();

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
