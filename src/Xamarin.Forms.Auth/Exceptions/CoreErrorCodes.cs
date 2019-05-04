// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Error codes attached to each exception.
    /// </summary>
    internal class CoreErrorCodes
    {
        public const string JsonParseError = "json_parse_failed";
        public const string RequestTimeout = "request_timeout";
        public const string ServiceNotAvailable = "service_not_available";

        public const string InvalidJwtError = "invalid_jwt";
        public const string TenantDiscoveryFailedError = "tenant_discovery_failed";
        public const string InvalidAuthorityType = "invalid_authority_type";
        public const string AuthenticationUiFailedError = "authentication_ui_failed";
        public const string InvalidGrantError = "invalid_grant";
        public const string InvalidRequest = "invalid_request";
        public const string UnknownError = "unknown_error";
        public const string AuthenticationCanceledError = "authentication_canceled";
        public const string AuthenticationFailed = "authentication_failed";
        public const string AuthenticationUiFailed = "authentication_ui_failed";
        public const string NonHttpsRedirectNotSupported = "non_https_redirect_failed";

        public const string UpnRequired = "upn_required";
        public const string MissingPassiveAuthEndpoint = "missing_passive_auth_endpoint";
        public const string InvalidAuthority = "invalid_authority";

        public const string PlatformNotSupported = "platform_not_supported";
        public const string InternalError = "internal_error";
        public const string AccessingWsMetadataExchangeFailed = "accessing_ws_metadata_exchange_failed";
        public const string FederatedServiceReturnedError = "federated_service_returned_error";
        public const string ParsingWsTrustResponseFailed = "parsing_wstrust_response_failed";

        public const string UnknownUser = "unknown_user";
        public const string UserRealmDiscoveryFailed = "user_realm_discovery_failed";

        /// <summary>
        /// Federation Metadata Url is missing for federated user.
        /// </summary>
        public const string MissingFederationMetadataUrl = "missing_federation_metadata_url";
        public const string WsTrustEndpointNotFoundInMetadataDocument = "wstrust_endpoint_not_found";
        public const string ParsingWsMetadataExchangeFailed = "parsing_ws_metadata_exchange_failed";
        public const string UnknownUserType = "unknown_user_type";
        public const string CannotAccessUserInformationOrUserNotDomainJoined = "user_information_access_failed";

        public const string UapCannotFindDomainUser = "user_information_access_failed";
        public const string UapCannotFindUpn = "uap_cannot_find_upn";

        public const string GetUserNameFailed = "get_user_name_failed";
        public const string NonParsableOAuthError = "non_parsable_oauth_error";
        public const string HttpStatusNotFound = "not_found";
        public const string HttpStatusCodeNotOk = "http_status_not_200";

        /// <summary>
        /// RedirectUri validation failed.
        /// </summary>
        public const string DefaultRedirectUriIsInvalid = "redirect_uri_validation_failed";
        public const string NoRedirectUri = "no_redirect_uri";

#if iOS
        public const string CannotAccessPublisherKeyChain = "cannot_access_publisher_keychain";
        public const string MissingEntitlements = "missing_entitlements";
#endif

#if ANDROID
        public const string FailedToCreateSharedPreference = "shared_preference_creation_failed";
        public const string AndroidActivityNotFound = "android_activity_not_found";
        public const string UnresolvableIntentError = "unresolvable_intent";
#endif
    }
}
