// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Xamarin.Forms.Auth
{
    [DataContract]
    internal class AuthorizationResult
    {
        internal AuthorizationResult(AuthorizationStatus status, string returnedUriInput) : this(status)
        {
            if (Status == AuthorizationStatus.UserCancel)
            {
                Error = CoreErrorCodes.AuthenticationCanceledError;
                ErrorDescription = CoreErrorMessages.AuthenticationCanceled;
            }
            else if (Status == AuthorizationStatus.UnknownError)
            {
                Error = CoreErrorCodes.UnknownError;
                ErrorDescription = CoreErrorMessages.Unknown;
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

        public AuthorizationStatus Status { get; private set; }

        [DataMember]
        public string Code { get; private set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }

        [DataMember]
        public string CloudInstanceHost { get; set; }

        public string State { get; set; }

        public void ParseAuthorizeResponse(string webAuthenticationResult)
        {
            var resultUri = new Uri(webAuthenticationResult);

            // NOTE: The Fragment property actually contains the leading '#' character and that must be dropped
            string resultData = resultUri.Query;

            if (!string.IsNullOrWhiteSpace(resultData))
            {
                // RemoveAccount the leading '?' first
                Dictionary<string, string> response = CoreHelpers.ParseKeyValueList(resultData.Substring(1), '&',
                    true, null);

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
                else if (response.ContainsKey(TokenResponseClaim.Error))
                {
                    Error = response[TokenResponseClaim.Error];
                    ErrorDescription = response.ContainsKey(TokenResponseClaim.ErrorDescription)
                        ? response[TokenResponseClaim.ErrorDescription]
                        : null;
                    Status = AuthorizationStatus.ProtocolError;
                }
                else
                {
                    Error = CoreErrorCodes.AuthenticationFailed;
                    ErrorDescription = CoreErrorMessages.AuthorizationServerInvalidResponse;
                    Status = AuthorizationStatus.UnknownError;
                }

                if (response.ContainsKey(TokenResponseClaim.CloudInstanceHost))
                {
                    CloudInstanceHost = response[TokenResponseClaim.CloudInstanceHost];
                }
            }
            else
            {
                Error = CoreErrorCodes.AuthenticationFailed;
                ErrorDescription = CoreErrorMessages.AuthorizationServerInvalidResponse;
                Status = AuthorizationStatus.UnknownError;
            }
        }
    }
}
