// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal static class BrokerConstants
    {
        public const string ChallengeResponseHeader = "Authorization";

        public const string ChallengeResponseType = "PKeyAuth";

        public const string ChallengeResponseToken = "AuthToken";

        public const string ChallengeResponseContext = "Context";

        public const string ChallengeResponseVersion = "Version";

        public const string BrowserExtPrefix = "browser://";

        public const string BrowserExtInstallPrefix = "msauth://";

        public const string DeviceAuthChallengeRedirect = "urn:http-auth:PKeyAuth";
        public const string ChallengeHeaderKey = "x-ms-PKeyAuth";
        public const string ChallengeHeaderValue = "1.0";
    }
}
