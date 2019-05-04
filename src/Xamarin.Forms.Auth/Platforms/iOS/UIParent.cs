// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains UI properties for interactive flows, such as the parent window (on Windows), or the parent activity (on Xamarin.Android), and
    /// which browser to use (on Xamarin.Android and Xamarin.iOS)
    /// </summary>
    public sealed class UIParent
    {
        static UIParent()
        {
            ModuleInitializer.EnsureModuleInitialized();
        }

        internal CoreUIParent CoreUIParent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// </summary>
        public UIParent()
        {
            CoreUIParent = new CoreUIParent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// </summary>
        /// <remarks>This method is likely to be removed (replaced) before final release.</remarks>
        public UIParent(bool useEmbeddedWebview)
            : this()
        {
            CoreUIParent.UseEmbeddedWebview = useEmbeddedWebview;
        }

#if iOS_RUNTIME
        /// <summary>
        /// Platform agnostic constructor that allows building an UIParent from a NetStandard assembly.
        /// On iOS, the parent is ignored, you can pass null.
        /// </summary>
        /// <remarks>This constructor is only avaiable at runtime, to provide support for NetStandard</remarks>
        /// <param name="parent">Ignored on iOS</param>
        /// <param name="useEmbeddedWebview">Flag to determine between embedded vs system browser. See https://aka.ms/msal-net-uses-web-browser </param>
        public UIParent(object parent, bool useEmbeddedWebview) :
            this(useEmbeddedWebview)
        {
        }
#endif

        /// <summary>
        /// Checks if the system weview can be used.
        /// Currently, on iOS, only the embedded webview is available, so this always returns false.
        /// </summary>
        public static bool IsSystemWebviewAvailable() // This is part of the NetStandard "interface"
        {
            return true;
        }

    }
}
