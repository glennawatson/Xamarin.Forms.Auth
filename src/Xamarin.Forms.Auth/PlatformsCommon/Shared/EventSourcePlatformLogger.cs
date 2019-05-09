// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class EventSourcePlatformLogger : IPlatformLogger
    {
        static EventSourcePlatformLogger()
        {
            MsalEventSource = new AuthEventSource();
        }

        internal static AuthEventSource MsalEventSource { get; }

        public void Error(string message)
        {
            MsalEventSource.Error(message);
        }

        public void Warning(string message)
        {
            MsalEventSource.Error(message);
        }

        public void Verbose(string message)
        {
            MsalEventSource.Error(message);
        }

        public void Information(string message)
        {
            MsalEventSource.Error(message);
        }
    }
}
