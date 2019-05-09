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
    public class IdToken
    {
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.Issuer)]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.ObjectId)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the Subject-Identifier for the user at the issuer. It's a unique value to identify
        /// the user.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.Subject)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the tenant ID.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.TenantId)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the version of the response.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.Version)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the users preferred user name.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.PreferredUsername)]
        public string PreferredUsername { get; set; }

        /// <summary>
        /// Gets or sets the user's full name in displayable form including all name parts,
        /// possibly including titles and suffixes, ordered according to the user's locale and preferences.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.Name)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the home object id. Used mostly for Azure AD.
        /// </summary>
        [JsonProperty(PropertyName = IdTokenClaim.HomeObjectId)]
        public string HomeObjectId { get; set; }

        /// <summary>
        /// Gets or sets the given name(s) or first name(s) of the user.
        /// </summary>
        /// <remarks>
        /// Note that in some cultures,
        /// people can have multiple given names; all can be present, with the names being separated by space characters.
        /// </remarks>
        [JsonProperty(PropertyName = IdTokenClaim.GivenName)]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname(s) or last name(s) of the user.
        /// </summary>
        /// <remarks>
        /// Note that in some cultures, people can have multiple family names or no family name;
        /// all can be present, with the names being separated by space characters.
        /// </remarks>
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
        /// Gets or sets the URL of the user's profile picture. This URL MUST refer to an image file
        /// (for example, a PNG, JPEG, or GIF image file), rather than to a Web page containing an image.
        /// </summary>
        /// <remarks>
        /// Note that this URL SHOULD specifically reference a profile photo of the End-User suitable for
        /// displaying when describing the user, rather than an arbitrary photo taken by the user.
        /// </remarks>
        [JsonProperty("picture")]
        public string Picture { get; set; }

        /// <summary>
        /// Gets or sets the user's preferred e-mail address.
        /// </summary>
        /// <remarks>
        /// The RP MUST NOT rely upon this value being unique.
        /// </remarks>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the URL of the user's Web page or blog. This Web page SHOULD contain information
        /// published by the End-User or an organization that the End-User is affiliated with.
        /// </summary>
        [JsonProperty("website")]
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets true if the user's e-mail address has been verified; otherwise false.
        /// </summary>
        /// <remarks>
        /// When this Claim Value is true, this means that the OP took affirmative steps to ensure that this e-mail
        /// address was controlled by the user at the time the verification was performed. The means by which an
        /// e-mail address is verified is context-specific, and dependent upon the trust framework or contractual
        /// agreements within which the parties are operating.
        /// </remarks>
        [JsonProperty("email_verified")]
        public bool? EmailVerified { get; set; }

        /// <summary>
        /// Gets or sets the user's gender. Values defined by this specification are female and male. Other values
        /// MAY be used when neither of the defined values are applicable.
        /// </summary>
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the user's birthday, represented as an ISO 8601:2004 [ISO8601‑2004] YYYY-MM-DD format.
        /// The year MAY be 0000, indicating that it is omitted. To represent only the year, YYYY format is allowed.
        /// </summary>
        [JsonProperty("birthdate")]
        public string Birthdate { get; set; }

        /// <summary>
        /// Gets or sets a string from zoneinfo [http://openid.net/specs/openid-connect-core-1_0.html#zoneinfo] time zone
        /// database representing the End-User's time zone. For example, Europe/Paris or America/Los_Angeles.
        /// </summary>
        [JsonProperty("zoneinfo")]
        public string ZoneInformation { get; set; }

        /// <summary>
        /// Gets or sets the user's locale, represented as a BCP47 [RFC5646] language tag. This is typically an ISO 639-1 Alpha-2 [ISO639‑1]
        /// language code in lowercase and an ISO 3166-1 Alpha-2 [ISO3166‑1] country code in uppercase, separated by a dash.
        /// For example, en-US or fr-CA.
        /// </summary>
        /// <remarks>
        /// As a compatibility note, some implementations have used an underscore as the separator rather than a dash,
        /// for example, en_US; Relying Parties MAY choose to accept this locale syntax as well.
        /// </remarks>
        [JsonProperty("locale")]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets the user's preferred telephone number. E.164 [E.164] is RECOMMENDED as the format of this Claim, for example,
        /// +1 (425) 555-1212 or +56 (2) 687 2400. If the phone number contains an extension, it is RECOMMENDED that the
        /// extension be represented using the RFC 3966 [RFC3966] extension syntax, for example, +1 (604) 555-1234;ext=5678.
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets True if the End-User's phone number has been verified; otherwise false. When this Claim Value is true,
        /// this means that the OP took affirmative steps to ensure that this phone number was controlled by the user at
        /// the time the verification was performed. The means by which a phone number is verified is context-specific, and
        /// dependent upon the trust framework or contractual agreements within which the parties are operating.
        /// When true, the phone_number Claim MUST be in E.164 format and any extensions MUST be represented in RFC 3966 format.
        /// </summary>
        [JsonProperty("phone_number_verified")]
        public bool? PhoneNumberVerified { get; set; }

        /// <summary>
        /// Gets or sets additional claims about the user that are not represented by
        /// properties of the <see cref="IdToken" />.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        /// <summary>
        /// Parses the id token and constructs the IdToken class.
        /// </summary>
        /// <param name="idToken">The string to pass.</param>
        /// <returns>The instance of the class.</returns>
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

        /// <summary>
        /// Gets a unique id of the user. It prefers object id then the subject.
        /// </summary>
        /// <returns>The unique id.</returns>
        public string GetUniqueId()
        {
            return ObjectId ?? Subject;
        }
    }
}
