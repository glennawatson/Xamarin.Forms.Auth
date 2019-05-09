// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A logger for us.
    /// </summary>
    internal interface ICoreLogger
    {
        /// <summary>
        /// Gets a value indicating whether we should log personal information.
        /// </summary>
        bool PiiLoggingEnabled { get; }

        /// <summary>
        /// Logs a error without personal information.
        /// </summary>
        /// <param name="messageScrubbed">The scrubbed message to log.</param>
        void Error(string messageScrubbed);

        /// <summary>
        /// Logs a error with both personal information and scrubbed information.
        /// </summary>
        /// <param name="messageWithPii">The message with personal information.</param>
        /// <param name="messageScrubbed">The message without personal information.</param>
        void ErrorPii(string messageWithPii, string messageScrubbed);

        /// <summary>
        /// Logs a exception as a error which may contain personal information.
        /// </summary>
        /// <param name="exWithPii">The exception with potential personal information.</param>
        void ErrorPii(Exception exWithPii);

        /// <summary>
        /// Logs a exception (with a prefix) as a error which may contain personal information.
        /// </summary>
        /// <param name="exWithPii">The exception with potential personal information.</param>
        /// <param name="prefix">The prefix to append.</param>
        void ErrorPiiWithPrefix(Exception exWithPii, string prefix);

        /// <summary>
        /// Logs a warning without personal information.
        /// </summary>
        /// <param name="messageScrubbed">The scrubbed message to log.</param>
        void Warning(string messageScrubbed);

        /// <summary>
        /// Logs a warning with both personal information and scrubbed information.
        /// </summary>
        /// <param name="messageWithPii">The message with personal information.</param>
        /// <param name="messageScrubbed">The message without personal information.</param>
        void WarningPii(string messageWithPii, string messageScrubbed);

        /// <summary>
        /// Logs a exception as a warning which may contain personal information.
        /// </summary>
        /// <param name="exWithPii">The exception with potential personal information.</param>
        void WarningPii(Exception exWithPii);

        /// <summary>
        /// Logs a exception (with a prefix) as a warning which may contain personal information.
        /// </summary>
        /// <param name="exWithPii">The exception with potential personal information.</param>
        /// <param name="prefix">The prefix to append.</param>
        void WarningPiiWithPrefix(Exception exWithPii, string prefix);

        /// <summary>
        /// Logs information without personal information.
        /// </summary>
        /// <param name="messageScrubbed">The scrubbed message to log.</param>
        void Info(string messageScrubbed);

        /// <summary>
        /// Logs information with both personal information and scrubbed information.
        /// </summary>
        /// <param name="messageWithPii">The message with personal information.</param>
        /// <param name="messageScrubbed">The message without personal information.</param>
        void InfoPii(string messageWithPii, string messageScrubbed);

        /// <summary>
        /// Logs a exception as information which may contain personal information.
        /// </summary>
        /// <param name="exWithPii">The exception with potential personal information.</param>
        void InfoPii(Exception exWithPii);

        /// <summary>
        /// Logs a exception (with a prefix) as information which may contain personal information.
        /// </summary>
        /// <param name="exWithPii">The exception with potential personal information.</param>
        /// <param name="prefix">The prefix to append.</param>
        void InfoPiiWithPrefix(Exception exWithPii, string prefix);

        /// <summary>
        /// Logs verbose information without personal information.
        /// </summary>
        /// <param name="messageScrubbed">The scrubbed message to log.</param>
        void Verbose(string messageScrubbed);

        /// <summary>
        /// Logs verbse information with both personal information and scrubbed information.
        /// </summary>
        /// <param name="messageWithPii">The message with personal information.</param>
        /// <param name="messageScrubbed">The message without personal information.</param>
        void VerbosePii(string messageWithPii, string messageScrubbed);
    }
}
