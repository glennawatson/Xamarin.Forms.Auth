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
    internal class PlatformProxy : AbstractPlatformProxy
    {
        public PlatformProxy(ICoreLogger logger)
            : base(logger)
        {
        }

        public override bool IsSystemWebViewAvailable => false;

        public override string GetEnvironmentVariable(string variable)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentNullException(nameof(variable));
            }

            return Environment.GetEnvironmentVariable(variable);
        }

        /// <inheritdoc />
        protected override string InternalGetProductName()
        {
            return "MSAL.CoreCLR";
        }

        protected override string InternalGetProcessorArchitecture()
        {
            return null;
        }

        protected override string InternalGetOperatingSystem()
        {
            return null;
        }

        protected override string InternalGetDeviceModel()
        {
            return null;
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Name of the calling application.</returns>
        protected override string InternalGetCallingApplicationName()
        {
            return null;
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Version of the calling application.</returns>
        protected override string InternalGetCallingApplicationVersion()
        {
            return null;
        }

        /// <summary>
        /// Considered PII. Please ensure that it is hashed.
        /// </summary>
        /// <returns>Device identifier.</returns>
        protected override string InternalGetDeviceId()
        {
            return null;
        }

        protected override IWebUIFactory CreateWebUiFactory() => new WebUIFactory();

        protected override ICryptographyManager InternalGetCryptographyManager() => new NetStandard13CryptographyManager();

        protected override IPlatformLogger InternalGetPlatformLogger() => new EventSourcePlatformLogger();

        /// <inheritdoc />
        protected override ITokenCache InternalGetTokenCache()
        {
            return null;
        }

        protected override IFeatureFlags CreateFeatureFlags() => new NetStandardFeatureFlags();
    }
}
