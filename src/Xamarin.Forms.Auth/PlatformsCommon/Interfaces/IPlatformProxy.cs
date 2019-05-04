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
        /// Gets the platform logger.
        /// </summary>
        IPlatformLogger PlatformLogger { get; }

        /// <summary>
        /// Gets the device model. On some TFMs this is not returned for security reasons.
        /// </summary>
        /// <returns>The device model or null.</returns>
        string GetDeviceModel();

        /// <summary>
        /// Gets a environment variable.
        /// </summary>
        /// <param name="variable">The environment variable to get.</param>
        /// <returns>The environment variable value.</returns>
        string GetEnvironmentVariable(string variable);

        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <returns>The operating system name.</returns>
        string GetOperatingSystem();

        /// <summary>
        /// Gets the processing architecture name.
        /// </summary>
        /// <returns>The processor architecture name.</returns>
        string GetProcessorArchitecture();

        /// <summary>
        /// Gets the upn of the user currently logged into the OS.
        /// </summary>
        /// <returns>The user name.</returns>
        Task<string> GetUserPrincipalNameAsync();

        /// <summary>
        /// Returns true if the current OS logged in user is AD or AAD joined.
        /// </summary>
        /// <returns>If the device is domain joined.</returns>
        bool IsDomainJoined();

        /// <summary>
        /// If the user is a local one.
        /// </summary>
        /// <param name="requestContext">The request details.</param>
        /// <returns>If the user is local.</returns>
        Task<bool> IsUserLocalAsync(RequestContext requestContext);

        /// <summary>
        /// Returns the name of the calling assembly.
        /// </summary>
        /// <returns>The application calling name.</returns>
        string GetCallingApplicationName();

        /// <summary>
        /// Returns the version of the calling assembly.
        /// </summary>
        /// <returns>The calling application calling version number as a string.</returns>
        string GetCallingApplicationVersion();

        /// <summary>
        /// Returns a device identifier. Varies by platform.
        /// </summary>
        /// <returns>The device ID.</returns>
        string GetDeviceId();

        /// <summary>
        /// Get the redirect Uri as string, or the a broker specified value.
        /// </summary>
        /// <param name="redirectUri">Our desired uri.</param>
        /// <returns>The redirect uri.</returns>
        string GetBrokerOrRedirectUri(Uri redirectUri);

        /// <summary>
        /// Gets the default redirect uri for the platform, which sometimes includes the clientId.
        /// </summary>
        /// <param name="clientId">The id of the client.</param>
        /// <returns>The redirect uri.</returns>
        string GetDefaultRedirectUri(string clientId);

        /// <summary>
        /// Gets the product name.
        /// </summary>
        /// <returns>The product name.</returns>
        string GetProductName();

        /// <summary>
        /// Gets a web ui factory.
        /// </summary>
        /// <returns>The factory.</returns>
        IWebUIFactory GetWebUiFactory();

        /// <summary>
        /// Sets the web factory.
        /// </summary>
        /// <param name="webUiFactory">The web factory.</param>
        void SetWebUiFactory(IWebUIFactory webUiFactory);
    }
}
