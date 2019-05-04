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
    internal class PlatformProxy : IPlatformProxy
    {
        internal const string IosDefaultRedirectUriTemplate = "msal{0}://auth";
        private readonly Lazy<IPlatformLogger> _platformLogger =
            new Lazy<IPlatformLogger>(() => new ConsolePlatformLogger());

        private IWebUIFactory _overloadWebUiFactory;

        /// <inheritdoc />
        public Task<string> GetUserPrincipalNameAsync()
        {
            return Task.FromResult(string.Empty);
        }

        /// <inheritdoc />
        public Task<bool> IsUserLocalAsync(RequestContext requestContext)
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public bool IsDomainJoined()
        {
            return false;
        }

        /// <inheritdoc />
        public string GetEnvironmentVariable(string variable)
        {
            return null;
        }

        /// <inheritdoc />
        public string GetProcessorArchitecture()
        {
            return null;
        }

        /// <inheritdoc />
        public string GetOperatingSystem()
        {
            return UIDevice.CurrentDevice.SystemVersion;
        }

        /// <inheritdoc />
        public string GetDeviceModel()
        {
            return UIDevice.CurrentDevice.Model;
        }

        /// <inheritdoc />
        public string GetBrokerOrRedirectUri(Uri redirectUri)
        {
            return redirectUri.OriginalString;
        }

        /// <inheritdoc />
        public string GetDefaultRedirectUri(string clientId)
        {
            return string.Format(CultureInfo.InvariantCulture, IosDefaultRedirectUriTemplate, clientId);
        }

        public string GetProductName()
        {
            return "MSAL.Xamarin.iOS";
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Name of the calling application</returns>
        public string GetCallingApplicationName()
        {
            return (NSString)NSBundle.MainBundle?.InfoDictionary?["CFBundleName"];
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Version of the calling application</returns>
        public string GetCallingApplicationVersion()
        {
            return (NSString)NSBundle.MainBundle?.InfoDictionary?["CFBundleVersion"];
        }

        /// <summary>
        /// Considered PII. Please ensure that it is hashed.
        /// </summary>
        /// <returns>Device identifier</returns>
        public string GetDeviceId()
        {
            return UIDevice.CurrentDevice?.IdentifierForVendor?.AsString();
        }

        public ILegacyCachePersistence CreateLegacyCachePersistence()
        {
            return new iOSLegacyCachePersistence();
        }

        public ITokenCacheAccessor CreateTokenCacheAccessor()
        {
            return new iOSTokenCacheAccessor();
        }

        /// <inheritdoc />
        public ICryptographyManager CryptographyManager { get; } = new iOSCryptographyManager();

        /// <inheritdoc />
        public IPlatformLogger PlatformLogger => _platformLogger.Value;

        /// <inheritdoc />
        public IWebUIFactory GetWebUiFactory()
        {
            return _overloadWebUiFactory ?? new IosWebUIFactory();
        }

        /// <inheritdoc />
        public void SetWebUiFactory(IWebUIFactory webUiFactory)
        {
            _overloadWebUiFactory = webUiFactory;
        }
    }
}
