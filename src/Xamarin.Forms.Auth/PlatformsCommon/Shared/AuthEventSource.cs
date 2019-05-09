// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.Tracing;

namespace Xamarin.Forms.Auth
{
    [EventSource(Name = "Xamarin.Forms.Auth")]
    internal class AuthEventSource : EventSource
    {
        [Event(1, Level = EventLevel.Verbose)]
        internal void Verbose(string message)
        {
            WriteEvent(1, message);
        }

        [Event(2, Level = EventLevel.Informational)]
        internal void Information(string message)
        {
            WriteEvent(2, message);
        }

        [Event(3, Level = EventLevel.Warning)]
        internal void Warning(string message)
        {
            WriteEvent(3, message);
        }

        [Event(4, Level = EventLevel.Error)]
        internal void Error(string message)
        {
            WriteEvent(4, message);
        }
    }
}
