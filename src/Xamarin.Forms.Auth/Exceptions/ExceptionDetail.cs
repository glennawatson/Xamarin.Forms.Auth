// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net.Http.Headers;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains details about an exception.
    /// </summary>
    internal class ExceptionDetail
    {
        /// <summary>
        /// Gets or sets the status code returned from http layer. This status code is either the HttpStatusCode in the inner
        /// HttpRequestException response or
        /// NavigateError Event Status Code in browser based flow (See
        /// http://msdn.microsoft.com/en-us/library/bb268233(v=vs.85).aspx).
        /// You can use this code for purposes such as implementing retry logic or error investigation.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the specific error codes that may be returned by the service.
        /// </summary>
        public string[] ServiceErrorCodes { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        public string Claims { get; set; }

        /// <summary>
        /// Gets or sets the raw response body received from the server.
        /// </summary>
        public string ResponseBody { get; set; }

        /// <summary>
        /// Gets or sets the http response headers.
        /// </summary>
        public HttpResponseHeaders HttpResponseHeaders { get; set; }

        /// <summary>
        /// Converts into exception details from a web response.
        /// </summary>
        /// <param name="response">The response to convert.</param>
        /// <returns>The converted response.</returns>
        public static ExceptionDetail FromHttpResponse(IHttpWebResponse response)
        {
            return new ExceptionDetail()
            {
                ResponseBody = response?.Body,
                StatusCode = response != null ? (int)response.StatusCode : -1,
                HttpResponseHeaders = response?.Headers
            };
        }
    }
}
