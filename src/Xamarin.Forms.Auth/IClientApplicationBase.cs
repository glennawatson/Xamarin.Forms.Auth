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
    public partial interface IClientApplicationBase
    {
        /// <summary>
        /// Gets the details on the configuration of the ClientApplication for debugging purposes.
        /// </summary>
        IAppConfig AppConfig { get; }

        /// <summary>
        /// Gets the URL of the authority.
        /// </summary>
        Uri Authority { get; }

        /// <summary>
        /// Attempts to acquire an access token for the <paramref name="loginHint"/> from the user token cache,
        /// with advanced parameters controlling the network call. See https://aka.ms/msal-net-acquiretokensilent for more details.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="loginHint">Typically the username, in UPN format, e.g. johnd@contoso.com. </param>
        /// <returns>An <see cref="AcquireTokenSilentParameterBuilder"/> used to build the token request, adding optional
        /// parameters.</returns>
        /// <exception cref="AuthUiRequiredException">will be thrown in the case where an interaction is required with the end user of the application,
        /// for instance, if no refresh token was in the cache,a or the user needs to consent, or re-sign-in (for instance if the password expired),
        /// or the user needs to perform two factor authentication.</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes. This means that an access token with more scopes than
        /// requested could be returned as well. If the access token is expired or close to expiration (within a 5 minute window),
        /// then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        /// See also the additional parameters that you can set chain:
        /// <see cref="AbstractAcquireTokenParameterBuilder{T}.WithAuthority(Uri)"/> or one of its
        /// overrides to request a token for a different authority than the one set at the application construction
        /// <see cref="AcquireTokenSilentParameterBuilder.WithForceRefresh(bool)"/> to bypass the user token cache and
        /// force refreshing the token, as well as
        /// <see cref="AbstractAcquireTokenParameterBuilder{T}.WithExtraQueryParameters(Dictionary{string, string})"/> to
        /// specify extra query parameters.
        /// </remarks>
        AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, string loginHint);

        /// <summary>
        /// Attempts to acquire an access token from the cache.
        /// </summary>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <returns>An <see cref="AcquireTokenSilentParameterBuilder"/> used to build the token request, adding optional
        /// parameters.</returns>
        /// <exception cref="AuthUiRequiredException">will be thrown in the case where an interaction is required with the end user of the application,
        /// for instance, if no refresh token was in the cache,a or the user needs to consent, or re-sign-in (for instance if the password expired),
        /// or the user needs to perform two factor authentication.</exception>
        /// <remarks>
        /// The access token is considered a match if it contains <b>at least</b> all the requested scopes. This means that an access token with more scopes than
        /// requested could be returned as well. If the access token is expired or close to expiration (within a 5 minute window),
        /// then the cached refresh token (if available) is used to acquire a new access token by making a silent network call.
        /// See also the additional parameters that you can set chain:
        /// <see cref="AbstractAcquireTokenParameterBuilder{T}.WithAuthority(Uri)"/> or one of its
        /// overrides to request a token for a different authority than the one set at the application construction
        /// <see cref="AcquireTokenSilentParameterBuilder.WithForceRefresh(bool)"/> to bypass the user token cache and
        /// force refreshing the token, as well as
        /// <see cref="AbstractAcquireTokenParameterBuilder{T}.WithExtraQueryParameters(Dictionary{string, string})"/> to
        /// specify extra query parameters.
        /// </remarks>
        AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes);

        /// <summary>
        /// Removes all tokens in the cache.
        /// </summary>
        /// <returns>A task to monitor the progress.</returns>
        Task RemoveAsync();
   }
}
