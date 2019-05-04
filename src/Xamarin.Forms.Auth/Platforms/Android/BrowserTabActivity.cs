// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// BrowserTabActivity to get the redirect with code from authorize endpoint. Intent filter has to be declared in the
    /// android manifest for this activity. When chrome custom tab is launched, and we're redirected back with the redirect
    /// uri (redirect_uri has to be unique across apps), the os will fire an intent with the redirect,
    /// and the BrowserTabActivity will be launched.
    /// </summary>
    public class BrowserTabActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Intent intent = new Intent(this, typeof(AuthenticationActivity));
            intent.PutExtra(AndroidConstants.CustomTabRedirect, Intent.DataString)
                .SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);
        }
    }
}
