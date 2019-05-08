// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Net.Http.Headers;

namespace Xamarin.Forms.Auth
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClientFactory()
        {
            var httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true })
            {
                MaxResponseContentBufferSize = MaxResponseContentBufferSizeInBytes
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpClient = httpClient;
        }

        public long MaxResponseContentBufferSizeInBytes { get; } = 1024 * 1024;

        public HttpClient HttpClient { get; }
    }
}
