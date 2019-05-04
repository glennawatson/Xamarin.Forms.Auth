// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <Summary>
    /// Abstract class containing common API methods and properties. Both <see cref="T:PublicClientApplication"/> and <see cref="T:ConfidentialClientApplication"/>
    /// extend this class. For details see https://aka.ms/msal-net-client-applications
    /// </Summary>
    public partial interface IClientApplicationBase
    {
        /// <summary>
        /// Identifier of the component (libraries/SDK) consuming MSAL.NET.
        /// This will allow for disambiguation between MSAL usage by the app vs MSAL usage by component libraries.
        /// </summary>
        string Component { get; set; }

        /// <Summary>
        /// Gets the URL of the authority, or the security token service (STS) from which MSAL.NET will acquire security tokens.
        /// The return value of this propety is either the value provided by the developer in the constructor of the application, or otherwise
        /// the value of the <see cref="ClientApplicationBase.Authority"/> static member (that is <c>https://login.microsoftonline.com/common/</c>)
        /// </Summary>
        string Authority { get; }

        /// <summary>
        /// Gets the Client ID (also known as Application ID) of the application as registered in the application registration portal (https://aka.ms/msal-net-register-app)
        /// and as passed in the constructor of the application.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// The redirect URI (also known as Reply URI or Reply URL), is the URI at which Azure AD will contact back the application with the tokens.
        /// This redirect URI needs to be registered in the app registration (https://aka.ms/msal-net-register-app)
        /// In MSAL.NET, <see cref="T:PublicClientApplication"/> define the following default RedirectUri values:
        /// <list type="bullet">
        /// <item><description><c>urn:ietf:wg:oauth:2.0:oob</c> for desktop (.NET Framework and .NET Core) applications</description></item>
        /// <item><description><c>msal{ClientId}</c> for Xamarin iOS and Xamarin Android (as this will be used by the system web browser by default on these
        /// platforms to call back the application)
        /// </description></item>
        /// </list>
        /// These default URIs could change in the future.
        /// In <see cref="Xamarin.Auth.Forms.ConfidentialClientApplication"/>, this can be the URL of the Web application / Web API.
        /// </summary>
        /// <remarks>This is especially important when you deploy an application that you have initially tested locally;
        /// you then need to add the reply URL of the deployed application in the application registration portal.
        /// </remarks>
        string RedirectUri { get; set; }

        /// <summary>
        /// Gets a boolean value telling the application if the authority needs to be verified against a list of known authorities. The default
        /// value is <c>true</c>. It should currently be set to <c>false</c> for Azure AD B2C authorities as those are customer specific
        /// (a list of known B2C authorities cannot be maintained by MSAL.NET)
        /// </summary>
        bool ValidateAuthority { get; }

        /// <summary>
        /// Returns all the available <see cref="IAccount">accounts</see> in the user token cache for the application.
        /// </summary>
        Task<IEnumerable<IAccount>> GetAccountsAsync();

        /// <summary>
        /// Sets or Gets a custom query parameters that may be sent to the STS for dogfood testing or debugging. This is a string of segments
        /// of the form <c>key=value</c> separated by an ampersand character.
        /// Unless requested otherwise, this parameter should not be set by application developers as it may have adverse effect on the application.
        /// </summary>
        string SliceParameters { get; set; }

        /// <summary>
        /// Attempts to acquire an access token for the <paramref name="account"/> from the user token cache.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <returns>An <see cref="AuthenticationResult"/> containing the requested token</returns>
        /// <exception cref="MsalUiRequiredException">can be thrown in the case where an interaction is required with the end user of the application,
        /// for instance so that the user consents, or re-signs-in (for instance if the password expirred), or performs two factor authentication</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes.
        /// This means that an access token with more scopes than requested could be returned as well. If the access token is expired or
        /// close to expiration (within 5 minute window), then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        /// </remarks>
        Task<AuthenticationResult> AcquireTokenSilentAsync(IEnumerable<string> scopes);

        /// <summary>
        /// Attempts to acquire and access token for the <paramref name="account"/> from the user token cache, with advanced parameters making a network call.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API</param>
        /// <param name="account">Account for which the token is requested. <see cref="IAccount"/></param>
        /// <param name="authority">Specific authority for which the token is requested. Passing a different value than configured in the application constructor
        /// narrows down the selection of tenants for which to get a tenant, but does not change the configured value</param>
        /// <param name="forceRefresh">If <c>true</c>, the will ignore the access token in the cache and attempt to acquire new access token
        /// using the refresh token for the account if this one is available. This can be useful in the case when the application developer wants to make
        /// sure that conditional access policies are applies immediately, rather than after the expiration of the access token</param>
        /// <returns>An <see cref="AuthenticationResult"/> containing the requested token</returns>
        /// <exception cref="MsalUiRequiredException">can be thrown in the case where an interaction is required with the end user of the application,
        /// for instance, if no refresh token was in the cache, or the user needs to consents, or re-sign-in (for instance if the password expirred),
        /// or performs two factor authentication</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes. This means that an access token with more scopes than
        /// requested could be returned as well. If the access token is expired or close to expiration (within 5 minute window),
        /// then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        /// See https://aka.ms/msal-net-acquiretokensilent for more details
        /// </remarks>
        Task<AuthenticationResult> AcquireTokenSilentAsync(
        IEnumerable<string> scopes,
            IAccount account,
            string authority,
            bool forceRefresh);

        /// <summary>
        /// Removes all tokens in the cache for the specified account.
        /// </summary>
        /// <param name="account">instance of the account that needs to be removed</param>
        Task RemoveAsync(IAccount account);
   }
}
