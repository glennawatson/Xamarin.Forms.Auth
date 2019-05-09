// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using Foundation;

namespace Xamarin.Forms.Auth
{
    internal sealed class MacEmbeddedWebUI : IWebUI, IDisposable
    {
        private SemaphoreSlim _returnedUriReady;
        private AuthorizationResult _authorizationResult;

        public CoreUIParent CoreUIParent { get; set; }

        public RequestContext RequestContext { get; set; }

        public async Task<AuthorizationResult> AcquireAuthorizationAsync(
            Uri authorizationUri,
            Uri redirectUri,
            RequestContext requestContext,
            CancellationToken cancellationToken)
        {
            _returnedUriReady = new SemaphoreSlim(0);
            Authenticate(authorizationUri, redirectUri, requestContext);
            await _returnedUriReady.WaitAsync(cancellationToken).ConfigureAwait(false);

            return _authorizationResult;
        }

        public void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _returnedUriReady?.Dispose();
        }

        private void SetAuthorizationResult(AuthorizationResult authorizationResultInput)
        {
            _authorizationResult = authorizationResultInput;
            _returnedUriReady.Release();
        }

        private void Authenticate(Uri authorizationUri, Uri redirectUri, RequestContext requestContext)
        {
            try
            {
                // Ensure we create the NSViewController on the main thread.
                // Consumers of our library must ensure they do not block the main thread
                // or else they will cause a deadlock.
                // For example calling `AcquireTokenAsync(...).Result` from the main thread
                // would result in this delegate never executing.
                NSRunLoop.Main.BeginInvokeOnMainThread(() =>
                {
                    var windowController = new AuthenticationAgentNSWindowController(
                        authorizationUri.AbsoluteUri,
                        redirectUri.OriginalString,
                        SetAuthorizationResult);
                    windowController.Run(CoreUIParent.CallerWindow);
                });
            }
            catch (Exception ex)
            {
                throw new AuthClientException(
                    AuthError.AuthenticationUiFailed,
                    "See inner exception for details",
                    ex);
            }
        }
    }
}
