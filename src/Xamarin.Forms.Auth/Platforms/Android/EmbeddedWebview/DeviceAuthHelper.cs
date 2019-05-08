// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    [global::Android.Runtime.Preserve(AllMembers = true)]
    internal static class DeviceAuthHelper
    {
        public static bool CanHandleDeviceAuthChallenge => false;

        public static Task<string> CreateDeviceAuthChallengeResponseAsync(IDictionary<string, string> challengeData)
        {
            return Task.FromResult(string.Format(CultureInfo.InvariantCulture, @"PKeyAuth Context=""{0}"",Version=""{1}""", challengeData[BrokerConstants.ChallangeResponseContext], challengeData[BrokerConstants.ChallangeResponseVersion]));
        }
    }
}
