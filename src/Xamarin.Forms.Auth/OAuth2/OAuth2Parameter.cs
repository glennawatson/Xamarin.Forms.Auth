// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal static class OAuth2Parameter
    {
        public const string ResponseType = "response_type";
        public const string GrantType = "grant_type";
        public const string ClientId = "client_id";
        public const string ClientSecret = "client_secret";
        public const string ClientAssertion = "client_assertion";
        public const string ClientAssertionType = "client_assertion_type";
        public const string RefreshToken = "refresh_token";
        public const string RedirectUri = "redirect_uri";
        public const string Resource = "resource";
        public const string Code = "code";
        public const string DeviceCode = "device_code";
        public const string Scope = "scope";
        public const string Assertion = "assertion";
        public const string RequestedTokenUse = "requested_token_use";
        public const string Username = "username";
        public const string Password = "password";
        public const string LoginHint = "login_hint"; // login_hint is not standard oauth2 parameter
        public const string CorrelationId = OAuth2Header.CorrelationId;
        public const string State = "state";

        public const string CodeChallengeMethod = "code_challenge_method";
        public const string CodeChallenge = "code_challenge";
        public const string CodeVerifier = "code_verifier";

        // correlation id is not standard oauth2 parameter
        public const string LoginReq = "login_req";
        public const string DomainReq = "domain_req";

        public const string Prompt = "prompt"; // prompt is not standard oauth2 parameter
        public const string ClientInfo = "client_info"; // restrict_to_hint is not standard oauth2 parameter

        public const string Claims = "claims"; // claims is not a standard oauth2 paramter
    }
}
