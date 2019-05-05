// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Abstract class containing common API methods and properties.
    /// </summary>
    public abstract partial class ClientApplicationBase
    {
        static ClientApplicationBase()
        {
            ModuleInitializer.EnsureModuleInitialized();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientApplicationBase"/> class.
        ///  </summary>
        /// <param name="clientId">Client ID (also known as <i>Application ID</i>) of the application as registered in the
        ///  application registration portal (https://aka.ms/msal-net-register-app).</param>
        /// <param name="authority">URL of the security token service (STS) from which MSAL.NET will acquire the tokens.
        ///  Usual authorities endpoints for the Azure public Cloud are:
        ///  <list type="bullet">
        ///  <item><description><c>https://login.microsoftonline.com/tenant/</c> where <c>tenant</c> is the tenant ID of the Azure AD tenant
        ///  or a domain associated with this Azure AD tenant, in order to sign-in users of a specific organization only</description></item>
        ///  <item><description><c>https://login.microsoftonline.com/common/</c> to sign-in users with any work and school accounts or Microsoft personal account</description></item>
        ///  <item><description><c>https://login.microsoftonline.com/organizations/</c> to sign-in users with any work and school accounts</description></item>
        ///  <item><description><c>https://login.microsoftonline.com/consumers/</c> to sign-in users with only personal Microsoft accounts (live)</description></item>
        ///  </list>
        ///  Note that this setting needs to be consistent with what is declared in the application registration portal.
        ///  </param>
        /// <param name="redirectUri">also named <i>Reply URI</i>, the redirect URI is the URI where the STS will call back the application with the security token.</param>
        /// <param name="serviceBundle">The service bundle.</param>
        internal ClientApplicationBase(string clientId, Uri authority, Uri redirectUri, IServiceBundle serviceBundle)
        {
            ServiceBundle = serviceBundle ?? Auth.ServiceBundle.CreateDefault();

            ClientId = clientId;
            Authority = authority;
            RedirectUri = redirectUri;

            RequestContext requestContext = new RequestContext(ClientId, new OAuth2Logger(Guid.Empty, null));
        }

        /// <summary>
        /// Gets or sets the identifier of the component (libraries/SDK) consuming this library.
        /// This will allow for disambiguation between this library usage by the app vs library usage by component libraries.
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// Gets the URL of the authority, or security token service (STS) from which library will acquire security tokens.
        /// </summary>
        public Uri Authority { get; }

        /// <summary>
        /// Gets the Client ID (also known as <i>Application ID</i>) of the application as registered in the application registration portal (https://aka.ms/msal-net-register-app)
        /// and as passed in the constructor of the application.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Gets or sets The redirect URI (also known as Reply URI or Reply URL), is the URI at which OAuth2 will contact back the application with the tokens.
        /// </summary>
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Sets or Gets a custom query parameters that may be sent to the STS for dogfood testing or debugging. This is a string of segments
        /// of the form <c>key=value</c> separated by an ampersand character.
        /// Unless requested otherwise by Microsoft support, this parameter should not be set by application developers as it may have adverse effect on the application.
        /// This property is also concatenated to the <c>extraQueryParameter</c> parameters of token acquisition operations.
        /// </summary>
        public string SliceParameters { get; set; }

        internal IServiceBundle ServiceBundle { get; }

        /// <summary>
        /// Returns all the available <see cref="IAccount">accounts</see> in the user token cache for the application.
        /// </summary>
        /// <returns>A list of valid accounts.</returns>
        public Task<IEnumerable<IAccount>> GetAccountsAsync()
        {
            RequestContext requestContext = new RequestContext(ClientId, new OAuth2Logger(Guid.Empty, null));
            IEnumerable<IAccount> accounts = Enumerable.Empty<IAccount>();
            if (UserTokenCache == null)
            {
                requestContext.Logger.Info("Token cache is null or empty. Returning empty list of accounts.");
            }
            else
            {
                accounts = UserTokenCache.GetAccounts(Authority, requestContext);
            }

            return Task.FromResult(accounts);
        }

        /// <summary>
        /// Get the <see cref="IAccount"/> by its identifier among the accounts available in the token cache.
        /// </summary>
        /// <param name="accountId">Account identifier. The identifier is typically
        /// value of the <see cref="AccountId.Identifier"/> property of <see cref="AccountId"/>.
        /// You typically get the account id from an <see cref="IAccount"/> by using the <see cref="IAccount.HomeAccountId"/> property>
        /// </param>
        public async Task<IAccount> GetAccountAsync(string accountId)
        {
            var accounts = await GetAccountsAsync().ConfigureAwait(false);
            return accounts.FirstOrDefault(account => account.HomeAccountId.Identifier.Equals(accountId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Attempts to acquire an access token for the <paramref name="account"/> from the user token cache.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API</param>
        /// <param name="account">Account for which the token is requested. <see cref="IAccount"/></param>
        /// <returns>An <see cref="AuthenticationResult"/> containing the requested token</returns>
        /// <exception cref="MsalUiRequiredException">can be thrown in the case where an interaction is required with the end user of the application,
        /// for instance so that the user consents, or re-signs-in (for instance if the password expired), or performs two factor authentication</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes.
        /// This means that an access token with more scopes than requested could be returned as well. If the access token is expired or
        /// close to expiration (within a 5 minute window), then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        ///
        /// See https://aka.ms/msal-net-acquiretokensilent for more details
        /// </remarks>
        public async Task<AuthenticationResult> AcquireTokenSilentAsync(IEnumerable<string> scopes, IAccount account)
        {
            return
                await
                    AcquireTokenSilentCommonAsync(Authority, scopes, account, false)
                        .ConfigureAwait(false);
        }

        /// <summary>
        /// Attempts to acquire an access token for the <paramref name="account"/> from the user token cache, with advanced parameters controlling network call.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API</param>
        /// <param name="account">Account for which the token is requested. <see cref="IAccount"/></param>
        /// <param name="authority">Specific authority for which the token is requested. Passing a different value than configured in the application constructor
        /// narrows down the selection to a specific tenant. This does not change the configured value in the application. This is specific
        /// to applications managing several accounts (like a mail client with several mailboxes)</param>
        /// <param name="forceRefresh">If <c>true</c>, ignore any access token in the cache and attempt to acquire new access token
        /// using the refresh token for the account if this one is available. This can be useful in the case when the application developer wants to make
        /// sure that conditional access policies are applied immediately, rather than after the expiration of the access token</param>
        /// <returns>An <see cref="AuthenticationResult"/> containing the requested access token</returns>
        /// <exception cref="MsalUiRequiredException">can be thrown in the case where an interaction is required with the end user of the application,
        /// for instance, if no refresh token was in the cache, or the user needs to consent, or re-sign-in (for instance if the password expired),
        /// or performs two factor authentication</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes. This means that an access token with more scopes than
        /// requested could be returned as well. If the access token is expired or close to expiration (within a 5 minute window),
        /// then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        ///
        /// See https://aka.ms/msal-net-acquiretokensilent for more details
        /// </remarks>
        public async Task<AuthenticationResult> AcquireTokenSilentAsync(IEnumerable<string> scopes, IAccount account, Uri authority, bool forceRefresh)
        {
            return
                await
                    AcquireTokenSilentCommonAsync(authority, scopes, account, forceRefresh).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes all tokens in the cache for the specified account.
        /// </summary>
        /// <param name="account">Instance of the account that needs to be removed.</param>
        /// <returns>A task to monitor the progress of the action.</returns>
        public Task RemoveAsync(IAccount account)
        {
            RequestContext requestContext = CreateRequestContext();
            if (account != null)
            {
                UserTokenCache?.RemoveAccount(account, requestContext);
            }

            return Task.FromResult(0);
        }

        internal async Task<AuthenticationResult> AcquireTokenSilentCommonAsync(
            Uri authority,
            IEnumerable<string> scopes,
            IAccount account,
            bool forceRefresh)
        {
            if (account == null)
            {
                throw new AuthUiRequiredException(AuthUiRequiredException.UserNullError, "The account is required to be able to retrieve a token.");
            }

            var handler = new SilentRequest(
                ServiceBundle,
                CreateRequestParameters(authority, scopes, account),
                forceRefresh);

            return await handler.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        internal async Task<AuthenticationResult> AcquireByRefreshTokenCommonAsync(IEnumerable<string> scopes, string userProvidedRefreshToken)
        {
            var context = CreateRequestContext();
            SortedSet<string> scopesToPass;

            if (scopes == null || !scopes.Any())
            {
                scopesToPass = new SortedSet<string>();
                scopesToPass.Add(ClientId + "/.default");
                context.Logger.Info(LogMessages.NoScopesProvidedForRefreshTokenRequest);
            }
            else
            {
                scopesToPass = ScopeHelper.CreateSortedSetFromEnumerable(scopes);
                context.Logger.Info(string.Format(CultureInfo.InvariantCulture, LogMessages.UsingXScopesForRefreshTokenRequest, scopes.Count()));
            }

            var reqParams = new AuthenticationRequestParameters
            {
                SliceParameters = SliceParameters,
                Authority = Authority,
                ClientId = ClientId,
                Scope = scopesToPass,
                RedirectUri = RedirectUri,
                RequestContext = context,
                IsRefreshTokenRequest = true
            };

            var handler = new ByRefreshTokenRequest(
                ServiceBundle,
                reqParams,
                userProvidedRefreshToken);

            return await handler.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        internal virtual AuthenticationRequestParameters CreateRequestParameters(
            Uri authority,
            IEnumerable<string> scopes,
            IAccount account)
        {
            return new AuthenticationRequestParameters
            {
                SliceParameters = SliceParameters,
                Authority = authority,
                ClientId = ClientId,
                Account = account,
                Scope = ScopeHelper.CreateSortedSetFromEnumerable(scopes),
                RedirectUri = RedirectUri,
                RequestContext = CreateRequestContext(),
            };
        }

        private RequestContext CreateRequestContext()
        {
            return new RequestContext(ClientId, new OAuth2Logger(Guid.NewGuid(), Component));
        }
    }
}
