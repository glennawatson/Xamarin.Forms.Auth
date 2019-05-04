// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Static class that allows application developers to set a callback to handle logs, specify the level
    /// of logs desired and if they accept to log Personally Identifiable Information (PII) or not
    /// </summary>
    /// <example>
    /// <code>
    /// private static void Log(LogLevel level, string message, bool containsPii)
    /// {
    ///  if (containsPii)
    ///  {
    ///   Console.ForegroundColor = ConsoleColor.Red;
    ///  }
    ///   Console.WriteLine($"{level} {message}");
    ///   Console.ResetColor();
    ///  }
    ///
    /// private async Task CallProtectedApiWithLoggingAsync(string[] args)
    /// {
    ///  PublicClientApplication application = new PublicClientApplication(clientID);
    ///  Logger.LogCallback = Log;
    ///  Logger.Level = LogLevel.Info;
    ///  Logger.PiiLoggingEnabled = true;
    ///  AuthenticationResult result = await application.AcquireTokenAsync(
    ///                                             new string[] { "User.Read" });
    ///  ...
    /// }
    /// </code>
    /// </example>
    public sealed class Logger
    {
        internal static readonly object LockObj = new object();

        private static volatile LogCallback _logCallback;

        /// <summary>
        /// Gets or sets a callback instance that you can set in your app to consume and publish logs in a custom manner.
        /// </summary>
        /// <exception cref="ArgumentException">will be thrown if the LogCallback was already set</exception>
        public static LogCallback LogCallback
        {
            get
            {
                lock (LockObj)
                {
                    return _logCallback;
                }
            }

            set
            {
                lock (LockObj)
                {
                    _logCallback = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the level of logging you want. The default value is <see cref="LogLevel.Info"/>. Setting it to <see cref="LogLevel.Error"/> will only get errors
        /// Setting it to <see cref="LogLevel.Warning"/> will get errors and warning, etc..
        /// </summary>
        public static LogLevel Level { get; set; } = LogLevel.Info;

        /// <summary>
        /// Gets or sets a value indicating whether to enable/disable logging to platform defaults. In Desktop/UWP, Event Tracing is used. In iOS, NSLog is used.
        /// In android, logcat is used. The default value is <c>false</c>.
        /// </summary>
        public static bool DefaultLoggingEnabled { get; set; }
    }
}
