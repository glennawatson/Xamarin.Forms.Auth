// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A service bundle which contains information that can be different per platform.
    /// </summary>
    internal interface IServiceBundle
    {
        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        IApplicationConfiguration Config { get; }

        /// <summary>
        /// Gets the default logger.
        /// </summary>
        ICoreLogger DefaultLogger { get; }

        /// <summary>
        /// Gets the HTTP manager which will handle HTTP clients.
        /// </summary>
        IHttpManager HttpManager { get; }

        /// <summary>
        /// Gets the platform proxy, which will contain platform specific implementations.
        /// </summary>
        IPlatformProxy PlatformProxy { get; }
    }
}
