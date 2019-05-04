// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains the results of one token acquisition operation in <see cref="PublicClientApplication"/>
    /// or <see cref="T:ConfidentialClientApplication"/>. For details see https://aka.ms/msal-net-authenticationresult
    /// </summary>
    public partial class AuthenticationResult
    {
        private const string Oauth2AuthorizationHeader = "Bearer ";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
        /// Constructor meant to help application developers test their apps. Allows mocking of authentication flows.
        /// App developers should never new-up <see cref="AuthenticationResult"/> in product code.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="isExtendedLifeTimeToken"></param>
        /// <param name="uniqueId"></param>
        /// <param name="expiresOn"></param>
        /// <param name="extendedExpiresOn"></param>
        /// <param name="account"></param>
        /// <param name="idToken"></param>
        /// <param name="scopes"></param>
        public AuthenticationResult(
            string accessToken,
            bool isExtendedLifeTimeToken,
            string uniqueId,
            DateTimeOffset expiresOn,
            DateTimeOffset extendedExpiresOn,
            IAccount account,
            string idToken,
            IEnumerable<string> scopes)
        {
            AccessToken = accessToken;
            IsExtendedLifeTimeToken = isExtendedLifeTimeToken;
            UniqueId = uniqueId;
            ExpiresOn = expiresOn;
            ExtendedExpiresOn = extendedExpiresOn;
            Account = account;
            IdToken = idToken;
            Scopes = scopes;
        }

        internal AuthenticationResult()
        {
        }

        internal AuthenticationResult(MsalAccessTokenCacheItem msalAccessTokenCacheItem, MsalIdTokenCacheItem msalIdTokenCacheItem)
        {
            if (msalAccessTokenCacheItem.HomeAccountId != null)
            {
                Account = new Account(
                    msalAccessTokenCacheItem.HomeAccountId,
                    msalIdTokenCacheItem?.IdToken?.PreferredUsername,
                    msalAccessTokenCacheItem.Environment);
            }

            AccessToken = msalAccessTokenCacheItem.Secret;
            UniqueId = msalIdTokenCacheItem?.IdToken?.GetUniqueId();
            ExpiresOn = msalAccessTokenCacheItem.ExpiresOn;
            ExtendedExpiresOn = msalAccessTokenCacheItem.ExtendedExpiresOn;
            IdToken = msalIdTokenCacheItem?.Secret;
            Scopes = msalAccessTokenCacheItem.ScopeSet;
            IsExtendedLifeTimeToken = msalAccessTokenCacheItem.IsExtendedLifeTimeToken;
        }

        /// <summary>
        /// Gets the access token that can be used as a bearer token to access protected web APIs
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// In case when Azure AD has an outage, to be more resilient, it can return tokens with
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
        /// Gets the account information. Some elements in <see cref="IAccount"/> might be null if not returned by the
        /// service. The account can be passed back in some API overloads to identify which account should be used such
        /// as <see cref="IClientApplicationBase.AcquireTokenSilentAsync(IEnumerable{string}, IAccount)"/> or
        /// <see cref="IClientApplicationBase.RemoveAsync(IAccount)"/> for instance.
        /// </summary>
        public IAccount Account { get; }

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
