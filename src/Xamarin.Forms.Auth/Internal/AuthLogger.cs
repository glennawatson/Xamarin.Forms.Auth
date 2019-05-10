// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text;

namespace Xamarin.Forms.Auth
{
    internal class AuthLogger : ICoreLogger
    {
        private static readonly Lazy<ICoreLogger> _nullLogger = new Lazy<ICoreLogger>(() => new NullLogger());
        private readonly IPlatformLogger _platformLogger;
        private readonly LogCallback _loggingCallback;
        private readonly LogLevel _logLevel;
        private readonly bool _isDefaultPlatformLoggingEnabled;

        internal AuthLogger(
            LogLevel logLevel,
            bool enablePiiLogging,
            bool isDefaultPlatformLoggingEnabled,
            LogCallback loggingCallback)
        {
            PiiLoggingEnabled = enablePiiLogging;
            _loggingCallback = loggingCallback;
            _logLevel = logLevel;
            _isDefaultPlatformLoggingEnabled = isDefaultPlatformLoggingEnabled;

            _platformLogger = PlatformProxyFactory.CreatePlatformProxy(null).PlatformLogger;

            ClientInformation = string.Empty;
        }

        public static ICoreLogger NullLogger => _nullLogger.Value;

        public bool PiiLoggingEnabled { get; }

        internal string ClientInformation { get; }

        public static ICoreLogger Create(
            IApplicationConfiguration config,
            bool isDefaultPlatformLoggingEnabled = false)
        {
            return new AuthLogger(
                config?.LogLevel ?? LogLevel.Verbose,
                config?.EnablePiiLogging ?? false,
                config?.IsDefaultPlatformLoggingEnabled ?? isDefaultPlatformLoggingEnabled,
                config?.LoggingCallback);
        }

        public void Info(string messageScrubbed)
        {
            Log(LogLevel.Info, string.Empty, messageScrubbed);
        }

        public void InfoPii(string messageWithPii, string messageScrubbed)
        {
            Log(LogLevel.Info, messageWithPii, messageScrubbed);
        }

        public void InfoPii(Exception exWithPii)
        {
            Log(LogLevel.Info, exWithPii.ToString(), GetPiiScrubbedExceptionDetails(exWithPii));
        }

        public void InfoPiiWithPrefix(Exception exWithPii, string prefix)
        {
            Log(LogLevel.Info, prefix + exWithPii.ToString(), prefix + GetPiiScrubbedExceptionDetails(exWithPii));
        }

        public void Verbose(string messageScrubbed)
        {
            Log(LogLevel.Verbose, string.Empty, messageScrubbed);
        }

        public void VerbosePii(string messageWithPii, string messageScrubbed)
        {
            Log(LogLevel.Verbose, messageWithPii, messageScrubbed);
        }

        public void Warning(string messageScrubbed)
        {
            Log(LogLevel.Warning, string.Empty, messageScrubbed);
        }

        public void WarningPii(string messageWithPii, string messageScrubbed)
        {
            Log(LogLevel.Warning, messageWithPii, messageScrubbed);
        }

        public void WarningPii(Exception exWithPii)
        {
            Log(LogLevel.Warning, exWithPii.ToString(), GetPiiScrubbedExceptionDetails(exWithPii));
        }

        public void WarningPiiWithPrefix(Exception exWithPii, string prefix)
        {
            Log(LogLevel.Warning, prefix + exWithPii.ToString(), prefix + GetPiiScrubbedExceptionDetails(exWithPii));
        }

        public void Error(string messageScrubbed)
        {
            Log(LogLevel.Error, string.Empty, messageScrubbed);
        }

        public void ErrorPii(Exception exWithPii)
        {
            Log(LogLevel.Error, exWithPii.ToString(), GetPiiScrubbedExceptionDetails(exWithPii));
        }

        public void ErrorPiiWithPrefix(Exception exWithPii, string prefix)
        {
            Log(LogLevel.Error, prefix + exWithPii.ToString(), prefix + GetPiiScrubbedExceptionDetails(exWithPii));
        }

        public void ErrorPii(string messageWithPii, string messageScrubbed)
        {
            Log(LogLevel.Error, messageWithPii, messageScrubbed);
        }

        internal static string GetPiiScrubbedExceptionDetails(Exception ex)
        {
            var sb = new StringBuilder();
            if (ex != null)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Exception type: {0}", ex.GetType()));

                if (ex is AuthException authException)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, ", ErrorCode: {0}", authException.ErrorCode));
                }

                if (ex is AuthServiceException authServiceException)
                {
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "HTTP StatusCode {0}", authServiceException.StatusCode))
                        .AppendLine(string.Format(CultureInfo.InvariantCulture, "CorrelationId {0}", authServiceException.CorrelationId));
                }

                if (ex.InnerException != null)
                {
                    sb.AppendLine("---> Inner Exception Details")
                        .AppendLine(GetPiiScrubbedExceptionDetails(ex.InnerException))
                        .AppendLine("=== End of inner exception stack trace ===");
                }

                if (ex.StackTrace != null)
                {
                    sb.Append(Environment.NewLine + ex.StackTrace);
                }
            }

            return sb.ToString();
        }

        private void Log(LogLevel authLogLevel, string messageWithPii, string messageScrubbed)
        {
            if (_loggingCallback == null || authLogLevel >= _logLevel)
            {
                return;
            }

            bool messageWithPiiExists = !string.IsNullOrWhiteSpace(messageWithPii);

            // If we have a message with PII, and PII logging is enabled, use the PII message, else use the scrubbed message.
            bool isLoggingPii = messageWithPiiExists && PiiLoggingEnabled;
            string messageToLog = isLoggingPii ? messageWithPii : messageScrubbed;

            string log = $"{isLoggingPii} AUTH {DateTime.UtcNow} {ClientInformation} {messageToLog}";

            if (_isDefaultPlatformLoggingEnabled)
            {
                switch (authLogLevel)
                {
                    case LogLevel.Error:
                        _platformLogger.Error(log);
                        break;
                    case LogLevel.Warning:
                        _platformLogger.Warning(log);
                        break;
                    case LogLevel.Info:
                        _platformLogger.Information(log);
                        break;
                    case LogLevel.Verbose:
                        _platformLogger.Verbose(log);
                        break;
                }
            }

            _loggingCallback.Invoke(authLogLevel, log, isLoggingPii);
        }
    }
}
