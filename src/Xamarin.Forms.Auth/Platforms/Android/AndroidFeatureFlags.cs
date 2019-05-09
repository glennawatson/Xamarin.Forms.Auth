// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class AndroidFeatureFlags : IFeatureFlags
    {
        /// <summary>
        /// Gets a value indicating whether FOCI is supported.
        /// </summary>
        /// <remarks>FOCI is not currently supported on Android because app metadata serialization is not defined.</remarks>
        public bool IsFociEnabled => false;
    }
}
