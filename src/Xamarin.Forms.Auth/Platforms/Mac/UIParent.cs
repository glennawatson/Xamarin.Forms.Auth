// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using AppKit;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Allows for configuration of the web UI experience.
    /// </summary>
    public sealed class UIParent
    {
        static UIParent()
        {
            ModuleInitializer.EnsureModuleInitialized();
        }

        internal CoreUIParent CoreUIParent { get; }

        /// <summary>
        /// Default constructor. Uses the NSApplication.SharedApplication.MainWindow to parent the web ui.
        /// </summary>
        public UIParent()
        {
            CoreUIParent = new CoreUIParent(NSApplication.SharedApplication.MainWindow);
        }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
                              /// <summary>
                              /// Create a UIParent given an instance of NSWindow, which will be used to parent the web ui.
                              /// </summary>
        public UIParent(NSWindow callerWindow)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            if (callerWindow == null)
            {
                callerWindow = NSApplication.SharedApplication.MainWindow;
            }

            CoreUIParent = new CoreUIParent(callerWindow);
        }


#if MAC_RUNTIME
        /// <summary>
        /// Platform agnostic constructor that allows building an UIParent from a NetStandard assembly.
        /// </summary>
        /// <remarks>This constructor is only avaiable at runtime, to provide support for NetStandard</remarks>
        /// <param name="parent">Expected to be a NSWindow instance. Passing null implies the MainWindow will be used</param>
        /// <param name="useEmbeddedWebview">Ignored, the embedded view is always used on Mac</param>
        public UIParent(object parent, bool useEmbeddedWebview) :
            this(ValidateParentObject(parent))
        {
            
        }
#endif

        /// <summary>
        /// Checks if the system weview can be used.
        /// Currently, on MAC, only the embedded webview is available, so this always returns false.
        /// </summary>
        public static bool IsSystemWebviewAvailable() // This is part of the NetStandard "interface"
        {
            return false;
        }

        private static NSWindow ValidateParentObject(object parent)
        {
            if (parent == null)
            {
                return null;
            }

            NSWindow parentActivity = parent as NSWindow;
            if (parentActivity == null)
            {
                throw new ArgumentException(nameof(parent) +
                                            " is expected to be of type NSWindow but is of type " +
                                            parent.GetType());
            }

            return parentActivity;
        }

    }
}
