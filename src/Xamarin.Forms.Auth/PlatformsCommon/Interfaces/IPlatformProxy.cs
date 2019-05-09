// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Common operations for extracting platform / operating system specifics.
    /// </summary>
    internal interface IPlatformProxy
    {
        /// <summary>
        /// Gets a value indicating whether the system web view is available.
        /// </summary>
        bool IsSystemWebViewAvailable { get; }

        /// <summary>
        /// Gets the Cryptography manager.
        /// </summary>
        ICryptographyManager CryptographyManager { get; }

        /// <summary>
        /// Gets the platform logger.
        /// </summary>
        IPlatformLogger PlatformLogger { get; }

        /// <summary>
        /// Gets the token cache.
        /// </summary>
        ITokenCache TokenCache { get; }

        /// <summary>
        /// Gets the device model. On some TFMs this is not returned for security reasons.
        /// </summary>
        /// <returns>device model or null.</returns>
        string GetDeviceModel();

        /// <summary>
        /// Gets a environmental variable.
        /// </summary>
        /// <param name="variable">The variable to retrieve.</param>
        /// <returns>The environment variable.</returns>
        string GetEnvironmentVariable(string variable);

        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <returns>The operating system name.</returns>
        string GetOperatingSystem();

        /// <summary>
        /// Gets the processor architecture name.
        /// </summary>
        /// <returns>The process architecture name.</returns>
        string GetProcessorArchitecture();

        /// <summary>
        /// Returns the name of the calling assembly.
        /// </summary>
        /// <returns>The calling application name.</returns>
        string GetCallingApplicationName();

        /// <summary>
        /// Returns the version of the calling assembly.
        /// </summary>
        /// <returns>The calling application version.</returns>
        string GetCallingApplicationVersion();

        /// <summary>
        /// Returns a device identifier. Varies by platform.
        /// </summary>
        /// <returns>The device id.</returns>
        string GetDeviceId();

        /// <summary>
        /// Gets the product name.
        /// </summary>
        /// <returns>The product name.</returns>
        string GetProductName();

        /// <summary>
        /// Gets the web ui factory.
        /// </summary>
        /// <returns>The factory.</returns>
        IWebUIFactory GetWebUiFactory();

        /// <summary>
        /// Sets the web factory.
        /// </summary>
        /// <param name="webUiFactory">The web factory to set.</param>
        void /* for test */ SetWebUiFactory(IWebUIFactory webUiFactory);

        /// <summary>
        /// Gets the feature flags available on the system.
        /// </summary>
        /// <returns>The feature flags.</returns>
        IFeatureFlags GetFeatureFlags();

        /// <summary>
        /// Sets the feature flags for a system.
        /// </summary>
        /// <param name="featureFlags">The feature flags.</param>
        void /* for test */ SetFeatureFlags(IFeatureFlags featureFlags);
    }
}
