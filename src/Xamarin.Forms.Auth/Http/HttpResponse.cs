// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Linq;

namespace Xamarin.Forms.Auth
{
    internal class HttpResponse : IHttpWebResponse
    {
        public HttpResponseHeaders Headers { get; set; }

        public IDictionary<string, string> HeadersAsDictionary {
            get
            {
                var headers = new Dictionary<string, string>();

                if (Headers != null)
                {
                    foreach (var kvp in Headers)
                    {
                        headers[kvp.Key] = kvp.Value.First();
                    }
                }

                return headers;
            }
        }

        public HttpStatusCode StatusCode { get; set; }

        public string UserAgent { get; set; }

        public string Body { get; set; }

    }
}
