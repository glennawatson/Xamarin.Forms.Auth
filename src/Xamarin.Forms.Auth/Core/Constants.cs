// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

namespace Xamarin.Forms.Auth
{
    internal static class Constants
    {
        public const string MsAppScheme = "ms-app";
        public const int ExpirationMarginInMinutes = 5;
        public const int CodeVerifierLength = 128;
        public const int CodeVerifierByteSize = 32;

        public const string UapWEBRedirectUri = "https://sso"; // only ADAL supports WEB
        public const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        public const string DefaultConfidentialClientRedirectUri = "https://replyUrlNotSet";

        public const string DefaultRealm = "http://schemas.microsoft.com/rel/trusted-realm";

        public const string WellKnownOpenIdConfigurationPath = ".well-known/openid-configuration";
        public const string OpenIdConfigurationEndpoint = "v2.0/" + WellKnownOpenIdConfigurationPath;

        public static string FormatEnterpriseRegistrationOnPremiseUri(string domain)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "https://enterpriseregistration.{0}/enrollmentserver/contract",
                domain);
        }

        public static string FormatEnterpriseRegistrationInternetUri(string domain)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "https://enterpriseregistration.windows.net/{0}/enrollmentserver/contract",
                domain);
        }

        public static string FormatAdfsWebFingerUrl(string host, string resource)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "https://{0}/adfs/.well-known/webfinger?rel={1}&resource={2}",
                host,
                DefaultRealm,
                resource);
        }
    }
}
