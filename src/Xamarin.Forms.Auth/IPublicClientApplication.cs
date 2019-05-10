// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Interface to be used with desktop or mobile applications (Desktop / UWP / Xamarin.iOS / Xamarin.Android).
    /// public client applications are not trusted to safely keep application secrets, and therefore they only access Web APIs in the name of the user only.
    /// For details see https://aka.ms/auth-net-client-applications.
    /// </summary>
    public partial interface IPublicClientApplication : IClientApplicationBase
    {
        /// <summary>
        /// Gets a value indicating whether the application can use the system web browser, therefore getting single-sign-on with web applications.
        /// By default, Auth will try to use a system browser on the mobile platforms, if it is available.
        /// </summary>
        bool IsSystemWebViewAvailable { get; }

        /// <summary>
        /// Interactive request to acquire a token for the specified scopes. The interactive window will be parented to the specified
        /// window. The user will be required to select an account.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <returns>A builder enabling you to add optional parameters before executing the token request.</returns>
        /// <remarks>The user will be signed-in interactively if needed,
        /// and will consent to scopes and do multi-factor authentication if such a policy was enabled in the Azure AD tenant.
        /// You can also pass optional parameters by calling:
        /// <see cref="AcquireTokenInteractiveParameterBuilder.WithPrompt(Prompt)"/> to specify the user experience
        /// when signing-in, <see cref="AcquireTokenInteractiveParameterBuilder.WithUseEmbeddedWebView(bool)"/> to specify
        /// if you want to use the embedded web browser or the system default browser,
        /// <see cref="AcquireTokenInteractiveParameterBuilder.WithLoginHint(string)"/>
        /// to prevent the select account dialog from appearing in the case you want to sign-in a specific account,
        /// <see cref="AcquireTokenInteractiveParameterBuilder.WithExtraScopesToConsent(IEnumerable{string})"/> if you want to let the
        /// user pre-consent to additional scopes (which won't be returned in the access token),
        /// <see cref="AbstractAcquireTokenParameterBuilder{T}.WithExtraQueryParameters(Dictionary{string, string})"/> to pass
        /// additional query parameters to the STS, and one of the overrides of <see cref="AbstractAcquireTokenParameterBuilder{T}.WithAuthority(Uri)"/>
        /// in order to override the default authority set at the application construction. Note that the overriding authority needs to be part
        /// of the known authorities added to the application construction.
        /// </remarks>
        AcquireTokenInteractiveParameterBuilder AcquireTokenInteractive(IEnumerable<string> scopes);
    }
}
