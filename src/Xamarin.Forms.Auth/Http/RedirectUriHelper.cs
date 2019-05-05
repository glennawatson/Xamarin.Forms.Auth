// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;

namespace Xamarin.Forms.Auth
{
    internal static class RedirectUriHelper
    {
        /// <summary>
        /// Check common redirect uri problems.
        /// Optionally check that the redirect uri is not the OAuth2 standard redirect uri urn:ietf:wg:oauth:2.0:oob
        /// when using a system browser, because the browser cannot redirect back to the app.
        /// </summary>
        /// <param name="redirectUri">The URI to validate.</param>
        public static void Validate(Uri redirectUri)
        {
            if (redirectUri == null)
            {
                throw ExceptionFactory.GetClientException(
                    CoreErrorCodes.NoRedirectUri,
                    CoreErrorMessages.NoRedirectUri);
            }

            if (!string.IsNullOrWhiteSpace(redirectUri.Fragment))
            {
                throw new ArgumentException(
                    CoreErrorMessages.RedirectUriContainsFragment,
                    nameof(redirectUri));
            }
        }
    }
}
