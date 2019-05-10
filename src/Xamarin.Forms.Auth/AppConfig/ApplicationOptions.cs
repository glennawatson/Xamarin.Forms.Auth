// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Base class for options objects with string values loadable from a configuration file
    /// (for instance a JSON file, as in an asp.net configuration scenario).
    /// </summary>
    public abstract class ApplicationOptions
    {
        /// <summary>
        /// Gets or sets the Client ID given to your application.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the authority (for instance https://login.microsoftonline.com for the Azure public cloud).
        /// The name was chosen to ensure compatibility with AzureAdOptions in ASP.NET Core.
        /// </summary>
        public Uri Authority { get; set; }

        /// <summary>
        /// Gets or sets The redirect URI (also known as Reply URI or Reply URL), is the URI at which OAuth2 will contact back the application with the tokens.
        /// </summary>
        /// <remarks>This is especially important when you deploy an application that you have initially tested locally;
        /// you then need to add the reply URL of the deployed application in the application registration portal.
        /// </remarks>
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable/disable logging of Personally Identifiable Information (PII).
        /// PII logs are never written to default outputs like Console, Logcat or NSLog
        /// Default is set to <c>false</c>, which ensures that your application is compliant with GDPR. You can set
        /// it to <c>true</c> for advanced debugging requiring PII.
        /// </summary>
        /// <seealso cref="IsDefaultPlatformLoggingEnabled"/>
        public bool EnablePiiLogging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable/disable logging to platform defaults. In Desktop/UWP, Event Tracing is used. In iOS, NSLog is used.
        /// In Android, logcat is used. The default value is <c>false</c>.
        /// </summary>
        /// <seealso cref="EnablePiiLogging"/>
        public bool IsDefaultPlatformLoggingEnabled { get; set; }
    }
}
