// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A logger for the platform.
    /// </summary>
    internal interface IPlatformLogger
    {
        /// <summary>
        /// Logs a error.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Error(string message);

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Warning(string message);

        /// <summary>
        /// Logs a verbose message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Verbose(string message);

        /// <summary>
        /// Logs a informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Information(string message);
    }
}
