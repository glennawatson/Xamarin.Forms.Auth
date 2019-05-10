// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Callback delegate that allows application developers to consume logs, and handle them in a custom manner. This
    /// callback is set using <see cref="AbstractApplicationBuilder{T}.WithLogging(LogCallback, LogLevel?, bool?, bool?)"/>.
    /// If <c>PiiLoggingEnabled</c> is set to <c>true</c>, when registering the callback this method will receive the messages twice:
    /// once with the <c>containsPii</c> parameter equals <c>false</c> and the message without PII,
    /// and a second time with the <c>containsPii</c> parameter equals to <c>true</c> and the message might contain PII.
    /// In some cases (when the message does not contain PII), the message will be the same.
    /// </summary>
    /// <param name="level">Log level of the log message to process.</param>
    /// <param name="message">Pre-formatted log message.</param>
    /// <param name="containsPii">Indicates if the log message contains Organizational Identifiable Information (OII)
    /// or Personally Identifiable Information (PII) nor not.
    /// If <see cref="ICoreLogger.PiiLoggingEnabled"/> is set to <c>false</c> then this value is always false.
    /// Otherwise it will be <c>true</c> when the message contains PII.</param>
    /// <seealso cref="ICoreLogger"/>
    public delegate void LogCallback(LogLevel level, string message, bool containsPii);
}
