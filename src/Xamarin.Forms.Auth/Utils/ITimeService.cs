// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A factory which gets the time.
    /// </summary>
    internal interface ITimeService
    {
        /// <summary>
        /// Gets the UTC time for Now.
        /// </summary>
        /// <returns>The date and time.</returns>
        DateTime GetUtcNow();
    }
}
