// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    ///     Handler enabling your application to send telemetry to your telemetry service or subscription (for instance
    ///     Microsoft Application Insights).
    ///     To enable telemetry in your application, you get the singleton instance of <c>Telemetry</c> by using
    ///     <see cref="Telemetry.GetInstance()" />, you set the delegate that will
    ///     process the telemetry events by calling <see cref="RegisterReceiver(Telemetry.Receiver)" />, and you decide if you
    ///     want to receive telemetry
    ///     events only in case of failure or all the time, by setting the <see cref="TelemetryOnFailureOnly" /> boolean.
    /// </summary>
    public class Telemetry : ITelemetryReceiver
    {
        /// <summary>
        ///     Delegate telling the signature of your callbacks that will send telemetry information to your telemetry service
        /// </summary>
        /// <param name="events">Dictionary of key/values pair</param>
        public delegate void Receiver(List<Dictionary<string, string>> events);

        private static readonly Telemetry Instance = new Telemetry();
        private Receiver _receiver;

        // This is an internal constructor to build isolated unit test instance
        internal Telemetry()
        {
        }

        /// <summary>
        ///     Get the instance of the Telemetry helper for MSAL.NET
        /// </summary>
        /// <returns>a unique instance of <see cref="Telemetry" /></returns>
        public static Telemetry GetInstance()
        {
            return Instance;
        }

        /// <summary>
        ///     Gets or sets a boolean that indicates if telemetry should be generated on failures only (<c>true</c>) or
        ///     all the time (<c>false</c>)
        /// </summary>
        public bool TelemetryOnFailureOnly { get; set; }

        /// <summary>
        ///     Registers one delegate that will send telemetry information to your telemetry service
        /// </summary>
        /// <param name="r">Receiver delegate. See <see cref="Receiver" /></param>
        public void RegisterReceiver(Receiver r)
        {
            _receiver = r;
        }

        /// <summary>
        ///     Checks if a delegate has been registered as a receiver
        /// </summary>
        public bool HasRegisteredReceiver()
        {
            return _receiver != null;
        }

        void ITelemetryReceiver.HandleTelemetryEvents(List<Dictionary<string, string>> events)
        {
            _receiver?.Invoke(events);
        }

        bool ITelemetryReceiver.OnlySendFailureTelemetry
        {
            get => TelemetryOnFailureOnly;
            set => TelemetryOnFailureOnly = value;
        }
   }
}
