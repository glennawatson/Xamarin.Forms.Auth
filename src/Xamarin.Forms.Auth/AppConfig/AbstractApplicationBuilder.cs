// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// The application builder abstract class.
    /// </summary>
    /// <typeparam name="T">The type we are building.</typeparam>
    public abstract class AbstractApplicationBuilder<T>
        where T : AbstractApplicationBuilder<T>
    {
        internal AbstractApplicationBuilder(ApplicationConfiguration configuration)
        {
            Config = configuration;
        }

        internal ApplicationConfiguration Config { get; }

        /// <summary>
        /// Uses a specific <see cref="IAuthHttpClientFactory"/> to communicate
        /// with the IdP. This enables advanced scenarios such as setting a proxy,
        /// or setting the Agent.
        /// </summary>
        /// <param name="httpClientFactory">HTTP client factory.</param>
        /// <remarks>MSAL does not guarantee that it will not modify the HttpClient, for example by adding new headers.</remarks>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithHttpClientFactory(IAuthHttpClientFactory httpClientFactory)
        {
            Config.HttpClientFactory = httpClientFactory;
            return (T)this;
        }

        /// <summary>
        /// Sets the logging callback.
        /// </summary>
        /// <param name="loggingCallback">A callback for providing logging information.</param>
        /// <param name="logLevel">Desired level of logging.  The default is LogLevel.Info.</param>
        /// <param name="enablePiiLogging">Boolean used to enable/disable logging of
        /// Personally Identifiable Information (PII).
        /// PII logs are never written to default outputs like Console, Logcat or NSLog
        /// Default is set to <c>false</c>, which ensures that your application is compliant with GDPR.
        /// You can set it to <c>true</c> for advanced debugging requiring PII.
        /// </param>
        /// <param name="enableDefaultPlatformLogging">Flag to enable/disable logging to platform defaults.
        /// In Desktop/UWP, Event Tracing is used. In iOS, NSLog is used.
        /// In android, logcat is used. The default value is <c>false</c>.
        /// </param>
        /// <returns>The builder to chain the .With methods.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the loggingCallback
        /// was already set on the application builder.</exception>
        public T WithLogging(
            LogCallback loggingCallback,
            LogLevel? logLevel = null,
            bool? enablePiiLogging = null,
            bool? enableDefaultPlatformLogging = null)
        {
            if (Config.LoggingCallback != null)
            {
                throw new InvalidOperationException(AuthErrorMessage.LoggingCallbackAlreadySet);
            }

            Config.LoggingCallback = loggingCallback;
            Config.LogLevel = logLevel ?? Config.LogLevel;
            Config.EnablePiiLogging = enablePiiLogging ?? Config.EnablePiiLogging;
            Config.IsDefaultPlatformLoggingEnabled = enableDefaultPlatformLogging ?? Config.IsDefaultPlatformLoggingEnabled;
            return (T)this;
        }

        /// <summary>
        /// Sets the Debug logging callback to a default debug method which displays
        /// the level of the message and the message itself. For details .
        /// </summary>
        /// <param name="logLevel">Desired level of logging.  The default is LogLevel.Info.</param>
        /// <param name="enablePiiLogging">Boolean used to enable/disable logging of
        /// Personally Identifiable Information (PII).
        /// PII logs are never written to default outputs like Console, Logcat or NSLog
        /// Default is set to <c>false</c>, which ensures that your application is compliant with GDPR.
        /// You can set it to <c>true</c> for advanced debugging requiring PII.
        /// </param>
        /// <param name="withDefaultPlatformLoggingEnabled">Flag to enable/disable logging to platform defaults.
        /// In Desktop/UWP, Event Tracing is used. In iOS, NSLog is used.
        /// In android, logcat is used. The default value is <c>false</c>.
        /// </param>
        /// <returns>The builder to chain the .With methods.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the loggingCallback
        /// was already set on the application builder by calling <see cref="WithLogging(LogCallback, LogLevel?, bool?, bool?)"/>.
        /// </exception>
        /// <seealso cref="WithLogging(LogCallback, LogLevel?, bool?, bool?)"/>
        public T WithDebugLoggingCallback(
            LogLevel logLevel = LogLevel.Info,
            bool enablePiiLogging = false,
            bool withDefaultPlatformLoggingEnabled = false)
        {
            WithLogging(
                (level, message, _) => { Debug.WriteLine($"{level}: {message}"); },
                logLevel,
                enablePiiLogging,
                withDefaultPlatformLoggingEnabled);
            return (T)this;
        }

        /// <summary>
        /// Sets the Client ID of the application.
        /// </summary>
        /// <param name="clientId">Client ID (also known as <i>Application ID</i>) of the application as registered in the
        ///  application registration portal.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithClientId(string clientId)
        {
            Config.ClientId = clientId;
            return (T)this;
        }

        /// <summary>
        /// Sets the redirect URI of the application.
        /// </summary>
        /// <param name="redirectUri">URL where the STS will call back the application with the security token.
        /// This parameter is not required for desktop or UWP applications (as a default is used).
        /// It's not required for mobile applications that don't use a broker
        /// It is required for Web Apps.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithRedirectUri(Uri redirectUri)
        {
            Config.RedirectUri = GetValueIfNotEmpty(Config.RedirectUri, redirectUri);
            return (T)this;
        }

        /// <summary>
        /// Adds a suffix that will be added to the end of the Authority property.
        /// This will make up the token endpoint.
        /// </summary>
        /// <param name="suffix">The suffix to set.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithTokenEndPointSuffix(string suffix)
        {
            Config.TokenEndpointSuffix = suffix;
            return (T)this;
        }

        /// <summary>
        /// Adds a suffix that will be added to the end of the Authority property.
        /// This will make up the Authorize endpoint.
        /// </summary>
        /// <param name="suffix">The suffix to set.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithAuthorizeEndPointSuffix(string suffix)
        {
            Config.AuthorizeEndpointSuffix = suffix;
            return (T)this;
        }

        /// <summary>
        /// Sets Extra Query Parameters for the query string in the HTTP authentication request.
        /// </summary>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority
        /// as a string of segments of the form <c>key=value</c> separated by an ampersand character.
        /// The parameter can be null.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithExtraQueryParameters(IDictionary<string, string> extraQueryParameters)
        {
            Config.ExtraQueryParameters = extraQueryParameters ?? new Dictionary<string, string>();
            return (T)this;
        }

        /// <summary>
        /// Sets Extra Query Parameters for the query string in the HTTP authentication request.
        /// </summary>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority.
        /// The string needs to be properly URL-encdoded and ready to send as a string of segments of the form <c>key=value</c> separated by an ampersand character.
        /// </param>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithExtraQueryParameters(string extraQueryParameters)
        {
            if (!string.IsNullOrWhiteSpace(extraQueryParameters))
            {
                return WithExtraQueryParameters(CoreHelpers.ParseKeyValueList(extraQueryParameters, '&', true, null));
            }

            return (T)this;
        }

        /// <summary>
        /// Adds a known authority to the application from its Uri.
        /// </summary>
        /// <param name="authorityUri">Uri of the authority.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public T WithAuthority(Uri authorityUri)
        {
            Config.Authority = authorityUri;
            return (T)this;
        }

        internal T WithHttpManager(IHttpManager httpManager)
        {
            Config.HttpManager = httpManager;
            return (T)this;
        }

        /// <summary>
        /// Validate the configuration.
        /// </summary>
        internal virtual void Validate()
        {
            // Validate that we have a client id
            if (string.IsNullOrWhiteSpace(Config.ClientId))
            {
                throw new InvalidOperationException(AuthErrorMessage.NoClientIdWasSpecified);
            }
        }

        /// <summary>
        /// Validate and build the configuration.
        /// </summary>
        /// <returns>The built configuration.</returns>
        internal ApplicationConfiguration BuildConfiguration()
        {
            Validate();
            return Config;
        }

        /// <summary>
        /// Sets application options, which can, for instance have been read from configuration files.
        /// </summary>
        /// <param name="applicationOptions">Application options.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        protected T WithOptions(ApplicationOptions applicationOptions)
        {
            WithClientId(applicationOptions.ClientId);
            WithRedirectUri(applicationOptions.RedirectUri);

            WithLogging(
                null,
                applicationOptions.LogLevel,
                applicationOptions.EnablePiiLogging,
                applicationOptions.IsDefaultPlatformLoggingEnabled);

            return (T)this;
        }

        private static string GetValueIfNotEmpty(string original, string value)
        {
            return string.IsNullOrWhiteSpace(value) ? original : value;
        }

        private static Uri GetValueIfNotEmpty(Uri original, Uri value)
        {
            return value == null ? original : value;
        }
    }
}
