// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.CustomTabs;

using Uri = Android.Net.Uri;

namespace Xamarin.Forms.Auth
{
    [Activity(Name = "xamarin.forms.auth.AuthenticationActivity")]
    [global::Android.Runtime.Preserve(AllMembers = true)]
    internal class AuthenticationActivity : Activity
    {
        private readonly string _customTabsServiceAction =
            "android.support.customtabs.action.CustomTabsService";

        private string _requestUrl;
        private int _requestId;
        private bool _restarted;

        internal static RequestContext RequestContext { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // If activity is killed by the os, savedInstance will be the saved bundle.
            if (bundle != null)
            {
                _restarted = true;
                return;
            }

            if (Intent == null)
            {
                SendError(
                    AuthError.UnresolvableIntentError,
                    "Received null data intent from caller");
                return;
            }

            _requestUrl = Intent.GetStringExtra(AndroidConstants.RequestUrlKey);
            _requestId = Intent.GetIntExtra(AndroidConstants.RequestId, 0);
            if (string.IsNullOrEmpty(_requestUrl))
            {
                SendError(AuthError.InvalidRequest, "Request url is not set on the intent");
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            string url = intent.GetStringExtra(AndroidConstants.CustomTabRedirect);

            Intent resultIntent = new Intent();
            resultIntent.PutExtra(AndroidConstants.AuthorizationFinalUrl, url);
            ReturnToCaller(AndroidConstants.AuthCodeReceived, resultIntent);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_restarted)
            {
                CancelRequest();
                return;
            }

            _restarted = true;

            string chromePackageWithCustomTabSupport = GetChromePackageWithCustomTabSupport(ApplicationContext);

            if (string.IsNullOrEmpty(chromePackageWithCustomTabSupport))
            {
                Intent browserIntent = new Intent(Intent.ActionView, Uri.Parse(_requestUrl));
                browserIntent.AddCategory(Intent.CategoryBrowsable);

                RequestContext.Logger.Warning(
                    "Browser with custom tabs package not available. " +
                    "Launching with alternate browser. See https://aka.ms/msal-net-system-browsers for details.");

                try
                {
                    StartActivity(browserIntent);
                }
                catch (ActivityNotFoundException ex)
                {
                    throw new AuthClientException(AuthError.AndroidActivityNotFound, AuthErrorMessage.AndroidActivityNotFound, ex);
                }
            }
            else
            {
                RequestContext.Logger.Info(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    "Browser with custom tabs package available. Using {0}. ",
                    chromePackageWithCustomTabSupport));

                CustomTabsIntent customTabsIntent = new CustomTabsIntent.Builder().Build();
                customTabsIntent.Intent.SetPackage(chromePackageWithCustomTabSupport);
                customTabsIntent.LaunchUrl(this, Uri.Parse(_requestUrl));
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutString(AndroidConstants.RequestUrlKey, _requestUrl);
        }

        private void CancelRequest()
        {
            ReturnToCaller(AndroidConstants.Cancel, new Intent());
        }

        /**
         * Return the error back to caller.
         * @param resultCode The result code to return back.
         * @param data {@link Intent} contains the detailed result.
         */
        private void ReturnToCaller(int resultCode, Intent data)
        {
            data.PutExtra(AndroidConstants.RequestId, _requestId);
            SetResult((Result)resultCode, data);
            Finish();
        }

        /**
         * Send error back to caller with the error description.
         * @param errorCode The error code to send back.
         * @param errorDescription The error description to send back.
         */
        private void SendError(string errorCode, string errorDescription)
        {
            Intent errorIntent = new Intent();
            errorIntent.PutExtra(OAuth2ResponseBaseClaim.Error, errorCode)
                .PutExtra(OAuth2ResponseBaseClaim.ErrorDescription, errorDescription);
            ReturnToCaller(AndroidConstants.AuthCodeError, errorIntent);
        }

        private string GetChromePackageWithCustomTabSupport(Context context)
        {
            if (context.PackageManager == null)
            {
                return null;
            }

            Intent customTabServiceIntent = new Intent(_customTabsServiceAction);

            IEnumerable<ResolveInfo> resolveInfoListWithCustomTabs = context.PackageManager.QueryIntentServices(
                    customTabServiceIntent, PackageInfoFlags.MatchAll);

            // queryIntentServices could return null or an empty list if no matching service existed.
            if (resolveInfoListWithCustomTabs == null || !resolveInfoListWithCustomTabs.Any())
            {
                return null;
            }

            foreach (ResolveInfo resolveInfo in resolveInfoListWithCustomTabs)
            {
                ServiceInfo serviceInfo = resolveInfo.ServiceInfo;
                if (serviceInfo != null)
                {
                    return serviceInfo.PackageName;
                }
            }

            return null;
        }
    }
}
