// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// An OAuth response that contains tokens.
    /// </summary>
    public class OAuth2TokenResponse : OAuth2ResponseBase
    {
        private long _expiresIn;
        private long _extendedExpiresIn;

        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the client information.
        /// </summary>
        [JsonProperty(PropertyName = "client_info")]
        public string ClientInfo { get; set; }

        /// <summary>
        /// Gets or sets the id token.
        /// </summary>
        [JsonProperty(PropertyName = "id_token")]
        public string IdToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public long ExpiresIn
        {
            get => _expiresIn;
            set
            {
                _expiresIn = value;
                AccessTokenExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(_expiresIn);
            }
        }

        /// <summary>
        /// Gets or sets the extended expiration time.
        /// </summary>
        [JsonProperty(PropertyName = "ext_expires_in")]
        public long ExtendedExpiresIn
        {
            get => _extendedExpiresIn;
            set
            {
                _extendedExpiresIn = value;
                AccessTokenExtendedExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(_extendedExpiresIn);
            }
        }

        /// <summary>
        /// Gets the access token expiration date time.
        /// </summary>
        [JsonIgnore]
        public DateTimeOffset AccessTokenExpiresOn { get; private set; }

        /// <summary>
        /// Gets the access token extended date time.
        /// </summary>
        [JsonIgnore]
        public DateTimeOffset AccessTokenExtendedExpiresOn { get; private set; }
    }
}
