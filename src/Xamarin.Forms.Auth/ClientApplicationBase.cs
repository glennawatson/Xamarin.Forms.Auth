// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Abstract class containing common API methods and properties.
    /// </summary>
    public abstract partial class ClientApplicationBase : IClientApplicationBase
    {
        internal ClientApplicationBase(ApplicationConfiguration config)
        {
            ServiceBundle = Auth.ServiceBundle.Create(config);
        }

        /// <summary>
        /// Gets the details on the configuration of the ClientApplication for debugging purposes.
        /// </summary>
        public IAppConfig AppConfig => ServiceBundle.Config;

        /// <summary>
        /// Gets the URL of the authority.
        /// </summary>
        public Uri Authority => ServiceBundle.Config.Authority;

        internal IServiceBundle ServiceBundle { get; }

        internal ITokenCache TokenCache { get; set; }

        /// <inheritdoc />
        public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes)
        {
            return AcquireTokenSilentParameterBuilder.Create(
                ClientExecutorFactory.CreateClientApplicationBaseExecutor(this),
                scopes);
        }

        /// <summary>
        /// Removes all tokens in the cache.
        /// </summary>
        /// <returns>A task to monitor the progress.</returns>
        public Task RemoveAsync()
        {
            TokenCache.RemoveAccount(Authority, AppConfig.ClientId);

            return Task.FromResult(0);
        }

        /// <summary>
        /// [V3 API] Attempts to acquire an access token for the login hint.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Typically the username, in UPN format, e.g. johnd@contoso.com. </param>
        /// <returns>An <see cref="AcquireTokenSilentParameterBuilder"/> used to build the token request, adding optional
        /// parameters.</returns>
        /// <exception cref="AuthUiRequiredException">will be thrown in the case where an interaction is required with the end user of the application,
        /// for instance, if no refresh token was in the cache, or the user needs to consent, or re-sign-in (for instance if the password expired),
        /// or the user needs to perform two factor authentication.</exception>
        public AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, string loginHint)
        {
            if (string.IsNullOrWhiteSpace(loginHint))
            {
                throw new ArgumentNullException(nameof(loginHint));
            }

            return AcquireTokenSilentParameterBuilder.Create(
                ClientExecutorFactory.CreateClientApplicationBaseExecutor(this),
                scopes,
                loginHint);
        }

        internal virtual AuthenticationRequestParameters CreateRequestParameters(
            AcquireTokenCommonParameters commonParameters,
            RequestContext requestContext)
        {
            return new AuthenticationRequestParameters(
                ServiceBundle,
                commonParameters,
                requestContext);
        }

        private RequestContext CreateRequestContext()
        {
            return new RequestContext(AppConfig.ClientId, AuthLogger.Create(ServiceBundle.Config));
        }
    }
}
