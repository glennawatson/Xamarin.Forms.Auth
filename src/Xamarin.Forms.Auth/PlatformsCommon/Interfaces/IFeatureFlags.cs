// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains the ability to get feature flags.
    /// </summary>
    internal interface IFeatureFlags
    {
        /// <summary>
        /// Gets a value indicating whether FOCI is enabled.
        /// </summary>
        bool IsFociEnabled { get; }
    }
}
