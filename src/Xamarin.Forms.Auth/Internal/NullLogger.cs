// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    internal class NullLogger : ICoreLogger
    {
        public string ClientName { get; } = string.Empty;

        public string ClientVersion { get; } = string.Empty;

        public Guid CorrelationId { get; } = Guid.Empty;

        public bool PiiLoggingEnabled { get; }

        public void Error(string messageScrubbed)
        {
        }

        public void ErrorPii(string messageWithPii, string messageScrubbed)
        {
        }

        public void ErrorPii(Exception exWithPii)
        {
        }

        public void ErrorPiiWithPrefix(Exception exWithPii, string prefix)
        {
        }

        public void Warning(string messageScrubbed)
        {
        }

        public void WarningPii(string messageWithPii, string messageScrubbed)
        {
        }

        public void WarningPii(Exception exWithPii)
        {
        }

        public void WarningPiiWithPrefix(Exception exWithPii, string prefix)
        {
        }

        public void Info(string messageScrubbed)
        {
        }

        public void InfoPii(string messageWithPii, string messageScrubbed)
        {
        }

        public void InfoPii(Exception exWithPii)
        {
        }

        public void InfoPiiWithPrefix(Exception exWithPii, string prefix)
        {
        }

        public void Verbose(string messageScrubbed)
        {
        }

        public void VerbosePii(string messageWithPii, string messageScrubbed)
        {
        }
    }
}
