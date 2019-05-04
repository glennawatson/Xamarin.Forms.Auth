// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking.Connectivity;
using Windows.Security.Authentication.Web;

namespace Xamarin.Forms.Auth
{
    internal class WebUI : IWebUI
    {
        private readonly bool useCorporateNetwork;
        private readonly bool silentMode;

        public RequestContext RequestContext { get; set; }

        public WebUI(CoreUIParent parent, RequestContext requestContext)
        {
            useCorporateNetwork = parent.UseCorporateNetwork;
            silentMode = parent.UseHiddenBrowser;
        }

        public async Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri,
            RequestContext requestContext)
        {
            bool ssoMode = string.Equals(redirectUri.OriginalString, Constants.UapWEBRedirectUri, StringComparison.OrdinalIgnoreCase);

            WebAuthenticationResult webAuthenticationResult;
            WebAuthenticationOptions options = (useCorporateNetwork &&
                                                (ssoMode || redirectUri.Scheme == Constants.MsAppScheme))
                ? WebAuthenticationOptions.UseCorporateNetwork
                : WebAuthenticationOptions.None;

            ThrowOnNetworkDown();
            if (silentMode)
            {
                options |= WebAuthenticationOptions.SilentMode;
            }

            try
            {
                webAuthenticationResult = await CoreApplication.MainView.CoreWindow.Dispatcher.RunTaskAsync(
                        async () =>
                        {
                            if (ssoMode)
                            {
                                return await
                                    WebAuthenticationBroker.AuthenticateAsync(options, authorizationUri)
                                        .AsTask()
                                        .ConfigureAwait(false);
                            }
                            else
                            {
                                return await WebAuthenticationBroker
                                    .AuthenticateAsync(options, authorizationUri, redirectUri)
                                    .AsTask()
                                    .ConfigureAwait(false);
                            }
                        })
                    .ConfigureAwait(false);
            }

            catch (Exception ex)
            {
                requestContext.Logger.ErrorPii(ex);
                throw new MsalException(MsalClientException.AuthenticationUiFailedError, "WAB authentication failed",
                    ex);
            }

            AuthorizationResult result = ProcessAuthorizationResult(webAuthenticationResult, requestContext);

            return result;
        }

        private void ThrowOnNetworkDown()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            var isConnected = (profile != null
                               && profile.GetNetworkConnectivityLevel() ==
                               NetworkConnectivityLevel.InternetAccess);
            if (!isConnected)
            {
                throw new MsalClientException(MsalClientException.NetworkNotAvailableError, MsalErrorMessage.NetworkNotAvailable);
            }
        }

        private static AuthorizationResult ProcessAuthorizationResult(WebAuthenticationResult webAuthenticationResult,
            RequestContext requestContext)
        {
            AuthorizationResult result;
            switch (webAuthenticationResult.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    result = new AuthorizationResult(AuthorizationStatus.Success, webAuthenticationResult.ResponseData);
                    break;
                case WebAuthenticationStatus.ErrorHttp:
                    result = new AuthorizationResult(AuthorizationStatus.ErrorHttp,
                        webAuthenticationResult.ResponseErrorDetail.ToString(CultureInfo.InvariantCulture));
                    break;
                case WebAuthenticationStatus.UserCancel:
                    result = new AuthorizationResult(AuthorizationStatus.UserCancel, null);
                    break;
                default:
                    result = new AuthorizationResult(AuthorizationStatus.UnknownError, null);
                    break;
            }

            return result;
        }


        public void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: false);
        }
    }
}
