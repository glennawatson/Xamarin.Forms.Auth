// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class EventSourcePlatformLogger : IPlatformLogger
    {
        static EventSourcePlatformLogger()
        {
            EventSource = new OAuth2EventSource();
        }

        internal static OAuth2EventSource EventSource { get; }

        public void Error(string message)
        {
            EventSource.Error(message);
        }

        public void Warning(string message)
        {
            EventSource.Error(message);
        }

        public void Verbose(string message)
        {
            EventSource.Error(message);
        }

        public void Information(string message)
        {
            EventSource.Error(message);
        }
    }
}
