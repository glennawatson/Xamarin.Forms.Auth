// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Http.Headers;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A response from a HTTP request.
    /// </summary>
    internal interface IHttpWebResponse
    {
        /// <summary>
        /// Gets the status code from the request.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the headers sent in the response.
        /// </summary>
        HttpResponseHeaders Headers { get; }

        /// <summary>
        /// Gets the body of the response.
        /// </summary>
        string Body { get; }
    }
}
