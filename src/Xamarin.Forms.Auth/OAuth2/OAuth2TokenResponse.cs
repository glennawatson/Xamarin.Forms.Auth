// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    internal class OAuth2TokenResponse : OAuth2ResponseBase
    {
        private long _expiresIn;
        private long _extendedExpiresIn;

        [JsonProperty(PropertyName = TokenResponseClaim.TokenType)]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = TokenResponseClaim.AccessToken)]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = TokenResponseClaim.RefreshToken)]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = TokenResponseClaim.Scope)]
        public string Scope { get; set; }

        [JsonProperty(PropertyName = TokenResponseClaim.ClientInfo)]
        public string ClientInfo { get; set; }

        [JsonProperty(PropertyName = TokenResponseClaim.IdToken)]
        public string IdToken { get; set; }

        [JsonProperty(PropertyName = TokenResponseClaim.ExpiresIn)]
        public long ExpiresIn
        {
            get => _expiresIn;
            set
            {
                _expiresIn = value;
                AccessTokenExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(_expiresIn);
            }
        }

        [JsonProperty(PropertyName = TokenResponseClaim.ExtendedExpiresIn)]
        public long ExtendedExpiresIn
        {
            get => _extendedExpiresIn;
            set
            {
                _extendedExpiresIn = value;
                AccessTokenExtendedExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(_extendedExpiresIn);
            }
        }

        [JsonProperty(PropertyName = TokenResponseClaim.FamilyId)]
        public string FamilyId { get; set; }

        [JsonIgnore]
        public DateTimeOffset AccessTokenExpiresOn { get; private set; }

        [JsonIgnore]
        public DateTimeOffset AccessTokenExtendedExpiresOn { get; private set; }

        [JsonIgnore]
        public Uri Authority { get; private set; }
    }
}
