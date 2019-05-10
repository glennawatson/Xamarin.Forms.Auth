// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Foundation;

using UIKit;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    ///     Platform / OS specific logic.  No library (ADAL / MSAL) specific code should go in here.
    /// </summary>
    internal class PlatformProxy : AbstractPlatformProxy
    {
        internal const string IosDefaultRedirectUriTemplate = "auth{0}://auth";

        public PlatformProxy(ICoreLogger logger)
            : base(logger)
        {
        }

        public override bool IsSystemWebViewAvailable => true;

        public override string GetEnvironmentVariable(string variable)
        {
            return null;
        }

        protected override string InternalGetProcessorArchitecture()
        {
            return null;
        }

        protected override string InternalGetOperatingSystem()
        {
            return UIDevice.CurrentDevice.SystemVersion;
        }

        protected override string InternalGetDeviceModel()
        {
            return UIDevice.CurrentDevice.Model;
        }

        protected override string InternalGetProductName()
        {
            return "MSAL.Xamarin.iOS";
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Name of the calling application.</returns>
        protected override string InternalGetCallingApplicationName()
        {
            return (NSString)NSBundle.MainBundle?.InfoDictionary?["CFBundleName"];
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Version of the calling application.</returns>
        protected override string InternalGetCallingApplicationVersion()
        {
            return (NSString)NSBundle.MainBundle?.InfoDictionary?["CFBundleVersion"];
        }

        /// <summary>
        /// Considered PII. Please ensure that it is hashed.
        /// </summary>
        /// <returns>Device identifier.</returns>
        protected override string InternalGetDeviceId()
        {
            return UIDevice.CurrentDevice?.IdentifierForVendor?.AsString();
        }

        /// <inheritdoc />
        protected override IWebUIFactory CreateWebUiFactory()
        {
            return new IosWebUIFactory();
        }

        protected override ICryptographyManager InternalGetCryptographyManager() => new iOSCryptographyManager();

        protected override IPlatformLogger InternalGetPlatformLogger() => new ConsolePlatformLogger();

        /// <inheritdoc />
        protected override ITokenCache InternalGetTokenCache()
        {
            return new EssentialsTokenCache();
        }

        protected override IFeatureFlags CreateFeatureFlags() => new iOSFeatureFlags();
    }
}
