// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;

namespace Xamarin.Forms.Auth
{
    internal class EmbeddedWebUI : WebviewBase
    {
        private readonly CoreUIParent _coreUIParent;

        public EmbeddedWebUI(CoreUIParent coreUIParent)
        {
            _coreUIParent = coreUIParent;
        }

        public RequestContext RequestContext { get; internal set; }

        public async override Task<AuthorizationResult> AcquireAuthorizationAsync(
            Uri authorizationUri,
            Uri redirectUri,
            RequestContext requestContext,
            CancellationToken cancellationToken)
        {
            ReturnedUriReady = new SemaphoreSlim(0);

            try
            {
                var agentIntent = new Intent(_coreUIParent.CallerActivity, typeof(AuthenticationAgentActivity));
                agentIntent.PutExtra("Url", authorizationUri.AbsoluteUri)
                    .PutExtra("Callback", redirectUri.AbsoluteUri);
                _coreUIParent.CallerActivity.StartActivityForResult(agentIntent, 0);
            }
            catch (Exception ex)
            {
                throw new AuthClientException(
                    AuthError.AuthenticationUiFailedError,
                    "AuthenticationActivity failed to start",
                    ex);
            }

            await ReturnedUriReady.WaitAsync(cancellationToken).ConfigureAwait(false);
            return AuthorizationResult;
        }

        public override void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: false);
        }
    }
}
