// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;

namespace Xamarin.Forms.Auth
{
    internal class OAuth2Logger : ICoreLogger
    {
        private readonly IPlatformLogger _platformLogger;

        internal MsalLogger(Guid correlationId, string component)
        {
            CorrelationId = correlationId;
            _platformLogger = PlatformProxyFactory.GetPlatformProxy().PlatformLogger;
            Component = string.Empty;
            if (!string.IsNullOrEmpty(component))
            {
                // space is intentional for formatting of the message
                Component = string.Format(CultureInfo.InvariantCulture, " ({0})", component);
            }
        }

        public static ICoreLogger Default { get; set; }

        public Guid CorrelationId { get; set; }

        internal string Component { get; }

        public void Info(string messageScrubbed)
        {
            Log(LogLevel.Info, messageScrubbed);
        }

        public void Verbose(string messageScrubbed)
        {
            Log(LogLevel.Verbose, messageScrubbed);
        }

        public void Warning(string messageScrubbed)
        {
            Log(LogLevel.Warning, messageScrubbed);
        }

        public void Error(string messageScrubbed)
        {
            Log(LogLevel.Error, messageScrubbed);
        }

        private static void ExecuteCallback(LogLevel level, string message)
        {
            lock (Logger.LockObj)
            {
                Logger.LogCallback?.Invoke(level, message);
            }
        }

        private void Log(LogLevel logLevel, string messageScrubbed)
        {
            if (logLevel > Logger.Level)
            {
                return;
            }

            // format log message;
            string correlationId = CorrelationId.Equals(Guid.Empty)
                ? string.Empty
                : " - " + CorrelationId;

            // If we have a message with PII, and PII logging is enabled, use the PII message, else use the scrubbed message.
            string log = $"OAUTH2 [{DateTime.UtcNow}{correlationId}]{Component} {messageScrubbed}";

            if (Logger.DefaultLoggingEnabled)
            {
                switch (Logger.Level)
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

            ExecuteCallback(logLevel, log);
        }
    }
}
