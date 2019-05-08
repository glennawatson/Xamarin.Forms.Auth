// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    ///     Returns the platform / os specific implementation of a PlatformProxy.
    /// </summary>
    internal class PlatformProxyFactory
    {
        // thread safety ensured by implicit LazyThreadSafetyMode.ExecutionAndPublication
        private static readonly Lazy<IPlatformProxy> PlatformProxyLazy = new Lazy<IPlatformProxy>(
            () => new PlatformProxy());

        private PlatformProxyFactory()
        {
        }

        /// <summary>
        ///     Gets the platform proxy, which can be used to perform platform specific operations.
        /// </summary>
        public static IPlatformProxy GetPlatformProxy()
        {
            return PlatformProxyLazy.Value;
        }
    }
}
