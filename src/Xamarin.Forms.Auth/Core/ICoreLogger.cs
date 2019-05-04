// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Represents a internal logging.
    /// </summary>
    internal interface ICoreLogger
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        Guid CorrelationId { get; set; }

        /// <summary>
        /// Logs a error message.
        /// </summary>
        /// <param name="messageScrubbed">The message.</param>
        void Error(string messageScrubbed);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="messageScrubbed">The message.</param>
        void Warning(string messageScrubbed);

        /// <summary>
        /// Logs a info message.
        /// </summary>
        /// <param name="messageScrubbed">The message.</param>
        void Info(string messageScrubbed);
    }
}
