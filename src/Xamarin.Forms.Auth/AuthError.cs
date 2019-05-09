// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Error code returned as a property in MsalException.
    /// </summary>
    public static class AuthError
    {
        /// <summary>
        /// Standard OAuth2 protocol error code. It indicates that the application needs to expose the UI to the user
        /// so that the user does an interactive action in order to get a new token.
        /// <para>Mitigation:.</para> If your application is a <see cref="IPublicClientApplication"/> call <c>AcquireTokenInteractive</c>
        /// perform an interactive authentication.
        /// </summary>
        public const string InvalidGrantError = "invalid_grant";

        /// <summary>
        /// No token was found in the token cache.
        /// </summary>
        public const string NoTokensFoundError = "no_tokens_found";

        /// <summary>
        /// This error code comes back from <see cref="IClientApplicationBase.AcquireTokenSilent(System.Collections.Generic.IEnumerable{string}, string)"/> calls when a null user is
        /// passed as the <c>account</c> parameter. This can be because you have called AcquireTokenSilent with an <c>account</c> parameter
        /// set to <c>accounts.FirstOrDefault()</c> but <c>accounts</c> is empty.
        /// <para>Mitigation.</para>
        /// Pass a different account, or otherwise call <see cref="IPublicClientApplication.AcquireTokenInteractive(System.Collections.Generic.IEnumerable{string})"/>.
        /// </summary>
        public const string UserNullError = "user_null";

        /// <summary>
        /// This error code comes back from <see cref="ClientApplicationBase.AcquireTokenSilent(System.Collections.Generic.IEnumerable{string}, string)"/> calls when
        /// the user cache had not been set in the application constructor. This should never happen in MSAL.NET 3.x as the cache is created by the application.
        /// </summary>
        public const string TokenCacheNullError = "token_cache_null";

        /// <summary>
        /// One of two conditions was encountered:
        /// <list type="bullet">
        /// <item><description>The <c>Prompt.NoPrompt</c> was passed in an interactive token call, but the constraint could not be honored because user interaction is required,
        /// for instance because the user needs to re-sign-in, give consent for more scopes, or perform multiple factor authentication.
        /// </description></item>
        /// <item><description>
        /// An error occurred during a silent web authentication that prevented the authentication flow from completing in a short enough time frame.
        /// </description></item>
        /// </list>
        /// <para>Remediation:.</para>call <c>AcquireTokenInteractive</c> so that the user of your application signs-in and accepts consent.
        /// </summary>
        public const string NoPromptFailedError = "no_prompt_failed";

        /// <summary>
        /// Service is unavailable and returned HTTP error code within the range of 500-599.
        /// <para>Mitigation.</para> you can retry after a delay.
        /// </summary>
        public const string ServiceNotAvailable = "service_not_available";

        /// <summary>
        /// The Http Request to the STS timed out.
        /// <para>Mitigation.</para> you can retry after a delay.
        /// </summary>
        public const string RequestTimeout = "request_timeout";

        /// <summary>
        /// Unknown Error occured.
        /// <para>Mitigation.</para> None. You might want to inform the end user.
        /// </summary>
        public const string UnknownError = "unknown_error";

        /// <summary>
        /// Authentication failed.
        /// <para>What happens?.</para>
        /// The authentication failed. For instance the user did not enter the right password.
        /// <para>Mitigation.</para>
        /// Inform the user to retry.
        /// </summary>
        public const string AuthenticationFailed = "authentication_failed";

        /// <summary>
        /// User Realm Discovery Failed.
        /// </summary>
        public const string UserRealmDiscoveryFailed = "user_realm_discovery_failed";

        /// <summary>
        /// Federation Metadata Url is missing for federated user.
        /// </summary>
        public const string MissingFederationMetadataUrl = "missing_federation_metadata_url";

        /// <summary>
        /// Parsing WS Metadata Exchange Failed.
        /// </summary>
        public const string ParsingWsMetadataExchangeFailed = "parsing_ws_metadata_exchange_failed";

        /// <summary>
        /// WS-Trust Endpoint Not Found in Metadata Document.
        /// </summary>
        public const string WsTrustEndpointNotFoundInMetadataDocument = "wstrust_endpoint_not_found";

        /// <summary>
        /// An error response was returned by the OAuth2 server and it could not be parsed.
        /// </summary>
        public const string NonParsableOAuthError = "non_parsable_oauth_error";

        /// <summary>
        /// Error code used when the http response returns HttpStatusCode.NotFound.
        /// </summary>
        public const string HttpStatusNotFound = "not_found";

        /// <summary>
        /// Request is invalid.
        /// </summary>
        public const string InvalidRequest = "invalid_request";

        /// <summary>
        /// The request could not be preformed because of an unknown failure in the UI flow.*.
        /// <para>Mitigation.</para>
        /// Inform the user.
        /// </summary>
        public const string AuthenticationUiFailed = "authentication_ui_failed";

        /// <summary>
        /// ErrorCode used when the http response returns something different from 200 (OK).
        /// </summary>
        /// <remarks>
        /// HttpStatusCode.NotFound have a specific error code. <see cref="AuthError.HttpStatusNotFound"/>.
        /// </remarks>
        public const string HttpStatusCodeNotOk = "http_status_not_200";

        /// <summary>
        /// Error code used when the <see cref="ICustomWebUi"/> has returned an uri, but it is invalid - it is either null or has no code.
        /// Consider throwing an exception if you are unable to intercept the uri containing the code.
        /// </summary>
        public const string CustomWebUiReturnedInvalidUri = "custom_webui_returned_invalid_uri";

        /// <summary>
        /// Error code used when the CustomWebUI has returned an uri, but it does not match the Authroity and AbsolutePath of
        /// the configured redirect uri.
        /// </summary>
        public const string CustomWebUiRedirectUriMismatch = "custom_webui_invalid_mismatch";

        // TODO: does not seem to be used?

        /// <summary>
        /// Access denied.
        /// </summary>
        public const string AccessDenied = "access_denied";

        /// <summary>
        /// RedirectUri validation failed.
        /// </summary>
        public const string DefaultRedirectUriIsInvalid = "redirect_uri_validation_failed";

        /// <summary>
        /// No Redirect URI.
        /// <para>What happens?.</para>
        /// You need to provide a Reply URI / Redirect URI, but have not called <see cref="AbstractApplicationBuilder{T}.WithRedirectUri(Uri)"/>.
        /// </summary>
        public const string NoRedirectUri = "no_redirect_uri";

        /// <summary>
        /// Multiple Tokens were matched.
        /// <para>What happens?.</para>This exception happens in the case of applications managing several identities,
        /// when calling <see cref="ClientApplicationBase.AcquireTokenSilent(System.Collections.Generic.IEnumerable{string}, string)"/>
        /// or one of its overrides and the user token cache contains multiple tokens for this client application and the the specified Account, but from different authorities.
        /// <para>Mitigation [App Development].</para>specify the authority to use in the acquire token operation.
        /// </summary>
        public const string MultipleTokensMatchedError = "multiple_matching_tokens_detected";

        /// <summary>
        /// Non HTTPS redirects are not supported.
        /// <para>What happens?.</para>This error happens when you have registered a non-https redirect URI for the
        /// public client application other than <c>urn:ietf:wg:oauth:2.0:oob</c>.
        /// <para>Mitigation [App registration and development].</para>Register in the application a Reply URL starting with "https://".
        /// </summary>
        public const string NonHttpsRedirectNotSupported = "non_https_redirect_failed";

        /// <summary>
        /// The request could not be preformed because the network is down.
        /// <para>Mitigation [App development].</para> In the application you could either inform the user that there are network issues
        /// or retry later.
        /// </summary>
        public const string NetworkNotAvailableError = "network_not_available";

        /// <summary>
        /// The B2C authority host is not the same as the one used when creating the client application.
        /// </summary>
        public const string B2CAuthorityHostMismatch = "B2C_authority_host_mismatch";

        /// <summary>
        /// Duplicate query parameter was found in extraQueryParameters.
        /// <para>What happens?.</para> You have used <c>extraQueryParameter</c> of overrides
        /// of token acquisition operations in public client and confidential client application and are passing a parameter which is already present in the
        /// URL (either because you had it in another way, or the library added it).
        /// <para>Mitigation [App Development].</para> RemoveAccount the duplicate parameter from the token acquisition override.
        /// </summary>
        public const string DuplicateQueryParameterError = "duplicate_query_parameter";

        /// <summary>
        /// The request could not be performed because of a failure in the UI flow.
        /// <para>What happens?.</para>The library failed to invoke the Web View required to perform interactive authentication.
        /// The exception might include the reason.
        /// <para>Mitigation.</para>If the exception includes the reason, you could inform the user. This might be, for instance, a browser
        /// implementing chrome tabs is missing on the Android phone (that's only an example: this exception can apply to other
        /// platforms as well).
        /// </summary>
        public const string AuthenticationUiFailedError = "authentication_ui_failed";

        /// <summary>
        /// Authentication canceled.
        /// <para>What happens?.</para>The user had canceled the authentication, for instance by closing the authentication dialog.
        /// <para>Mitigation.</para>None, you cannot get a token to call the protected API. You might want to inform the user.
        /// </summary>
        public const string AuthenticationCanceledError = "authentication_canceled";

        /// <summary>
        /// JSON parsing failed.
        /// <para>What happens?.</para>A Json blob read from the token cache or received from the STS was not parseable.
        /// This can happen when reading the token cache, or receiving an IDToken from the STS.
        /// <para>Mitigation.</para>Make sure that the token cache was not tampered.
        /// </summary>
        public const string JsonParseError = "json_parse_failed";

        /// <summary>
        /// JWT was invalid.
        /// <para>What happens?.</para>The library expected a JWT (for instance a token from the cache, or received from the STS), but
        /// the format is invalid.
        /// <para>Mitigation.</para>Make sure that the token cache was not tampered.
        /// </summary>
        public const string InvalidJwtError = "invalid_jwt";

        /// <summary>
        /// State returned from the STS was different from the one sent by the library.
        /// <para>What happens?.</para>The library sends to the STS a state associated to a request, and expects the reply to be consistent.
        /// This errors indicates that the reply is not associated with the request. This could indicate an attempt to replay a response.
        /// <para>Mitigation.</para> None.
        /// </summary>
        public const string StateMismatchError = "state_mismatch";

        /// <summary>
        /// Tenant discovery failed.
        /// <para>What happens?.</para>While reading the openid configuration associated with the authority, the Authorize endpoint,
        /// or Token endpoint, or the Issuer was not found.
        /// <para>Mitigation.</para>This indicates and authority which is not Open ID Connect compliant. Specify a different authority
        /// in the constructor of the application, or the token acquisition override
        /// ///. </summary>
        public const string TenantDiscoveryFailedError = "tenant_discovery_failed";

        /// <summary>
        /// The library is loaded on a platform which is not supported.
        /// </summary>
        public const string PlatformNotSupported = "platform_not_supported";

#if iOS
        /// <summary>
        /// Xamarin.iOS specific. This error indicates that keychain access has not be enabled for the application.
        /// From MSAL 2.x and ADAL 4.x, the keychain for the publisher needs to be accessed in order to provide
        /// Single Sign On between applications of the same publisher.
        /// <para>Mitigation.</para> In order to access the keychain on iOS, you will need to ensure the Entitlements.plist
        /// file is configured and included under &amp;lt;CodesignEntitlements&amp;gt;Entitlements.plist&amp;lt;/CodesignEntitlements&amp;gt;
        /// in the csproj file of the iOS app.
        /// </summary>
        public const string CannotAccessPublisherKeyChain = "cannot_access_publisher_keychain";

        /// <summary>
        /// Xamarin.iOS specific. This error indicates that saving a token to the keychain failed.
        /// <para>Mitigation.</para> In order to access the keychain on iOS, you will need to set the
        /// keychain access groups in the Entitlements.plist for the application.
        /// </summary>
        public const string MissingEntitlements = "missing_entitlements";
#endif

#if ANDROID

        /// <summary>
        /// Xamarin.Android specific. This error indicates that a system browser was not installed on the user's device, and authentication
        /// using system browser could not be attempted because there was no available Android activity to handle the intent.
        /// <para>Mitigation.</para>If you want to use the System web browser (for instance to get SSO with the browser), notify the end
        /// user that chrome or a browser implementing chrome custom tabs needs to be installed on the device.
        /// </summary>
        public const string AndroidActivityNotFound = "android_activity_not_found";

        /// <summary>
        /// The intent to launch AuthenticationActivity is not resolvable by the OS or the intent.
        /// </summary>
        public const string UnresolvableIntentError = "unresolvable_intent";

        /// <summary>
        /// Failed to create shared preferences on the Android platform.
        /// <para>What happens?.</para> The library uses Android shared preferences to store the token cache.
        /// <para>Mitigation.</para> Make sure the application is configured to use this platform feature.
        /// </summary>
        public const string FailedToCreateSharedPreference = "shared_preference_creation_failed";
#endif
    }
}
