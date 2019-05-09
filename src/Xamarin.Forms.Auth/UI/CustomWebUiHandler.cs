// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class CustomWebUiHandler : IWebUI
    {
        private readonly ICustomWebUi _customWebUi;

        public CustomWebUiHandler(ICustomWebUi customWebUi)
        {
            _customWebUi = customWebUi;
        }

        /// <inheritdoc />
        public async Task<AuthorizationResult> AcquireAuthorizationAsync(
            Uri authorizationUri,
            Uri redirectUri,
            RequestContext requestContext,
            CancellationToken cancellationToken)
        {
            requestContext.Logger.Info(LogMessages.CustomWebUiAcquiringAuthorizationCode);

            try
            {
                requestContext.Logger.InfoPii(
                    LogMessages.CustomWebUiCallingAcquireAuthorizationCodePii(authorizationUri, redirectUri),
                    LogMessages.CustomWebUiCallingAcquireAuthorizationCodeNoPii);
                var uri = await _customWebUi.AcquireAuthorizationCodeAsync(authorizationUri, redirectUri, cancellationToken)
                                            .ConfigureAwait(false);
                if (uri == null || string.IsNullOrWhiteSpace(uri.Query))
                {
                    throw new AuthClientException(
                        AuthError.CustomWebUiReturnedInvalidUri,
                        AuthErrorMessage.CustomWebUiReturnedInvalidUri);
                }

                if (uri.Authority.Equals(redirectUri.Authority, StringComparison.OrdinalIgnoreCase) &&
                    uri.AbsolutePath.Equals(redirectUri.AbsolutePath, StringComparison.OrdinalIgnoreCase))
                {
                    IDictionary<string, string> inputQp = CoreHelpers.ParseKeyValueList(
                        authorizationUri.Query.Substring(1),
                        '&',
                        true,
                        null);

                    requestContext.Logger.Info(LogMessages.CustomWebUiRedirectUriMatched);
                    return new AuthorizationResult(AuthorizationStatus.Success, uri.OriginalString);
                }

                throw new AuthClientException(
                    AuthError.CustomWebUiRedirectUriMismatch,
                    AuthErrorMessage.CustomWebUiRedirectUriMismatch(
                        uri.AbsolutePath,
                        redirectUri.AbsolutePath));
            }
            catch (OperationCanceledException)
            {
                requestContext.Logger.Info(LogMessages.CustomWebUiOperationCancelled);
                return new AuthorizationResult(AuthorizationStatus.UserCancel, null);
            }
            catch (Exception ex)
            {
                requestContext.Logger.WarningPiiWithPrefix(ex, AuthErrorMessage.CustomWebUiAuthorizationCodeFailed);
                throw;
            }
        }

        /// <inheritdoc />
        public void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: false);
        }
    }
}
