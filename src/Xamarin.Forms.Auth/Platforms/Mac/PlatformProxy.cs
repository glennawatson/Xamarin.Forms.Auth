// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Platform / OS specific logic.
    /// </summary>
    internal class PlatformProxy : IPlatformProxy
    {
        private readonly Lazy<IPlatformLogger> _platformLogger = new Lazy<IPlatformLogger>(() => new ConsolePlatformLogger());
        private IWebUIFactory _overloadWebUiFactory;

        /// <inheritdoc />
        public ICryptographyManager CryptographyManager { get; } = new MacCryptographyManager();

        /// <inheritdoc />
        public IPlatformLogger PlatformLogger => _platformLogger.Value;

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
            if (String.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentNullException(nameof(variable));
            }

            return Environment.GetEnvironmentVariable(variable);
        }

        /// <inheritdoc />
        public string GetProcessorArchitecture()
        {
            return null;
        }

        /// <inheritdoc />
        public string GetOperatingSystem()
        {
            return Environment.OSVersion.ToString();
        }

        /// <inheritdoc />
        public string GetDeviceModel()
        {
            return null;
        }

        /// <inheritdoc />
        public string GetBrokerOrRedirectUri(Uri redirectUri)
        {
            return redirectUri.OriginalString;
        }

        /// <inheritdoc />
        public string GetDefaultRedirectUri(string clientId)
        {
            return Constants.DefaultRedirectUri;
        }

        public string GetProductName()
        {
            return "MSAL.Xamarin.Mac";
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Name of the calling application</returns>
        public string GetCallingApplicationName()
        {
            return Assembly.GetEntryAssembly()?.GetName()?.Name;
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Version of the calling application.</returns>
        public string GetCallingApplicationVersion()
        {
            return Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString();
        }

        private static readonly Lazy<string> DeviceIdLazy = new Lazy<string>(
           () => NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                                 .Select(nic => nic.GetPhysicalAddress()?.ToString()).FirstOrDefault());

        /// <summary>
        /// Considered PII. Please ensure that it is hashed.
        /// </summary>
        /// <returns>Device identifier</returns>
        public string GetDeviceId()
        {
            return DeviceIdLazy.Value;
        }

        /// <inheritdoc />
        public IWebUIFactory GetWebUiFactory()
        {
            return _overloadWebUiFactory ?? new MacUIFactory();
        }

        /// <inheritdoc />
        public void SetWebUiFactory(IWebUIFactory webUiFactory)
        {
            _overloadWebUiFactory = webUiFactory;
        }
    }
}
