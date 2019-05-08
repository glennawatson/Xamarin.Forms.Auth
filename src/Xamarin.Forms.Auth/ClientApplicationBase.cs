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
        /// <param name="clientId">Client ID registered in your application portal.</param>
        /// <param name="authority">URL of the security token service (STS) from which weT will acquire the tokens.</param>p
        /// <param name="redirectUri">also named <i>Reply URI</i>, the redirect URI is the URI where the STS will call back the application with the security token.</param>
        /// <param name="serviceBundle">The service bundle.</param>
        internal ClientApplicationBase(string clientId, Uri authority, Uri redirectUri, IServiceBundle serviceBundle)
        {
            ServiceBundle = serviceBundle ?? Auth.ServiceBundle.CreateDefault();

            ClientId = clientId;
            Authority = authority;
            RedirectUri = redirectUri;
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
        /// Gets or sets a custom query parameters that may be sent to the STS for dogfood testing or debugging. This is a string of segments
        /// of the form <c>key=value</c> separated by an ampersand character.
        /// Unless requested otherwise by Microsoft support, this parameter should not be set by application developers as it may have adverse effect on the application.
        /// This property is also concatenated to the <c>extraQueryParameter</c> parameters of token acquisition operations.
        /// </summary>
        public string SliceParameters { get; set; }

        internal IServiceBundle ServiceBundle { get; }

        internal ITokenCache TokenCache => ServiceBundle?.PlatformProxy?.TokenCache;

        /// <summary>
        /// Attempts to acquire an access token from the token cache.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <returns>An <see cref="OAuth2TokenResponse"/> containing the requested token.</returns>
        /// <exception cref="AuthUiRequiredException">can be thrown in the case where an interaction is required with the end user of the application,
        /// for instance so that the user consents, or re-signs-in (for instance if the password expired), or performs two factor authentication.</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes.
        /// This means that an access token with more scopes than requested could be returned as well. If the access token is expired or
        /// close to expiration (within a 5 minute window), then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        /// </remarks>
        public Task<OAuth2TokenResponse> AcquireTokenSilentAsync(IReadOnlyCollection<string> scopes)
        {
            return AcquireTokenSilentCommonAsync(Authority, scopes, false);
        }

        /// <summary>
        /// Attempts to acquire an access token with advanced parameters controlling network call.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="forceRefresh">If <c>true</c>, ignore any access token in the cache and attempt to acquire new access token
        /// using the refresh token for the account if this one is available. This can be useful in the case when the application developer wants to make
        /// sure that conditional access policies are applied immediately, rather than after the expiration of the access token.</param>
        /// <returns>An <see cref="OAuth2TokenResponse"/> containing the requested access token.</returns>
        /// <exception cref="AuthUiRequiredException">can be thrown in the case where an interaction is required with the end user of the application,
        /// for instance, if no refresh token was in the cache, or the user needs to consent, or re-sign-in (for instance if the password expired),
        /// or performs two factor authentication.</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes. This means that an access token with more scopes than
        /// requested could be returned as well. If the access token is expired or close to expiration (within a 5 minute window),
        /// then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        /// </remarks>
        public Task<OAuth2TokenResponse> AcquireTokenSilentAsync(IReadOnlyCollection<string> scopes, bool forceRefresh)
        {
            return AcquireTokenSilentCommonAsync(Authority, scopes, forceRefresh);
        }

        /// <summary>
        /// Removes all tokens in the cache for the specified account.
        /// </summary>
        /// <returns>A task to monitor the progress of the action.</returns>
        public Task RemoveAsync()
        {
            TokenCache?.RemoveAccount(Authority, ClientId);

            return Task.FromResult(0);
        }

        internal Task<OAuth2TokenResponse> AcquireTokenSilentCommonAsync(
            Uri authority,
            IReadOnlyCollection<string> scopes,
            bool forceRefresh)
        {
            var handler = new SilentRequest(
                ServiceBundle,
                CreateRequestParameters(authority, scopes),
                forceRefresh);

            return handler.RunAsync(CancellationToken.None);
        }

        internal Task<OAuth2TokenResponse> AcquireByRefreshTokenCommonAsync(IReadOnlyCollection<string> scopes, string userProvidedRefreshToken)
        {
            var context = CreateRequestContext();
            SortedSet<string> scopesToPass;

            if (scopes == null || scopes.Count == 0)
            {
                scopesToPass = new SortedSet<string> { ClientId + "/.default" };
                context.Logger.Info(LogMessages.NoScopesProvidedForRefreshTokenRequest);
            }
            else
            {
                scopesToPass = ScopeHelper.CreateSortedSetFromEnumerable(scopes);
                context.Logger.Info(string.Format(CultureInfo.InvariantCulture, LogMessages.UsingXScopesForRefreshTokenRequest, scopes.Count));
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

            return handler.RunAsync(CancellationToken.None);
        }

        internal virtual AuthenticationRequestParameters CreateRequestParameters(
            Uri authority,
            IReadOnlyCollection<string> scopes)
        {
            return new AuthenticationRequestParameters
            {
                SliceParameters = SliceParameters,
                Authority = authority,
                ClientId = ClientId,
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
