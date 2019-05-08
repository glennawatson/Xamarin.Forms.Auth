// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    internal class OAuth2Client
    {
        private readonly Dictionary<string, string> _bodyParameters = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queryParameters = new Dictionary<string, string>();
        private readonly IHttpManager _httpManager;

        public OAuth2Client(IHttpManager httpManager)
        {
            _httpManager = httpManager ?? throw new ArgumentNullException(nameof(httpManager));
        }

        public static T CreateResponse<T>(HttpResponse response, RequestContext requestContext, bool addCorrelationId)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                CreateErrorResponse(response, requestContext);
            }

            if (addCorrelationId)
            {
                VerifyCorrelationIdHeaderInResponse(response.HeadersAsDictionary, requestContext);
            }

            return JsonConvert.DeserializeObject<T>(response.Body);
        }

        public static void CreateErrorResponse(HttpResponse response, RequestContext requestContext)
        {
            bool shouldLogAsError = true;

            var httpErrorCodeMessage = string.Format(CultureInfo.InvariantCulture, "HttpStatusCode: {0}: {1}", (int)response.StatusCode, response.StatusCode.ToString());
            requestContext.Logger.Info(httpErrorCodeMessage);

            Exception exceptionToThrow;

            try
            {
                exceptionToThrow = ExtractErrorsFromTheResponse(response, ref shouldLogAsError);
            }
            catch (JsonException)
            {
                // in the rare case we get an error response we cannot deserialize
                exceptionToThrow = ExceptionFactory.GetServiceException(
                    CoreErrorCodes.NonParsableOAuthError,
                    CoreErrorMessages.NonParsableOAuthError,
                    response);
            }
            catch (Exception ex)
            {
                exceptionToThrow = ExceptionFactory.GetServiceException(CoreErrorCodes.UnknownError, response.Body, ex, response);
            }

            if (exceptionToThrow == null)
            {
                exceptionToThrow = response.StatusCode != HttpStatusCode.NotFound ?
                    ExceptionFactory.GetServiceException(CoreErrorCodes.HttpStatusCodeNotOk, httpErrorCodeMessage, response) :
                    ExceptionFactory.GetServiceException(CoreErrorCodes.HttpStatusNotFound, httpErrorCodeMessage, response);
            }

            if (shouldLogAsError)
            {
                requestContext.Logger.Error("Cannot get a response from the server.");
            }
            else
            {
                requestContext.Logger.Info("Cannot get a response from the server.");
            }

            throw exceptionToThrow;
        }

        public void AddQueryParameter(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                _queryParameters[key] = value;
            }
        }

        public void AddBodyParameter(string key, string value)
        {
            _bodyParameters[key] = value;
        }

        public async Task<OAuth2TokenResponse> GetTokenAsync(Uri endPoint, RequestContext requestContext, CancellationToken token)
        {
            return await ExecuteRequestAsync<OAuth2TokenResponse>(endPoint, HttpMethod.Post, requestContext, token).ConfigureAwait(false);
        }

        internal async Task<T> ExecuteRequestAsync<T>(Uri endPoint, HttpMethod method, RequestContext requestContext, CancellationToken token)
        {
            bool addCorrelationId =
                requestContext != null && !string.IsNullOrEmpty(requestContext.Logger.CorrelationId.ToString());
            if (addCorrelationId)
            {
                _headers.Add(OAuth2Header.CorrelationId, requestContext.Logger.CorrelationId.ToString());
                _headers.Add(OAuth2Header.RequestCorrelationIdInResponse, "true");
            }

            var endpointUri = CreateFullEndpointUri(endPoint);

            HttpResponse response;
            if (method == HttpMethod.Post)
            {
                response = await _httpManager.SendPostAsync(endpointUri, _headers, _bodyParameters, requestContext, token)
                                            .ConfigureAwait(false);
            }
            else
            {
                response = await _httpManager.SendGetAsync(endpointUri, _headers, requestContext, token).ConfigureAwait(false);
            }

            return CreateResponse<T>(response, requestContext, addCorrelationId);
        }

        private static Exception ExtractErrorsFromTheResponse(HttpResponse response, ref bool shouldLogAsError)
        {
            // In cases where the end-point is not found (404) response.body will be empty.
            if (string.IsNullOrWhiteSpace(response.Body))
            {
                return null;
            }

            var tokenResponse = JsonConvert.DeserializeObject<OAuth2TokenResponse>(response.Body);

            if (tokenResponse?.Error == null)
            {
                return null;
            }

            Exception exceptionToThrow;
            if (CoreErrorCodes.InvalidGrantError.Equals(tokenResponse.Error, StringComparison.OrdinalIgnoreCase))
            {
                exceptionToThrow = ExceptionFactory.GetUiRequiredException(
                    CoreErrorCodes.InvalidGrantError,
                    tokenResponse.ErrorDescription,
                    null,
                    ExceptionDetail.FromHttpResponse(response));
            }
            else
            {
                exceptionToThrow = ExceptionFactory.GetServiceException(
                    tokenResponse.Error,
                    tokenResponse.ErrorDescription,
                    response);
            }

            // For device code flow, AuthorizationPending can occur a lot while waiting
            // for the user to auth via browser and this causes a lot of error noise in the logs.
            // So suppress this particular case to an Info so we still see the data but don't
            // log it as an error since it's expected behavior while waiting for the user.
            if (string.Compare(tokenResponse.Error, OAuth2Error.AuthorizationPending, StringComparison.OrdinalIgnoreCase) == 0)
            {
                shouldLogAsError = false;
            }

            return exceptionToThrow;
        }

        private static void VerifyCorrelationIdHeaderInResponse(
            IDictionary<string, string> headers,
            RequestContext requestContext)
        {
            foreach (string responseHeaderKey in headers.Keys)
            {
                string trimmedKey = responseHeaderKey.Trim();
                if (string.Compare(trimmedKey, OAuth2Header.CorrelationId, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    string correlationIdHeader = headers[trimmedKey].Trim();
                    if (string.Compare(
                            correlationIdHeader,
                            requestContext.Logger.CorrelationId.ToString(),
                            StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        requestContext.Logger.Warning("Returned correlation id does not match the sent correlation id");
                    }

                    break;
                }
            }
        }

        private Uri CreateFullEndpointUri(Uri endPoint)
        {
            var endpointUri = new UriBuilder(endPoint);
            string extraQp = _queryParameters.ToQueryParameter();
            endpointUri.AppendQueryParameters(extraQp);

            return endpointUri.Uri;
        }
    }
}
