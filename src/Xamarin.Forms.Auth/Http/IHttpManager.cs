// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;

namespace Xamarin.Forms.Auth
{
    internal interface IHttpManager
    {
        Task<HttpResponse> SendPostAsync(
            Uri endpoint,
            IDictionary<string, string> headers,
            IDictionary<string, string> bodyParameters,
            RequestContext requestContext);

        Task<HttpResponse> SendPostAsync(
            Uri endpoint,
            IDictionary<string, string> headers,
            HttpContent body,
            RequestContext requestContext);

        Task<HttpResponse> SendGetAsync(
            Uri endpoint,
            Dictionary<string, string> headers,
            RequestContext requestContext);

        Task<IHttpWebResponse> SendPostForceResponseAsync(
            Uri uri,
            Dictionary<string, string> headers,
            StringContent body,
            RequestContext requestContext);
    }
}
