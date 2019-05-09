// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A factory that generates new Guid values.
    /// </summary>
    internal interface IGuidFactory
    {
        /// <summary>
        /// Generate a new Guid.
        /// </summary>
        /// <returns>The Guid.</returns>
        Guid NewGuid();
    }
}
