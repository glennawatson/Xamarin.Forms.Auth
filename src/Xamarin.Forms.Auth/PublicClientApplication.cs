// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Class to be used to acquire tokens in desktop or mobile applications (Desktop / UWP / Xamarin.iOS / Xamarin.Android).
    /// public client applications are not trusted to safely keep application secrets, and therefore they only access Web APIs in the name of the user only
    /// (they only support public client flows).
    /// </summary>
    public sealed partial class PublicClientApplication : ClientApplicationBase, IPublicClientApplication
    {
        static PublicClientApplication()
        {
            ModuleInitializer.EnsureModuleInitialized();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicClientApplication"/> class.
        /// </summary>
        /// <param name="clientId">Client ID (also named Application ID) of the application as registered in the
        /// application registration portal (https://aka.ms/msal-net-register-app)/. REQUIRED.</param>
        /// <param name="authority">Authority of the security token service (STS) from which MSAL.NET will acquire the tokens.
        /// Usual authorities are:
        /// <list type="bullet">
        /// <item><description><c>https://login.microsoftonline.com/tenant/</c>, where <c>tenant</c> is the tenant ID of the Azure AD tenant
        /// or a domain associated with this Azure AD tenant, in order to sign-in user of a specific organization only</description></item>
        /// <item><description><c>https://login.microsoftonline.com/common/</c> to signing users with any work and school accounts or Microsoft personal account</description></item>
        /// <item><description><c>https://login.microsoftonline.com/organizations/</c> to signing users with any work and school accounts</description></item>
        /// <item><description><c>https://login.microsoftonline.com/consumers/</c> to signing users with only personal Microsoft account (live)</description></item>
        /// </list>
        /// Note that this setting needs to be consistent with what is declared in the application registration portal.
        /// </param>
        /// <param name="redirectUri">The URI where to redirect to.</param>
        public PublicClientApplication(string clientId, Uri authority, Uri redirectUri)
            : this(null, clientId, authority, redirectUri)
        {
        }

        internal PublicClientApplication(IServiceBundle serviceBundle, string clientId, Uri authority, Uri redirectUri)
            : base(
                clientId,
                authority,
                redirectUri,
                serviceBundle)
        {
        }

        /// <summary>
        /// Interactive request to acquire token for the specified scopes. The user is required to select an account.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <returns>Authentication result containing a token for the requested scopes and account.</returns>
        /// <remarks>The user will be signed-in interactively if needed,
        /// and will consent to scopes and do multi-factor authentication if such a policy was enabled in the Azure AD tenant.</remarks>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(IReadOnlyCollection<string> scopes)
        {
            GuardNetCore();
            GuardUIParentAndroid();

            return
                await
                    AcquireTokenForLoginHintCommonAsync(Authority, scopes, null, null, UIBehavior.SelectAccount, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token for the specified scopes. The user will need to sign-in but an account will be proposed
        /// based on the <paramref name="loginHint"/>.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Identifier of the user. Generally in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.</param>
        /// <returns>Authentication result containing a token for the requested scopes and account.</returns>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(IReadOnlyCollection<string> scopes, string loginHint)
        {
            GuardNetCore();
            GuardUIParentAndroid();

            var authority = Authority;
            return
                await
                    AcquireTokenForLoginHintCommonAsync(authority, scopes, null, loginHint, UIBehavior.SelectAccount, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token for a login with control of the UI behavior and possiblity of passing extra query parameters like additional claims.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Identifier of the user. Generally in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.</param>
        /// <param name="behavior">Designed interactive experience for the user.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority.
        /// This is expected to be a string of segments of the form <c>key=value</c> separated by an ampersand character.
        /// The parameter can be null.</param>
        /// <returns>Authentication result containing a token for the requested scopes and account.</returns>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(IReadOnlyCollection<string> scopes, string loginHint, UIBehavior behavior, string extraQueryParameters)
        {
            GuardNetCore();
            GuardUIParentAndroid();

            var authority = Authority;
            return
                await
                    AcquireTokenForLoginHintCommonAsync(authority, scopes, null, loginHint, behavior, extraQueryParameters, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token for a given login, with the possibility of controlling the user experience, passing extra query
        /// parameters, providing extra scopes that the user can pre-consent to, and overriding the authority pre-configured in the application.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Identifier of the user. Generally in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.</param>
        /// <param name="behavior">Designed interactive experience for the user.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority.
        /// This is expected to be a string of segments of the form <c>key=value</c> separated by an ampersand character.
        /// The parameter can be null.</param>
        /// <param name="extraScopesToConsent">Scopes that you can request the end user to consent upfront, in addition to the scopes for the protected Web API
        /// for which you want to acquire a security token.</param>
        /// <returns>Authentication result containing a token for the requested scopes and account.</returns>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(
            IReadOnlyCollection<string> scopes,
            string loginHint,
            UIBehavior behavior,
            string extraQueryParameters,
            IReadOnlyCollection<string> extraScopesToConsent)
        {
            GuardNetCore();
            GuardUIParentAndroid();

            var authority = Authority;
            return
                await
                    AcquireTokenForLoginHintCommonAsync(authority, scopes, extraScopesToConsent, loginHint, behavior, extraQueryParameters, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token for the specified scopes. The interactive window will be parented to the specified
        /// window. The user will be required to select an account.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="parent">Object containing a reference to the parent window/activity. REQUIRED for Xamarin.Android only.</param>
        /// <returns>Authentication result containing a token for the requested scopes and account.</returns>
        /// <remarks>The user will be signed-in interactively if needed,
        /// and will consent to scopes and do multi-factor authentication if such a policy was enabled in the Azure AD tenant.</remarks>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(IReadOnlyCollection<string> scopes, UIParent parent)
        {
            GuardNetCore();

            var authority = Authority;
            return
                await
                    AcquireTokenForLoginHintCommonAsync(authority, scopes, null, null, UIBehavior.SelectAccount, null, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token for the specified scopes. The interactive window will be parented to the specified
        /// window. The user will need to sign-in but an account will be proposed
        /// based on the <paramref name="loginHint"/>.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Identifier of the user. Generally in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.</param>
        /// <param name="parent">Object containing a reference to the parent window/activity. REQUIRED for Xamarin.Android only.</param>
        /// <returns>Authentication result containing a token for the requested scopes and login.</returns>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(IReadOnlyCollection<string> scopes, string loginHint, UIParent parent)
        {
            GuardNetCore();

            var authority = Authority;
            return
                await
                    AcquireTokenForLoginHintCommonAsync(authority, scopes, null, loginHint, UIBehavior.SelectAccount, null, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token for a login with control of the UI behavior and possiblity of passing extra query parameters like additional claims.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Identifier of the user. Generally in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.</param>
        /// <param name="behavior">Designed interactive experience for the user.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority.
        /// This is expected to be a string of segments of the form <c>key=value</c> separated by an ampersand character.
        /// The parameter can be null.</param>
        /// <param name="parent">Object containing a reference to the parent window/activity. REQUIRED for Xamarin.Android only.</param>
        /// <returns>Authentication result containing a token for the requested scopes and account.</returns>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(IReadOnlyCollection<string> scopes, string loginHint, UIBehavior behavior, string extraQueryParameters, UIParent parent)
        {
            GuardNetCore();

            var authority = Authority;
            return
                await
                    AcquireTokenForLoginHintCommonAsync(authority, scopes, null, loginHint, behavior, extraQueryParameters, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token for a given login, with the possibility of controlling the user experience, passing extra query
        /// parameters, providing extra scopes that the user can pre-consent to, and overriding the authority pre-configured in the application.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Identifier of the user. Generally in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.</param>
        /// <param name="behavior">Designed interactive experience for the user.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority.
        /// This is expected to be a string of segments of the form <c>key=value</c> separated by an ampersand character.
        /// The parameter can be null.</param>
        /// <param name="extraScopesToConsent">scopes that you can request the end user to consent upfront, in addition to the scopes for the protected Web API
        /// for which you want to acquire a security token.</param>
        /// <param name="authority">Specific authority for which the token is requested. Passing a different value than configured does not change the configured value.</param>
        /// <param name="parent">Object containing a reference to the parent window/activity. REQUIRED for Xamarin.Android only.</param>
        /// <returns>Authentication result containing a token for the requested scopes and account.</returns>
        public async Task<OAuth2TokenResponse> AcquireTokenAsync(IReadOnlyCollection<string> scopes, string loginHint, UIBehavior behavior, string extraQueryParameters, IReadOnlyCollection<string> extraScopesToConsent, string authority, UIParent parent)
        {
            GuardNetCore();

            return
                await
                    AcquireTokenForLoginHintCommonAsync(Authority, scopes, extraScopesToConsent, loginHint, behavior, extraQueryParameters, parent).ConfigureAwait(false);
        }

        internal IWebUI CreateWebAuthenticationDialog(UIParent parent, UIBehavior behavior, RequestContext requestContext)
        {
            // create instance of UIParent and assign useCorporateNetwork to UIParent
            if (parent == null)
            {
#pragma warning disable CS0618 // Throws a good exception on Android, but ctor cannot be removed for backwards compat reasons
                parent = new UIParent();
#pragma warning restore CS0618 // Type or member is obsolete
            }

#if WINDOWS_APP || DESKTOP
            //hidden webview can be used in both WinRT and desktop applications.
            parent.UseHiddenBrowser = behavior.Equals(UIBehavior.Never);
#if WINDOWS_APP
            parent.UseCorporateNetwork = UseCorporateNetwork;
#endif
#endif

            return ServiceBundle.PlatformProxy.GetWebUiFactory().CreateAuthenticationDialog(parent.CoreUIParent, requestContext);
        }

        private static void GuardNetCore()
        {
#if NET_CORE
            throw new PlatformNotSupportedException("On .NET Core, interactive authentication is not supported. " + 
                "Consider using Device Code Flow https://aka.ms/msal-net-device-code-flow or Integrated Windows Auth https://aka.ms/msal-net-iwa");
#endif
        }

        private static void GuardUIParentAndroid()
        {
#if ANDROID
            throw new PlatformNotSupportedException("To enable interactive authentication on Android, please call an overload of AcquireTokenAsync that " +
                "takes in an UIParent object, which you should initialize to an Activity. " +
                "See https://aka.ms/msal-interactive-android for details.");
#endif
        }

        private async Task<OAuth2TokenResponse> AcquireTokenForLoginHintCommonAsync(
            Uri authority,
            IReadOnlyCollection<string> scopes,
            IReadOnlyCollection<string> extraScopesToConsent,
            string loginHint,
            UIBehavior behavior,
            string extraQueryParameters,
            UIParent parent)
        {
            var requestParams = CreateRequestParameters(authority, scopes);
            requestParams.ExtraQueryParameters = extraQueryParameters;

            var handler = new InteractiveRequest(
                ServiceBundle,
                requestParams,
                extraScopesToConsent,
                loginHint,
                behavior,
                CreateWebAuthenticationDialog(
                    parent,
                    behavior,
                    requestParams.RequestContext));

            return await handler.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
