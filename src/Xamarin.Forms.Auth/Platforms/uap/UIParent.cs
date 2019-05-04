// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains UI properties for interactive flows, such as the parent window (on Windows), or the parent activity (on Xamarin.Android), and
    /// which browser to use (on Xamarin.Android and Xamarin.iOS).
    /// </summary>
    public sealed class UIParent
    {
        /// <summary>
        /// Initializes static members of the <see cref="UIParent"/> class.
        /// </summary>
        static UIParent()
        {
            ModuleInitializer.EnsureModuleInitialized();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIParent"/> class.
        /// </summary>
        public UIParent()
        {
            CoreUIParent = new CoreUIParent();
        }

        internal CoreUIParent CoreUIParent { get; }

        // hidden webview can be used in both WinRT and desktop applications.
        internal bool UseHiddenBrowser
        {
            get => CoreUIParent.UseHiddenBrowser;
            set => CoreUIParent.UseHiddenBrowser = value;
        }

        internal bool UseCorporateNetwork
        {
            get => CoreUIParent.UseCorporateNetwork;
            set => CoreUIParent.UseCorporateNetwork = value;
        }
    }
}
