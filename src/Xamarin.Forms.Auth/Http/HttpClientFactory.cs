// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Net.Http.Headers;

namespace Xamarin.Forms.Auth
{
    internal interface IHttpClientFactory
    {
        HttpClient HttpClient { get; }
    }

    internal class HttpClientFactory : IHttpClientFactory
    {
        // The HttpClient is a singleton per ClientApplication so that we don't have a process wide singleton.
        public const long MaxResponseContentBufferSizeInBytes = 1024*1024;

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

        public HttpClient HttpClient { get; }
    }
}
