// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// An exception that happens when there is an issue with a extension.
    /// </summary>
    [Serializable]
    public class AuthExtensionException : AuthException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthExtensionException"/> class.
        /// </summary>
        /// <param name="message">The message about the error.</param>
        public AuthExtensionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthExtensionException"/> class.
        /// </summary>
        /// <param name="message">The message about the error.</param>
        /// <param name="innerException">An additional exception with extra context.</param>
        public AuthExtensionException(string message, Exception innerException)
            : base(message, string.Empty, innerException)
        {
        }
    }
}
