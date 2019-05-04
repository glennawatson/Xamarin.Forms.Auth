// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.Identity.Client.Exceptions;

namespace Xamarin.Forms.Auth
{
    internal class RedirectUriHelper
    {
        /// <summary>
        /// Check common redirect uri problems.
        /// Optionally check that the redirect uri is not the OAuth2 standard redirect uri urn:ietf:wg:oauth:2.0:oob
        /// when using a system browser, because the browser cannot redirect back to the app.
        /// </summary>
        public static void Validate(Uri redirectUri, bool usesSystemBrowser = false)
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

            // Currently only MSAL supports the system browser, on Android and iOS
            if (usesSystemBrowser &&
                Constants.DefaultRedirectUri.Equals(redirectUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
            {
                throw ExceptionFactory.GetClientException(
                    CoreErrorCodes.DefaultRedirectUriIsInvalid,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        CoreErrorMessages.DefaultRedirectUriIsInvalid,
                        Constants.DefaultRedirectUri));
            }
        }

    }
}
