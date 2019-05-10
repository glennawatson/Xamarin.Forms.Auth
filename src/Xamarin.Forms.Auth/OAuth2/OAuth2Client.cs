// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
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

        public static T CreateResponse<T>(HttpResponse response, RequestContext requestContext)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                CreateErrorResponse(response, requestContext);
            }

            return JsonHelper.DeserializeFromJson<T>(response.Body);
        }

        public static void CreateErrorResponse(HttpResponse response, RequestContext requestContext)
        {
            bool shouldLogAsError = true;

            var httpErrorCodeMessage = string.Format(CultureInfo.InvariantCulture, "HttpStatusCode: {0}: {1}", (int)response.StatusCode, response.StatusCode.ToString());
            requestContext.Logger.Info(httpErrorCodeMessage);

            Exception exceptionToThrow = null;

            try
            {
                exceptionToThrow = ExtractErrorsFromTheResponse(response, ref shouldLogAsError);
            }
            catch (JsonException)
            {
                exceptionToThrow = new AuthServiceException(
                    AuthError.NonParsableOAuthError,
                    AuthErrorMessage.NonParsableOAuthError)
                {
                    HttpResponse = response
                };
            }
            catch (Exception ex)
            {
                exceptionToThrow = new AuthServiceException(AuthError.UnknownError, response.Body, ex)
                {
                    HttpResponse = response
                };
            }

            if (exceptionToThrow == null)
            {
                exceptionToThrow = new AuthServiceException(
                    response.StatusCode == HttpStatusCode.NotFound
                        ? AuthError.HttpStatusNotFound
                        : AuthError.HttpStatusCodeNotOk, httpErrorCodeMessage)
                {
                    HttpResponse = response
                };
            }

            if (shouldLogAsError)
            {
                requestContext.Logger.ErrorPii(exceptionToThrow);
            }
            else
            {
                requestContext.Logger.InfoPii(exceptionToThrow);
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

        public async Task<OAuth2TokenResponse> GetTokenAsync(Uri endPoint, RequestContext requestContext)
        {
            return await ExecuteRequestAsync<OAuth2TokenResponse>(endPoint, HttpMethod.Post, requestContext).ConfigureAwait(false);
        }

        internal async Task<T> ExecuteRequestAsync<T>(Uri endPoint, HttpMethod method, RequestContext requestContext)
        {
            HttpResponse response = null;
            Uri endpointUri = CreateFullEndpointUri(endPoint);

            if (method == HttpMethod.Post)
            {
                response = await _httpManager.SendPostAsync(endpointUri, _headers, _bodyParameters, requestContext)
                                            .ConfigureAwait(false);
            }
            else
            {
                response = await _httpManager.SendGetAsync(endpointUri, _headers, requestContext).ConfigureAwait(false);
            }

            return CreateResponse<T>(response, requestContext);
        }

        private static Exception ExtractErrorsFromTheResponse(HttpResponse response, ref bool shouldLogAsError)
        {
            Exception exceptionToThrow = null;

            // In cases where the end-point is not found (404) response.body will be empty.
            if (string.IsNullOrWhiteSpace(response.Body))
            {
                return null;
            }

            var authTokenResponse = JsonHelper.DeserializeFromJson<OAuth2TokenResponse>(response.Body);

            if (authTokenResponse?.Error == null)
            {
                return null;
            }

            if (AuthError.InvalidGrantError.Equals(authTokenResponse.Error, StringComparison.OrdinalIgnoreCase))
            {
                exceptionToThrow = new AuthUiRequiredException(
                    AuthError.InvalidGrantError,
                    authTokenResponse.ErrorDescription)
                {
                    HttpResponse = response
                };
            }
            else
            {
                exceptionToThrow = new AuthServiceException(
                    authTokenResponse.Error,
                    authTokenResponse.ErrorDescription)
                {
                    HttpResponse = response
                };
            }

            // For device code flow, AuthorizationPending can occur a lot while waiting
            // for the user to auth via browser and this causes a lot of error noise in the logs.
            // So suppress this particular case to an Info so we still see the data but don't
            // log it as an error since it's expected behavior while waiting for the user.
            if (string.Compare(authTokenResponse.Error, OAuth2Error.AuthorizationPending, StringComparison.OrdinalIgnoreCase) == 0)
            {
                shouldLogAsError = false;
            }

            return exceptionToThrow;
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
