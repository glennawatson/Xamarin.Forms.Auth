// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class PublicClientExecutor : AbstractExecutor, IPublicClientApplicationExecutor
    {
        private readonly PublicClientApplication _publicClientApplication;

        public PublicClientExecutor(IServiceBundle serviceBundle, PublicClientApplication publicClientApplication)
            : base(serviceBundle, publicClientApplication)
        {
            _publicClientApplication = publicClientApplication;
        }

        public async Task<AuthenticationResult> ExecuteAsync(
            AcquireTokenCommonParameters commonParameters,
            AcquireTokenInteractiveParameters interactiveParameters,
            CancellationToken cancellationToken)
        {
            var requestContext = CreateRequestContextAndLogVersionInfo();

            var requestParams = _publicClientApplication.CreateRequestParameters(
                commonParameters,
                requestContext);

            requestParams.LoginHint = interactiveParameters.LoginHint;

            var handler = new InteractiveRequest(
                ServiceBundle,
                requestParams,
                interactiveParameters,
                CreateWebAuthenticationDialog(interactiveParameters, requestParams.RequestContext));

            return await handler.RunAsync(cancellationToken).ConfigureAwait(false);
        }

        private IWebUI CreateWebAuthenticationDialog(
            AcquireTokenInteractiveParameters interactiveParameters,
            RequestContext requestContext)
        {
            if (interactiveParameters.CustomWebUi != null)
            {
                return new CustomWebUiHandler(interactiveParameters.CustomWebUi);
            }

            var coreUiParent = interactiveParameters.UiParent;

#if ANDROID || iOS
            coreUiParent.UseEmbeddedWebview = interactiveParameters.UseEmbeddedWebView;
#endif

#if WINDOWS_APP || DESKTOP
            // hidden web view can be used in both WinRT and desktop applications.
            coreUiParent.UseHiddenBrowser = interactiveParameters.Prompt.Equals(Prompt.Never);
#if WINDOWS_APP
            coreUiParent.UseCorporateNetwork = _publicClientApplication.AppConfig.UseCorporateNetwork;
#endif
#endif
            return ServiceBundle.PlatformProxy.GetWebUiFactory().CreateAuthenticationDialog(coreUiParent, requestContext);
        }
    }
}
