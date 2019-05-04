namespace Xamarin.Forms.Auth
{
    internal static class OAuth2Value
    {
        public const string CodeChallengeMethodValue = "S256";
        public const string ScopeOpenId = "openid";
        public const string ScopeOfflineAccess = "offline_access";
        public const string ScopeProfile = "profile";
        public static readonly string[] ReservedScopes = { ScopeOpenId, ScopeProfile, ScopeOfflineAccess };
    }
}