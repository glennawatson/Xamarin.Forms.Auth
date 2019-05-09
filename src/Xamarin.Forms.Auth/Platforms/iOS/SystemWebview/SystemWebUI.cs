// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Foundation;

using SafariServices;
using UIKit;

namespace Xamarin.Forms.Auth
{
    internal class SystemWebUI : WebviewBase, IDisposable
    {
        public RequestContext RequestContext { get; set; }

        public override async Task<AuthorizationResult> AcquireAuthorizationAsync(
            Uri authorizationUri,
            Uri redirectUri,
            RequestContext requestContext,
            CancellationToken cancellationToken)
        {
            ViewController = null;
            InvokeOnMainThread(() =>
            {
                UIWindow window = UIApplication.SharedApplication.KeyWindow;
                ViewController = CoreUIParent.FindCurrentViewController(window.RootViewController);
            });

            ReturnedUriReady = new SemaphoreSlim(0);
            Authenticate(authorizationUri, redirectUri, requestContext);
            await ReturnedUriReady.WaitAsync(cancellationToken).ConfigureAwait(false);

            // dismiss safariviewcontroller
            ViewController.InvokeOnMainThread(() =>
            {
                SafariViewController?.DismissViewController(false, null);
            });

            return AuthorizationResult;
        }

        public void Authenticate(Uri authorizationUri, Uri redirectUri, RequestContext requestContext)
        {
            try
            {
                /* For app center builds, this will need to build on a hosted mac agent. The mac agent does not have the latest SDK's required to build 'ASWebAuthenticationSession'
                * Until the agents are updated, appcenter build will need to ignore the use of 'ASWebAuthenticationSession' for iOS 12.*/
#if !IS_APPCENTER_BUILD
                if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
                {
                    AsWebAuthenticationSession = new AuthenticationServices.ASWebAuthenticationSession(
                        new NSUrl(authorizationUri.AbsoluteUri),
                        redirectUri.Scheme,
                        (callbackUrl, error) =>
                        {
                            if (error != null)
                            {
                                ProcessCompletionHandlerError(error);
                            }
                            else
                            {
                                ContinueAuthentication(callbackUrl.ToString());
                            }
                        });

                    AsWebAuthenticationSession.Start();
                }
                else if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    SfAuthenticationSession = new SFAuthenticationSession(
                        new NSUrl(authorizationUri.AbsoluteUri),
                        redirectUri.Scheme,
                        (callbackUrl, error) =>
                        {
                            if (error != null)
                            {
                                ProcessCompletionHandlerError(error);
                            }
                            else
                            {
                                ContinueAuthentication(callbackUrl.ToString());
                            }
                        });

                    SfAuthenticationSession.Start();
                }
#else
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    sfAuthenticationSession = new SFAuthenticationSession(new NSUrl(authorizationUri.AbsoluteUri),
                        redirectUri.Scheme, (callbackUrl, error) =>
                        {
                            if (error != null)
                            {
                                ProcessCompletionHandlerError(error);
                            }
                            else
                            {
                                ContinueAuthentication(callbackUrl.ToString());
                            }
                        });

                    sfAuthenticationSession.Start();
                }
#endif
                else
                {
                    SafariViewController = new SFSafariViewController(new NSUrl(authorizationUri.AbsoluteUri), false)
                    {
                        Delegate = this
                    };
                    ViewController.InvokeOnMainThread(() =>
                    {
                        ViewController.PresentViewController(SafariViewController, false, null);
                    });
                }
            }
            catch (Exception ex)
            {
                requestContext.Logger.ErrorPii(ex);
                throw new AuthClientException(
                    AuthError.AuthenticationUiFailedError,
                    "Failed to invoke SFSafariViewController",
                    ex);
            }
        }

        [SuppressMessage("Design", "CA1822: Unused parameter", Justification = "Needs to be available.")]
        public void ProcessCompletionHandlerError(NSError error)
        {
            if (ReturnedUriReady != null)
            {
                // The authorizationResult is set on the class and sent back to the InteractiveRequest
                // There it's processed in VerifyAuthorizationResult() and an MsalClientException
                // will be thrown.
                AuthorizationResult = new AuthorizationResult(AuthorizationStatus.UserCancel, null);
                ReturnedUriReady.Release();
            }
        }

        [Export("safariViewControllerDidFinish:")]
        public void DidFinish(SFSafariViewController controller)
        {
            controller.DismissViewController(true, null);

            if (ReturnedUriReady != null)
            {
                AuthorizationResult = new AuthorizationResult(AuthorizationStatus.UserCancel, null);
                ReturnedUriReady.Release();
            }
        }

        public override void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: true);
        }
    }
}
