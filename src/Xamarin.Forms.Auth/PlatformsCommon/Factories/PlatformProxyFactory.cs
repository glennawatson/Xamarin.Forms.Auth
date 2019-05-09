// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    ///     Returns the platform / os specific implementation of a PlatformProxy.
    /// </summary>
    internal static class PlatformProxyFactory
    {
        /// <summary>
        ///     Gets the platform proxy, which can be used to perform platform specific operations.
        /// </summary>
        /// <param name="logger">The logger to use on the platform.</param>
        /// <returns>The platform proxy.</returns>
        public static IPlatformProxy CreatePlatformProxy(ICoreLogger logger)
        {
            var finalLogger = logger ?? AuthLogger.NullLogger;

            return new PlatformProxy(finalLogger);
        }
    }
}
