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
        /// <param name="redirectUri">The uri to validate.</param>
        /// <param name="usesSystemBrowser">If we are using the system browser.</param>
        public static void Validate(Uri redirectUri, bool usesSystemBrowser = false)
        {
            if (redirectUri == null)
            {
                throw new AuthClientException(
                    AuthError.NoRedirectUri,
                    AuthErrorMessage.NoRedirectUri);
            }

            if (!string.IsNullOrWhiteSpace(redirectUri.Fragment))
            {
                throw new ArgumentException(
                    AuthErrorMessage.RedirectUriContainsFragment,
                    nameof(redirectUri));
            }

            if (usesSystemBrowser &&
                Constants.DefaultRedirectUri.Equals(redirectUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
            {
                throw new AuthClientException(
                    AuthError.DefaultRedirectUriIsInvalid,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        AuthErrorMessage.DefaultRedirectUriIsInvalid,
                        Constants.DefaultRedirectUri));
            }
        }
    }
}
