// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A set of services available on the platforms.
    /// </summary>
    internal interface IServiceBundle
    {
        /// <summary>
        /// Gets the selected http manager.
        /// </summary>
        IHttpManager HttpManager { get; }

        /// <summary>
        /// Gets the proxy manager for a platform.
        /// </summary>
        IPlatformProxy PlatformProxy { get; }
    }
}
