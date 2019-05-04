// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Callback delegate that allows application developers to consume logs, and handle them in a custom manner. This
    /// callback is set on the <see cref="Logger.LogCallback"/> member of the <see cref="Logger"/> static class.
    /// If <see cref="Logger.PiiLoggingEnabled"/> is set to <c>true</c>, this method will receive the messages twice:
    /// once with the <c>containsPii</c> parameter equals <c>false</c> and the message without PII,
    /// and a second time with the <c>containsPii</c> parameter equals to <c>true</c> and the message might contain PII.
    /// In some cases (when the message does not contain PII), the message will be the same.
    /// </summary>
    /// <param name="level">Log level of the log message to process.</param>
    /// <param name="message">Pre-formatted log message.</param>
    public delegate void LogCallback(LogLevel level, string message);
}
