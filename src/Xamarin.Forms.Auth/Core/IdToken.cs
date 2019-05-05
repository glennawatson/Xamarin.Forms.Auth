// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using JWT;
using JWT.Serializers;

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
        /// <summary>
        /// Gets or sets the security token service (STS) that constructs and returns the token,
        /// and the Azure AD tenant in which the user was authenticated.
        /// </summary>
        [JsonProperty(PropertyName = "iss")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets an immutable identifier for an object in the Microsoft identity system, in this case, a user account.
        /// This ID uniquely identifies the user across applications.
        /// </summary>
        [JsonProperty(PropertyName = "oid")]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets a casual name of the user that may or may not be the same as the given_name.
        /// For instance, a nickname value of 'Mike' might be returned alongside a given_name value of 'Michael'.
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the Subject-Identifier for the user at the issuer. It's a unique value to identify
        /// the user.
        /// </summary>
        [JsonProperty(PropertyName = "sub")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets a GUID that represents the Azure AD tenant that the user is from.
        /// For work and school accounts, the GUID is the immutable tenant ID of the organization that the user belongs to.
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the version of the ID token.
        /// </summary>
        [JsonProperty(PropertyName = "ver")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the shorthand name by which the user wishes to be referred to at the RP, such as janedoe or j.doe.
        /// This value MAY be any valid JSON string including special characters such as @, /, or whitespace.
        /// The RP MUST NOT rely upon this value being unique.
        /// </summary>
        [JsonProperty(PropertyName = "preferred_username")]
        public string PreferredUsername { get; set; }

        /// <summary>
        /// Gets or sets the user's full name in displayable form including all name parts,
        /// possibly including titles and suffixes, ordered according to the user's locale and preferences.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets for guest users, the object ID of the user in the user's home tenant.
        /// </summary>
        [JsonProperty(PropertyName = "home_oid")]
        public string HomeObjectId { get; set; }

        /// <summary>
        /// Gets or sets the given name(s) or first name(s) of the user.
        /// </summary>
        /// <remarks>
        /// Note that in some cultures,
        /// people can have multiple given names; all can be present, with the names being separated by space characters.
        /// </remarks>
        [JsonProperty(PropertyName = "given_name")]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname(s) or last name(s) of the user.
        /// </summary>
        /// <remarks>
        /// Note that in some cultures, people can have multiple family names or no family name;
        /// all can be present, with the names being separated by space characters.
        /// </remarks>
        [JsonProperty(PropertyName = "family_name")]
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

        public static IdToken Parse(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            try
            {
                var urlEncoder = new JwtBase64UrlEncoder();
                var decoder = new JwtDecoder(new JsonNetSerializer(), null, urlEncoder);
                return decoder.DecodeToObject<IdToken>(token);
            }
            catch (Exception exc)
            {
                throw ExceptionFactory.GetClientException(
                    CoreErrorCodes.JsonParseError,
                    CoreErrorMessages.FailedToParseIDToken,
                    exc);
            }
        }

        public string GetUniqueId()
        {
            return ObjectId ?? Subject;
        }
    }
}
