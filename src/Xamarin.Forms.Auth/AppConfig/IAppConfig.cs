// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Configuration properties used to build a public or confidential client application.
    /// </summary>
    public interface IAppConfig
    {
        /// <summary>
        /// Gets client ID (also known as App ID) of the application as registered in the
        /// application registration portal (https://aka.ms/auth-net-register-app).
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Gets a value indicating whether flag telling if logging of Personally Identifiable Information (PII) is enabled/disabled for
        /// the application.
        /// </summary>
        /// <seealso cref="IsDefaultPlatformLoggingEnabled"/>
        bool EnablePiiLogging { get; }

        /// <summary>
        /// Gets <see cref="IAuthHttpClientFactory"/> used to get HttpClient instances to communicate
        /// with the identity provider.
        /// </summary>
        IAuthHttpClientFactory HttpClientFactory { get; }

        /// <summary>
        /// Gets level of logging requested for the app.
        /// </summary>
        LogLevel LogLevel { get; }

        /// <summary>
        /// Gets a value indicating whether flag telling if logging to platform defaults is enabled/disabled for the app.
        /// In Desktop/UWP, Event Tracing is used. In iOS, NSLog is used.
        /// In Android, logcat is used.
        /// </summary>
        bool IsDefaultPlatformLoggingEnabled { get; }

        /// <summary>
        /// Gets redirect URI for the application. See <see cref="ApplicationOptions.RedirectUri"/>.
        /// </summary>
        Uri RedirectUri { get; }

        /// <summary>
        /// Gets callback used for logging. It was set with <see cref="AbstractApplicationBuilder{T}.WithLogging(LogCallback, LogLevel?, bool?, bool?)"/>.
        /// </summary>
        LogCallback LoggingCallback { get; }

        /// <summary>
        /// Gets extra query parameters that will be applied to every acquire token operation.
        /// See <see cref="AbstractApplicationBuilder{T}.WithExtraQueryParameters(IDictionary{string, string})"/>.
        /// </summary>
        IDictionary<string, string> ExtraQueryParameters { get; }

#if WINDOWS_APP
        /// <summary>
        /// Flag to enable authentication with the user currently logeed-in in Windows.
        /// When set to true, the application will try to connect to the corporate network using windows integrated authentication.
        /// </summary>
        bool UseCorporateNetwork { get; }
#endif // WINDOWS_APP

#if iOS
        /// <summary>
        /// Gets the iOS keychain security group to use.
        /// </summary>
        string IosKeychainSecurityGroup { get; }
#endif // iOS
    }
}
