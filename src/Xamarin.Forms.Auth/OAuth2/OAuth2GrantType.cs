namespace Xamarin.Forms.Auth
{
    internal static class OAuth2GrantType
    {
        public const string AuthorizationCode = "authorization_code";
        public const string RefreshToken = "refresh_token";
        public const string ClientCredentials = "client_credentials";
        public const string Saml11Bearer = "urn:ietf:params:oauth:grant-type:saml1_1-bearer";
        public const string Saml20Bearer = "urn:ietf:params:oauth:grant-type:saml2-bearer";
        public const string JwtBearer = "urn:ietf:params:oauth:grant-type:jwt-bearer";
        public const string Password = "password";
        public const string DeviceCode = "device_code";
    }
}