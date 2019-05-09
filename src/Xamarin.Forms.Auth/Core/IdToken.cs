// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Represents the information of a user as available from
    /// a standard OIDC /userinfo endpoint.
    /// http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
    /// https://docs.microsoft.com/en-us/azure/active-directory/develop/id-tokens#payload-claims
    /// Also contains some additional values available from the Microsoft standard.
    /// </summary>
     internal class IdToken
    {
        [JsonProperty(PropertyName = IdTokenClaim.Issuer)]
        public string Issuer { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.ObjectId)]
        public string ObjectId { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.Subject)]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.TenantId)]
        public string TenantId { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.Version)]
        public string Version { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.PreferredUsername)]
        public string PreferredUsername { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.Name)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.HomeObjectId)]
        public string HomeObjectId { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.GivenName)]
        public string GivenName { get; set; }

        [JsonProperty(PropertyName = IdTokenClaim.FamilyName)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the middle name(s) of the user.
        /// </summary>
        /// <remarks>
        /// Note that in some cultures, people can have multiple middle names; all can be present, with the names
        /// being separated by space characters. Also note that in some cultures, middle names are not used.
        /// </remarks>
        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets additional claims about the user that are not represented by
        /// properties of the <see cref="IdToken" />.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        public static IdToken Parse(string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
            {
                return null;
            }

            string[] idTokenSegments = idToken.Split(new[] { '.' });

            if (idTokenSegments.Length < 2)
            {
                throw new AuthClientException(
                    AuthError.InvalidJwtError,
                    AuthErrorMessage.IDTokenMustHaveTwoParts);
            }

            try
            {
                var idTokenString = Base64UrlHelpers.DecodeToString(idTokenSegments[1]);
                return JsonConvert.DeserializeObject<IdToken>(idTokenString);
            }
            catch (Exception exc)
            {
                throw new AuthClientException(
                    AuthError.JsonParseError,
                    AuthErrorMessage.FailedToParseIDToken,
                    exc);
            }
        }

        public string GetUniqueId()
        {
            return ObjectId ?? Subject;
        }
    }
}
