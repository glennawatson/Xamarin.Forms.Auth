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
        public RequestContext RequestContext { get; internal set; }

        public EmbeddedWebUI(CoreUIParent coreUIParent)
        {
            _coreUIParent = coreUIParent;
        }

        public async override Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext)
        {
            returnedUriReady = new SemaphoreSlim(0);

            try
            {
                var agentIntent = new Intent(_coreUIParent.CallerActivity, typeof(AuthenticationAgentActivity));
                agentIntent.PutExtra("Url", authorizationUri.AbsoluteUri);
                agentIntent.PutExtra("Callback", redirectUri.AbsoluteUri);
                _coreUIParent.CallerActivity.StartActivityForResult(agentIntent, 0);
            }
            catch (Exception ex)
            {
                throw MsalExceptionFactory.GetClientException(
                    CoreErrorCodes.AuthenticationUiFailedError,
                    "AuthenticationActivity failed to start",
                    ex);
            }

            await returnedUriReady.WaitAsync().ConfigureAwait(false);
            return authorizationResult;
        }

        public override void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: false);
        }
    }
}
