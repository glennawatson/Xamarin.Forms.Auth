// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
