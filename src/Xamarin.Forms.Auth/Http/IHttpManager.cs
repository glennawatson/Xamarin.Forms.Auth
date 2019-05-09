// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Manages a abstraction of http requests.
    /// </summary>
    internal interface IHttpManager
    {
        /// <summary>
        /// Sends a post request to the endpoint.
        /// </summary>
        /// <param name="endpoint">The end point where to send the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="bodyParameters">The body parameters of the request.</param>
        /// <param name="requestContext">The context of the request.</param>
        /// <returns>The response from the request.</returns>
        Task<HttpResponse> SendPostAsync(
            Uri endpoint,
            IDictionary<string, string> headers,
            IDictionary<string, string> bodyParameters,
            RequestContext requestContext);

        /// <summary>
        /// Sends a post request to the endpoint.
        /// </summary>
        /// <param name="endpoint">The end point where to send the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="body">The body of the request.</param>
        /// <param name="requestContext">The context of the request.</param>
        /// <returns>The response from the request.</returns>
        Task<HttpResponse> SendPostAsync(
            Uri endpoint,
            IDictionary<string, string> headers,
            HttpContent body,
            RequestContext requestContext);

        /// <summary>
        /// Sends a get request to the endpoint.
        /// </summary>
        /// <param name="endpoint">The end point where to send the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="requestContext">The context of the request.</param>
        /// <returns>The response from the request.</returns>
        Task<HttpResponse> SendGetAsync(
            Uri endpoint,
            IDictionary<string, string> headers,
            RequestContext requestContext);

        /// <summary>
        /// Sends a post request to the endpoint forcing a response.
        /// </summary>
        /// <param name="uri">The end point where to send the request.</param>
        /// <param name="headers">The headers for the request.</param>
        /// <param name="body">The body of the request.</param>
        /// <param name="requestContext">The context of the request.</param>
        /// <returns>The response from the request.</returns>
        Task<HttpResponse> SendPostForceResponseAsync(
            Uri uri,
            Dictionary<string, string> headers,
            StringContent body,
            RequestContext requestContext);
    }
}
