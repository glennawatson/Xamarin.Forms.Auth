// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    internal class OAuth2ResponseBase
    {
        [JsonProperty(PropertyName = OAuth2ResponseBaseClaim.Error)]
        public string Error { get; set; }

        [JsonProperty(PropertyName = OAuth2ResponseBaseClaim.SubError)]
        public string SubError { get; set; }

        [JsonProperty(PropertyName = OAuth2ResponseBaseClaim.ErrorDescription)]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets do not expose these in the MsalException because Evo does not guarantee that the error
        /// codes remain the same.
        /// </summary>
        [JsonProperty(PropertyName = OAuth2ResponseBaseClaim.ErrorCodes)]
        public string[] ErrorCodes { get; set; }

        [JsonProperty(PropertyName = OAuth2ResponseBaseClaim.CorrelationId)]
        public string CorrelationId { get; set; }

        [JsonProperty(PropertyName = OAuth2ResponseBaseClaim.Claims)]
        public string Claims { get; set; }
    }
}
