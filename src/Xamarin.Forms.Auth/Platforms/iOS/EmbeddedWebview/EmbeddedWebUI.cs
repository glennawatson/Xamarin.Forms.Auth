// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Xamarin.Forms.Auth
{
    internal class EmbeddedWebUI : WebviewBase, IDisposable
    {
        public RequestContext RequestContext { get; internal set; }
        public CoreUIParent CoreUIParent { get; set; }

        public async override Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext)
        {
            ReturnedUriReady = new SemaphoreSlim(0);
            Authenticate(authorizationUri, redirectUri, requestContext);
            await ReturnedUriReady.WaitAsync().ConfigureAwait(false);

            return AuthorizationResult;
        }

        public static void SetAuthorizationResult(AuthorizationResult authorizationResultInput)
        {
            AuthorizationResult = authorizationResultInput;
            ReturnedUriReady.Release();
        }

        public void Authenticate(Uri authorizationUri, Uri redirectUri, RequestContext requestContext)
        {
            UIViewController viewController = null;
            InvokeOnMainThread(() =>
            {
                UIWindow window = UIApplication.SharedApplication.KeyWindow;
                viewController = CoreUIParent.FindCurrentViewController(window.RootViewController);
            });
            try
            {
                viewController.InvokeOnMainThread(() =>
                {
                    var navigationController =
                        new MsalAuthenticationAgentUINavigationController(authorizationUri.AbsoluteUri,
                            redirectUri.OriginalString, CallbackMethod, CoreUIParent.PreferredStatusBarStyle)
                        {
                            ModalPresentationStyle = CoreUIParent.ModalPresentationStyle,
                            ModalTransitionStyle = CoreUIParent.ModalTransitionStyle,
                            TransitioningDelegate = viewController.TransitioningDelegate
                        };

                    viewController.PresentViewController(navigationController, true, null);
                });
            }
            catch (Exception ex)
            {
                throw MsalExceptionFactory.GetClientException(
                    CoreErrorCodes.AuthenticationUiFailed,
                    "See inner exception for details",
                    ex);
            }
        }

        private static void CallbackMethod(AuthorizationResult result)
        {
            SetAuthorizationResult(result);
        }

        public override void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: false);
        }

    }
}
