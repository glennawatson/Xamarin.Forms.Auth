// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    internal class AuthorizationResult
    {
        internal AuthorizationResult(AuthorizationStatus status, string returnedUriInput)
            : this(status)
        {
            if (Status == AuthorizationStatus.UserCancel)
            {
                Error = AuthError.AuthenticationCanceledError;
#if ANDROID
                ErrorDescription = AuthErrorMessage.AuthenticationCanceledAndroid;
#else
                ErrorDescription = AuthErrorMessage.AuthenticationCanceled;
#endif
            }
            else if (Status == AuthorizationStatus.UnknownError)
            {
                Error = AuthError.UnknownError;
                ErrorDescription = AuthErrorMessage.Unknown;
            }
            else
            {
                ParseAuthorizeResponse(returnedUriInput);
            }
        }

        internal AuthorizationResult(AuthorizationStatus status)
        {
            Status = status;
        }

        [JsonIgnore]
        public AuthorizationStatus Status { get; private set; }

        public string Code { get; private set; }

        public string Error { get; set; }

        public string ErrorDescription { get; set; }

        public string CloudInstanceHost { get; set; }

        /// <summary>
        /// Gets or sets a string that is added to each authorization Request and is expected to be sent back along with the
        /// authorization code. MSAL is responsible for validating that the state sent is identical to the state received.
        /// </summary>
        /// <remarks>
        /// This is in addition to PKCE, which is validated by the server to ensure that the system redeeming the auth code
        /// is the same as the system who asked for it. It protects against XSRF https://openid.net/specs/openid-connect-core-1_0.html.
        /// </remarks>
        public string State { get; set; }

        public void ParseAuthorizeResponse(string webAuthenticationResult)
        {
            var resultUri = new Uri(webAuthenticationResult);

            // NOTE: The Fragment property actually contains the leading '#' character and that must be dropped
            string resultData = resultUri.Query;

            if (!string.IsNullOrWhiteSpace(resultData))
            {
                // RemoveAccount the leading '?' first
                Dictionary<string, string> response = CoreHelpers.ParseKeyValueList(
                    resultData.Substring(1),
                    '&',
                    true,
                    null);

                if (response.ContainsKey(OAuth2Parameter.State))
                {
                    State = response[OAuth2Parameter.State];
                }

                if (response.ContainsKey(TokenResponseClaim.Code))
                {
                    Code = response[TokenResponseClaim.Code];
                }
                else if (webAuthenticationResult.StartsWith("msauth://", StringComparison.OrdinalIgnoreCase))
                {
                    Code = webAuthenticationResult;
                }
                else if (response.ContainsKey(OAuth2ResponseBaseClaim.Error))
                {
                    Error = response[OAuth2ResponseBaseClaim.Error];
                    ErrorDescription = response.ContainsKey(OAuth2ResponseBaseClaim.ErrorDescription)
                        ? response[OAuth2ResponseBaseClaim.ErrorDescription]
                        : null;
                    Status = AuthorizationStatus.ProtocolError;
                }
                else
                {
                    Error = AuthError.AuthenticationFailed;
                    ErrorDescription = AuthErrorMessage.AuthorizationServerInvalidResponse;
                    Status = AuthorizationStatus.UnknownError;
                }

                if (response.ContainsKey(TokenResponseClaim.CloudInstanceHost))
                {
                    CloudInstanceHost = response[TokenResponseClaim.CloudInstanceHost];
                }
            }
            else
            {
                Error = AuthError.AuthenticationFailed;
                ErrorDescription = AuthErrorMessage.AuthorizationServerInvalidResponse;
                Status = AuthorizationStatus.UnknownError;
            }
        }
    }
}
