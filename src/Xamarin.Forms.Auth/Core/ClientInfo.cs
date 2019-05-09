// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    internal class ClientInfo
    {
        [JsonProperty(PropertyName = ClientInfoClaim.UniqueIdentifier)]
        public string UniqueObjectIdentifier { get; set; }

        [JsonProperty(PropertyName = ClientInfoClaim.UniqueTenantIdentifier)]
        public string UniqueTenantIdentifier { get; set; }

        public static ClientInfo CreateFromJson(string clientInfo)
        {
            if (string.IsNullOrEmpty(clientInfo))
            {
                throw new AuthClientException(
                    AuthError.JsonParseError,
                    "client info is null");
            }

            try
            {
                return JsonHelper.DeserializeFromJson<ClientInfo>(Base64UrlHelpers.DecodeToBytes(clientInfo));
            }
            catch (Exception exc)
            {
                throw new AuthClientException(
                     AuthError.JsonParseError,
                     "Failed to parse the returned client info.",
                     exc);
            }
        }

        public string ToEncodedJson()
        {
            return Base64UrlHelpers.Encode(JsonHelper.SerializeToJson<ClientInfo>(this));
        }

        public string ToAccountIdentifier()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", UniqueObjectIdentifier, UniqueTenantIdentifier);
        }
    }
}
