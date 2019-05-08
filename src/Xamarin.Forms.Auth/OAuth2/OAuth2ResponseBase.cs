// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A response from the OAuth2 authentication.
    /// </summary>
    public class OAuth2ResponseBase
    {
        /// <summary>
        /// Gets or sets any error string.
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets any error description.
        /// </summary>
        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets any error codes.
        /// </summary>
        [JsonProperty(PropertyName = "error_codes")]
        public string[] ErrorCodes { get; set; }

        /// <summary>
        /// Gets or sets the correlation id.
        /// </summary>
        [JsonProperty(PropertyName = "correlation_id")]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets any claims.
        /// </summary>
        [JsonProperty(PropertyName = "claims")]
        public string Claims { get; set; }
    }
}
