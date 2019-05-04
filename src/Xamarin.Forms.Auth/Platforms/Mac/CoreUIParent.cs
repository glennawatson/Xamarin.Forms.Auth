// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppKit;

namespace Xamarin.Forms.Auth.Platforms.Mac
{
    internal class CoreUIParent
    {
        public CoreUIParent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreUIParent"/> class.
        /// </summary>
        /// <param name="callerWindow">Caller window. OPTIONAL.</param>
        public CoreUIParent(NSWindow callerWindow)
        {
            CallerWindow = callerWindow;
        }

        /// <summary>
        /// Gets or sets the Caller NSWindow.
        /// </summary>
        public NSWindow CallerWindow { get; set; }
    }
}
