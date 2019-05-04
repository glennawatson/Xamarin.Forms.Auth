// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Platform / OS specific logic. No library (ADAL / MSAL) specific code should go in here.
    /// </summary>
    internal class UapPlatformProxy : IPlatformProxy
    {
        private readonly Lazy<IPlatformLogger> _platformLogger =
            new Lazy<IPlatformLogger>(() => new EventSourcePlatformLogger());
        private IWebUIFactory _overloadWebUiFactory;

        /// <summary>
        /// Get the user logged in to Windows or throws
        /// </summary>
        /// <remarks>
        /// Win10 allows several identities to be logged in at once;
        /// select the first principal name that can be used
        /// </remarks>
        /// <returns>The username or throws</returns>
        public async Task<string> GetUserPrincipalNameAsync()
        {
            IReadOnlyList<User> users = await User.FindAllAsync();
            if (users == null || !users.Any())
            {
                throw MsalExceptionFactory.GetClientException(
                    CoreErrorCodes.CannotAccessUserInformationOrUserNotDomainJoined,
                    CoreErrorMessages.UapCannotFindDomainUser);
            }

            var getUserDetailTasks = users.Select(async u =>
            {
                object domainObj = await u.GetPropertyAsync(KnownUserProperties.DomainName);
                string domainString = domainObj?.ToString();

                object principalObject = await u.GetPropertyAsync(KnownUserProperties.PrincipalName);
                string principalNameString = principalObject?.ToString();

                return new { Domain = domainString, PrincipalName = principalNameString };
            }).ToList();

            var userDetails = await Task.WhenAll(getUserDetailTasks).ConfigureAwait(false);

            // try to get a user that has both domain name and upn
            var userDetailWithDomainAndPn = userDetails.FirstOrDefault(
                d => !string.IsNullOrWhiteSpace(d.Domain) &&
                !string.IsNullOrWhiteSpace(d.PrincipalName));

            if (userDetailWithDomainAndPn != null)
            {
                return userDetailWithDomainAndPn.PrincipalName;
            }

            // try to get a user that at least has upn
            var userDetailWithPn = userDetails.FirstOrDefault(
              d => !string.IsNullOrWhiteSpace(d.PrincipalName));

            if (userDetailWithPn != null)
            {
                return userDetailWithPn.PrincipalName;
            }

            // user has domain name, but no upn -> missing Enterprise Auth capability
            if (userDetails.Any(d => !string.IsNullOrWhiteSpace(d.Domain)))
            {
                throw MsalExceptionFactory.GetClientException(
                   CoreErrorCodes.CannotAccessUserInformationOrUserNotDomainJoined,
                   CoreErrorMessages.UapCannotFindUpn);
            }

            // no domain, no upn -> missing User Info capability
            throw MsalExceptionFactory.GetClientException(
                CoreErrorCodes.CannotAccessUserInformationOrUserNotDomainJoined,
                CoreErrorMessages.UapCannotFindDomainUser);

        }

        public async Task<bool> IsUserLocalAsync(RequestContext requestContext)
        {
            IReadOnlyList<User> users = await User.FindAllAsync();
            return users.Any(u => u.Type == UserType.LocalUser || u.Type == UserType.LocalGuest);
        }

        public bool IsDomainJoined()
        {
            return NetworkInformation.GetHostNames().Any(entry => entry.Type == HostNameType.DomainName);
        }


        public string GetEnvironmentVariable(string variable)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values.ContainsKey(variable) ? localSettings.Values[variable].ToString() : null;
        }

        public string GetProcessorArchitecture()
        {
            return WindowsNativeMethods.GetProcessorArchitecture();
        }

        public string GetOperatingSystem()
        {
            // In WinRT, there is no way to reliably get OS version. All can be done reliably is to check
            // for existence of specific features which does not help in this case, so we do not emit OS in WinRT.
            return null;
        }

        public string GetDeviceModel()
        {
            var deviceInformation = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            return deviceInformation.SystemProductName;
        }

        /// <inheritdoc />
        public string GetBrokerOrRedirectUri(Uri redirectUri)
        {
            return redirectUri.OriginalString;
        }

        /// <inheritdoc />
        public string GetDefaultRedirectUri(string correlationId)
        {
            return Constants.DefaultRedirectUri;
        }

        public string GetProductName()
        {
            return "MSAL.UAP";
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Name of the calling application</returns>
        public string GetCallingApplicationName()
        {
            return Package.Current?.DisplayName?.ToString();
        }

        /// <summary>
        /// Considered PII, ensure that it is hashed.
        /// </summary>
        /// <returns>Version of the calling application</returns>
        public string GetCallingApplicationVersion()
        {
            return Package.Current?.Id?.Version.ToString();
        }

        /// <summary>
        /// Considered PII. Please ensure that it is hashed.
        /// </summary>
        /// <returns>Device identifier</returns>
        public string GetDeviceId()
        {
            return new EasClientDeviceInformation()?.Id.ToString();
        }

        public ILegacyCachePersistence CreateLegacyCachePersistence()
        {
            return new UapLegacyCachePersistence(CryptographyManager);
        }

        public ITokenCacheAccessor CreateTokenCacheAccessor()
        {
            return new UapTokenCacheAccessor(CryptographyManager);
        }

        /// <inheritdoc />
        public ICryptographyManager CryptographyManager { get; } = new UapCryptographyManager();

        /// <inheritdoc />
        public IPlatformLogger PlatformLogger => _platformLogger.Value;

        /// <inheritdoc />
        public IWebUIFactory GetWebUiFactory()
        {
            return _overloadWebUiFactory ?? new WebUIFactory();
        }

        /// <inheritdoc />
        public void SetWebUiFactory(IWebUIFactory webUiFactory)
        {
            _overloadWebUiFactory = webUiFactory;
        }
    }
}
