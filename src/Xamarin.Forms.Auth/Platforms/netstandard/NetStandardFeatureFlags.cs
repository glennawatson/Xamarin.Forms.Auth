// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// These control the behaviour of platforms targetting directly NetStandard (e.g. WinRT).
    /// </summary>
    internal class NetStandardFeatureFlags : IFeatureFlags
    {
        public bool IsFociEnabled => true;
    }
}
