// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
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

        public static IdToken Parse(string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
            {
                return null;
            }

            IdToken idTokenBody = null;
            string[] idTokenSegments = idToken.Split(new[] { '.' });

            if (idTokenSegments.Length < 2)
            {
                throw ExceptionFactory.GetClientException(
                    CoreErrorCodes.InvalidJwtError,
                    CoreErrorMessages.IDTokenMustHaveTwoParts);
            }

            try
            {
                byte[] idTokenBytes = Base64UrlHelpers.DecodeToBytes(idTokenSegments[1]);
                using (var stream = new MemoryStream(idTokenBytes))
                {
                    var serializer = new DataContractJsonSerializer(typeof(IdToken));
                    idTokenBody = (IdToken)serializer.ReadObject(stream);
                }
            }
            catch (Exception exc)
            {
                throw ExceptionFactory.GetClientException(
                    CoreErrorCodes.JsonParseError,
                    CoreErrorMessages.FailedToParseIDToken,
                    exc);
            }

            return idTokenBody;
        }

        public string GetUniqueId()
        {
            return ObjectId ?? Subject;
        }
    }
}
