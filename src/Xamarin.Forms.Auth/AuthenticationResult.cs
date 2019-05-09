// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains the results of one token acquisition operation in <see cref="PublicClientApplication"/>.
    /// </summary>
    public partial class AuthenticationResult
    {
        private const string Oauth2AuthorizationHeader = "Bearer ";
        private const int DefaultExpirationBufferInMinutes = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
        /// Constructor meant to help application developers test their apps. Allows mocking of authentication flows.
        /// App developers should <b>never</b> new-up <see cref="AuthenticationResult"/> in product code.
        /// </summary>
        /// <param name="accessToken">Access Token that can be used as a bearer token to access protected web APIs.</param>
        /// <param name="isExtendedLifeTimeToken">See <see cref="IsExtendedLifeTimeToken"/>.</param>
        /// <param name="uniqueId">Unique Id of the account. It can be null. When the <see cref="IdToken"/> is not <c>null</c>, this is its ID, that
        /// is its ObjectId claim, or if that claim is <c>null</c>, the Subject claim.</param>
        /// <param name="expiresOn">Expiracy date-time for the access token.</param>
        /// <param name="extendedExpiresOn">See <see cref="ExtendedExpiresOn"/>.</param>
        /// <param name="idToken">ID token.</param>
        /// <param name="scopes">granted scope values as returned by the service.</param>
        public AuthenticationResult(
            string accessToken,
            bool isExtendedLifeTimeToken,
            string uniqueId,
            DateTimeOffset expiresOn,
            DateTimeOffset extendedExpiresOn,
            string idToken,
            IEnumerable<string> scopes)
        {
            AccessToken = accessToken;
            IsExtendedLifeTimeToken = isExtendedLifeTimeToken;
            UniqueId = uniqueId;
            ExpiresOn = expiresOn;
            ExtendedExpiresOn = extendedExpiresOn;
            IdToken = idToken;
            Scopes = scopes;
        }

        internal AuthenticationResult(OAuth2TokenResponse tokenResponse, bool isExtendedLifetimeEnabled)
        {
            IdToken idToken = Auth.IdToken.Parse(tokenResponse.IdToken);

            bool isExtendedLifetimeToken = isExtendedLifetimeEnabled && tokenResponse.AccessTokenExtendedExpiresOn >
                                      DateTime.UtcNow + TimeSpan.FromMinutes(DefaultExpirationBufferInMinutes);

            var scopes = ScopeHelper.ConvertStringToLowercaseSortedSet(tokenResponse.Scope);
            AccessToken = tokenResponse.AccessToken;
            IsExtendedLifeTimeToken = isExtendedLifetimeToken;
            UniqueId = idToken.GetUniqueId();
            ExpiresOn = tokenResponse.AccessTokenExpiresOn;
            ExtendedExpiresOn = tokenResponse.AccessTokenExtendedExpiresOn;
            IdToken = tokenResponse.IdToken;
            Scopes = scopes;
        }

        internal AuthenticationResult()
        {
        }

        /// <summary>
        /// Gets access Token that can be used as a bearer token to access protected web APIs.
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Gets a value indicating whether in case when Azure AD has an outage, to be more resilient, it can return tokens with
        /// an expiration time, and also with an extended expiration time.
        /// The tokens are then automatically refreshed by MSAL when the time is more than the
        /// expiration time, except when ExtendedLifeTimeEnabled is true and the time is less
        /// than the extended expiration time. This goes in pair with Web APIs middleware which,
        /// when this extended life time is enabled, can accept slightly expired tokens.
        /// Client applications accept extended life time tokens only if
        /// the ExtendedLifeTimeEnabled Boolean is set to true on ClientApplicationBase.
        /// </summary>
        public bool IsExtendedLifeTimeToken { get; }

        /// <summary>
        /// Gets the Unique Id of the account. It can be null. When the <see cref="IdToken"/> is not <c>null</c>, this is its ID, that
        /// is its ObjectId claim, or if that claim is <c>null</c>, the Subject claim.
        /// </summary>
        public string UniqueId { get; }

        /// <summary>
        /// Gets the point in time in which the Access Token returned in the <see cref="AccessToken"/> property ceases to be valid.
        /// This value is calculated based on current UTC time measured locally and the value expiresIn received from the
        /// service.
        /// </summary>
        public DateTimeOffset ExpiresOn { get; }

        /// <summary>
        /// Gets the point in time in which the Access Token returned in the AccessToken property ceases to be valid in MSAL's extended LifeTime.
        /// This value is calculated based on current UTC time measured locally and the value ext_expiresIn received from the service.
        /// </summary>
        public DateTimeOffset ExtendedExpiresOn { get; }

        /// <summary>
        /// Gets the  Id Token if returned by the service or null if no Id Token is returned.
        /// </summary>
        public string IdToken { get; }

        /// <summary>
        /// Gets the granted scope values returned by the service.
        /// </summary>
        public IEnumerable<string> Scopes { get; }

        /// <summary>
        /// Creates the content for an HTTP authorization header from this authentication result, so
        /// that you can call a protected API.
        /// </summary>
        /// <returns>Created authorization header of the form "Bearer {AccessToken}".</returns>
        /// <example>
        /// Here is how you can call a protected API from this authentication result (in the <c>result</c>
        /// variable):
        /// <code>
        /// HttpClient client = new HttpClient();
        /// client.DefaultRequestHeaders.Add("Authorization", result.CreateAuthorizationHeader());
        /// HttpResponseMessage r = await client.GetAsync(urlOfTheProtectedApi);
        /// </code>
        /// </example>
        public string CreateAuthorizationHeader()
        {
            return Oauth2AuthorizationHeader + AccessToken;
        }
    }
}
