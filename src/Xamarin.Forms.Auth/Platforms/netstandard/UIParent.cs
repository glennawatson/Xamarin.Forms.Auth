// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains UI properties for interactive flows, such as the parent window (on Windows), or the parent activity (on Xamarin.Android), and
    /// which browser to use (on Xamarin.Android and Xamarin.iOS).
    /// </summary>
    public sealed class UIParent
    {
        static UIParent()
        {
            ModuleInitializer.EnsureModuleInitialized();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// Platform agnostic default constructor.
        /// </summary>
        public UIParent()
        {
            CoreUIParent = new CoreUIParent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// Platform agnostic constructor that allows building an UIParent from a NetStandard assembly.
        /// </summary>
        /// <remarks>Interactive auth is not currently implemented in .net core.</remarks>
        /// <param name="parent">An owner window to which to attach the webview to.
        /// On Android, it is mandatory to pass in an Activity.
        /// On all other platforms, it is not required.
        /// On .net desktop, it is optional - you can either pass a System.Windows.Forms.IWin32Window or an System.IntPtr
        /// to a window handle or null. This is used to center the webview. </param>
        /// <param name="useEmbeddedWebview">Flag to determine between embedded vs system browser. Currently affects only iOS and Android. See https://aka.ms/msal-net-uses-web-browser. </param>
        public UIParent(object parent, bool useEmbeddedWebview)
        {
            ThrowPlatformNotSupported();
        }

        internal CoreUIParent CoreUIParent { get; }

        /// <summary>
        /// Checks if the system weview can be used.
        /// </summary>
        /// <returns>If the system web view is available.</returns>
        public static bool IsSystemWebviewAvailable() // This is part of the NetStandard "interface"
        {
            ThrowPlatformNotSupported();
            return false;
        }

        /// <summary>
        /// For the rare case when an application actually uses the netstandard implementation
        /// i.e. other frameworks - e.g. Xamarin.MAC - or MSAL.netstandard loaded via reflection.
        /// </summary>
        private static void ThrowPlatformNotSupported()
        {
            throw new PlatformNotSupportedException("Interactive Authentication flows are not supported when the NetStandard assembly is used at runtime. " +
                                                    "Consider using Device Code Flow https://aka.ms/msal-device-code-flow or " +
                                                    "Integrated Windows Auth https://aka.ms/msal-net-iwa");
        }
    }
}
