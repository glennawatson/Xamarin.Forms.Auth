// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Foundation;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Static class that consumes the response from the Authentication flow and continues token acquisition. This class should be called in ApplicationDelegate whenever app loads/reloads.
    /// </summary>
    [CLSCompliant(false)]
    public static class AuthenticationContinuationHelper
    {
        /// <summary>
        /// Sets response for continuing authentication flow. This function will return true if the response was meant for MSAL, else it will return false.
        /// </summary>
        /// <param name="url">url used to invoke the application</param>
        public static bool SetAuthenticationContinuationEventArgs(NSUrl url)
        {
            return WebviewBase.ContinueAuthentication(url.AbsoluteString);
        }
    }
}
