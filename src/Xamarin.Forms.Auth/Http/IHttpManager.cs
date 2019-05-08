// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A manager responsible for providing HTTP functionality.
    /// </summary>
    internal interface IHttpManager
    {
        /// <summary>
        /// Sends the message request using POST.
        /// </summary>
        /// <param name="endpoint">The end point where to end the message.</param>
        /// <param name="headers">The headers to use.</param>
        /// <param name="bodyParameters">Parameters for the body.</param>
        /// <param name="requestContext">The context with information about the request.</param>
        /// <param name="token">A cancellation token for cancelling a request.</param>
        /// <returns>The response.</returns>
        Task<HttpResponse> SendPostAsync(
            Uri endpoint,
            IDictionary<string, string> headers,
            IDictionary<string, string> bodyParameters,
            RequestContext requestContext,
            CancellationToken token);

        /// <summary>
        /// Sends the message request using POST.
        /// </summary>
        /// <param name="endpoint">The end point where to end the message.</param>
        /// <param name="headers">The headers to use.</param>
        /// <param name="body">The body to send..</param>
        /// <param name="requestContext">The context with information about the request.</param>
        /// <param name="token">A cancellation token for cancelling a request.</param>
        /// <returns>The response.</returns>
        Task<HttpResponse> SendPostAsync(
            Uri endpoint,
            IDictionary<string, string> headers,
            HttpContent body,
            RequestContext requestContext,
            CancellationToken token);

        /// <summary>
        /// Sends the message request using GET.
        /// </summary>
        /// <param name="endpoint">The end point where to end the message.</param>
        /// <param name="headers">The headers to use.</param>
        /// <param name="requestContext">The context with information about the request.</param>
        /// <param name="token">A cancellation token for cancelling a request.</param>
        /// <returns>The response.</returns>
        Task<HttpResponse> SendGetAsync(
            Uri endpoint,
            Dictionary<string, string> headers,
            RequestContext requestContext,
            CancellationToken token);

        /// <summary>
        /// Sends the message request using POST which requires a response.
        /// </summary>
        /// <param name="uri">The end point where to end the message.</param>
        /// <param name="headers">The headers to use.</param>
        /// <param name="body">The body to send..</param>
        /// <param name="requestContext">The context with information about the request.</param>
        /// <param name="token">A cancellation token for cancelling a request.</param>
        /// <returns>The response.</returns>
        Task<IHttpWebResponse> SendPostForceResponseAsync(
            Uri uri,
            Dictionary<string, string> headers,
            StringContent body,
            RequestContext requestContext,
            CancellationToken token);
    }
}
