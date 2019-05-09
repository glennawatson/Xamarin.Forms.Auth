﻿// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Net.Http.Headers;

using Newtonsoft.Json.Linq;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Exception type thrown when service returns an error response or other networking errors occur.
    /// For more details, see https://aka.ms/msal-net-exceptions.
    /// </summary>
    [Serializable]
    public class AuthServiceException : AuthException
    {
        private const string ClaimsKey = "claims";
        private const string ResponseBodyKey = "response_body";
        private const string CorrelationIdKey = "correlation_id";
        private const string SubErrorKey = "sub_error";

        private HttpResponse _httpResponse;
        private OAuth2ResponseBase _oauth2ResponseBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServiceException"/> class with a specified.
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        public AuthServiceException(string errorCode, string errorMessage)
            : base(errorCode, errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServiceException"/> class with a specified.
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by the client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">Status code of the resposne received from the service.</param>
        public AuthServiceException(string errorCode, string errorMessage, int statusCode)
            : this(errorCode, errorMessage)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServiceException"/> class with a specified.
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by the client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public AuthServiceException(string errorCode, string errorMessage, Exception innerException)
            : base(errorCode, errorMessage, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServiceException"/> class with a specified.
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by the client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">HTTP status code of the resposne received from the service.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public AuthServiceException(string errorCode, string errorMessage, int statusCode, Exception innerException)
            : base(errorCode, errorMessage, innerException)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServiceException"/> class with a specified.
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by the client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">The status code of the request.</param>
        /// <param name="claims">The claims challenge returned back from the service.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public AuthServiceException(
            string errorCode,
            string errorMessage,
            int statusCode,
            string claims,
            Exception innerException)
            : this(errorCode, errorMessage, statusCode, innerException)
        {
            Claims = claims;
        }

        /// <summary>
        /// Gets the status code returned from http layer. This status code is either the <c>HttpStatusCode</c> in the inner
        /// <see cref="System.Net.Http.HttpRequestException"/> response or the the NavigateError Event Status Code in a browser based flow (See
        /// http://msdn.microsoft.com/en-us/library/bb268233(v=vs.85).aspx).
        /// You can use this code for purposes such as implementing retry logic or error investigation.
        /// </summary>
        public int StatusCode { get; internal set; }

        /// <summary>
        /// Gets the Additional claims requested by the service. When this property is not null or empty, this means that the service requires the user to
        /// provide additional claims, such as doing two factor authentication.
        /// </summary>
        public string Claims { get; internal set; }

        /// <summary>
        /// Gets the raw response body received from the server.
        /// </summary>
        public string ResponseBody { get; internal set; }

        /// <summary>
        /// Gets the http headers from the server response that indicated an error.
        /// </summary>
        /// <remarks>
        /// When the server returns a 429 Too Many Requests error, a Retry-After should be set. It is important to read and respect the
        /// time specified in the Retry-After header to avoid a retry storm.
        /// </remarks>
        public HttpResponseHeaders Headers { get; internal set; }

        /// <summary>
        /// Gets an ID that can used to piece up a single authentication flow.
        /// </summary>
        public string CorrelationId { get; internal set; }

        internal HttpResponse HttpResponse
        {
            get => _httpResponse;
            set
            {
                _httpResponse = value;
                ResponseBody = _httpResponse?.Body;
                StatusCode = _httpResponse != null ? (int)_httpResponse.StatusCode : 0;
                Headers = _httpResponse?.Headers;

                // In most cases we can deserialize the body to get more details such as the suberror
                OAuth2Response = JsonHelper.TryToDeserializeFromJson<OAuth2ResponseBase>(_httpResponse?.Body);
            }
        }

        internal OAuth2ResponseBase OAuth2Response
        {
            get => _oauth2ResponseBase;
            set
            {
                _oauth2ResponseBase = value;
                Claims = _oauth2ResponseBase?.Claims;
                CorrelationId = _oauth2ResponseBase?.CorrelationId;
                SubError = _oauth2ResponseBase?.SubError;
            }
        }

        /// <summary>
        /// Gets or sets the suberror should not be exposed for public consumption yet, as STS needs to do some work
        /// first.
        /// </summary>
        internal string SubError { get; set; }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(
                CultureInfo.InvariantCulture,
                "\n\tStatusCode: {0} \n\tResponseBody: {1} \n\tHeaders: {2}",
                StatusCode,
                ResponseBody,
                Headers);
        }

        internal override void PopulateJson(JObject jobj)
        {
            base.PopulateJson(jobj);

            jobj[ClaimsKey] = Claims;
            jobj[ResponseBodyKey] = ResponseBody;
            jobj[CorrelationIdKey] = CorrelationId;
            jobj[SubErrorKey] = SubError;
        }

        internal override void PopulateObjectFromJson(JObject jobj)
        {
            base.PopulateObjectFromJson(jobj);

            Claims = JsonUtils.GetExistingOrEmptyString(jobj, ClaimsKey);
            ResponseBody = JsonUtils.GetExistingOrEmptyString(jobj, ResponseBodyKey);
            CorrelationId = JsonUtils.GetExistingOrEmptyString(jobj, CorrelationIdKey);
            SubError = JsonUtils.GetExistingOrEmptyString(jobj, SubErrorKey);
        }
    }
}
