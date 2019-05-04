// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    ///     Returns the platform / os specific implementation of a PlatformProxy.
    /// </summary>
    internal class PlatformProxyFactory
    {
        // thread safety ensured by implicit LazyThreadSafetyMode.ExecutionAndPublication
        private static readonly Lazy<IPlatformProxy> PlatformProxyLazy = new Lazy<IPlatformProxy>(
            () =>
#if NET_CORE
            new Microsoft.Identity.Client.Platforms.netcore.NetCorePlatformProxy()
#elif ANDROID
            new Microsoft.Identity.Client.Platforms.Android.AndroidPlatformProxy()
#elif iOS
            new Microsoft.Identity.Client.Platforms.iOS.iOSPlatformProxy()
#elif MAC
            new Platforms.Mac.MacPlatformProxy()
#elif WINDOWS_APP
            new Microsoft.Identity.Client.Platforms.uap.UapPlatformProxy()
#elif FACADE
            new NetStandard11PlatformProxy(IsMsal())
#elif NETSTANDARD1_3
            new Microsoft.Identity.Client.Platforms.netstandard13.Netstandard13PlatformProxy()
#elif DESKTOP
            new Microsoft.Identity.Client.Platforms.net45.NetDesktopPlatformProxy()
#endif
        );

        private PlatformProxyFactory()
        {
        }

        /// <summary>
        ///     Gets the platform proxy, which can be used to perform platform specific operations
        /// </summary>
        public static IPlatformProxy GetPlatformProxy()
        {
            return PlatformProxyLazy.Value;
        }
    }
}
