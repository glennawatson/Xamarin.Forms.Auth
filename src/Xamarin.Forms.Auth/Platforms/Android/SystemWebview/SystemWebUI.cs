// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Uri = System.Uri;

namespace Xamarin.Forms.Auth
{
    [global::Android.Runtime.Preserve(AllMembers = true)]
    internal class SystemWebUI : WebviewBase
    {
        private readonly CoreUIParent _parent;

        public SystemWebUI(CoreUIParent parent)
        {
            _parent = parent;
        }

        public RequestContext RequestContext { get; set; }

        public async override Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext)
        {
            returnedUriReady = new SemaphoreSlim(0);

            try
            {
                var agentIntent = new Intent(_parent.Activity, typeof(AuthenticationActivity));
                agentIntent.PutExtra(AndroidConstants.RequestUrlKey, authorizationUri.AbsoluteUri);
                agentIntent.PutExtra(AndroidConstants.CustomTabRedirect, redirectUri.OriginalString);
                AuthenticationActivity.RequestContext = RequestContext;
                _parent.Activity.RunOnUiThread(() => _parent.Activity.StartActivityForResult(agentIntent, 0));
            }
            catch (Exception ex)
            {
                requestContext.Logger.ErrorPii(ex);
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
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: true);
        }
    }
}
