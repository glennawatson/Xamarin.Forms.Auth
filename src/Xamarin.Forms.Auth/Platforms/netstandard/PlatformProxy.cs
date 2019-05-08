// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Platform / OS specific logic.  No library (ADAL / MSAL) specific code should go in here.
    /// </summary>
    internal class PlatformProxy : IPlatformProxy
    {
        private readonly Lazy<IPlatformLogger> _platformLogger =
            new Lazy<IPlatformLogger>(() => new EventSourcePlatformLogger());

        private IWebUIFactory _overloadWebUiFactory;

        /// <inheritdoc />
        public IPlatformLogger PlatformLogger => _platformLogger.Value;

        /// <inheritdoc />
        public ITokenCache TokenCache => null;

        /// <summary>
        /// Get the user logged in.
        /// </summary>
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
        public string GetBrokerOrRedirectUri(Uri redirectUri)
        {
            return redirectUri.OriginalString;
        }

        /// <inheritdoc />
        public string GetProductName()
        {
            return "MSAL.CoreCLR";
        }

        /// <inheritdoc />
        public bool IsDomainJoined()
        {
            return false;
        }

        /// <inheritdoc />
        public string GetEnvironmentVariable(string variable)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentNullException(nameof(variable));
            }

            return Environment.GetEnvironmentVariable(variable);
        }

        public string GetProcessorArchitecture()
        {
            return null;
        }

        public string GetOperatingSystem()
        {
            return null;
        }

        public string GetDeviceModel()
        {
            return null;
        }

        public string GetCallingApplicationName()
        {
            return null;
        }

        public string GetCallingApplicationVersion()
        {
            return null;
        }

        public string GetDeviceId()
        {
            return null;
        }

        /// <inheritdoc />
        public IWebUIFactory GetWebUiFactory()
        {
            return _overloadWebUiFactory ?? new WebUIFactory();
        }

        /// <inheritdoc />
        public void SetWebUiFactory(IWebUIFactory webUiFactory)
        {
            _overloadWebUiFactory = webUiFactory;
        }
    }
}
