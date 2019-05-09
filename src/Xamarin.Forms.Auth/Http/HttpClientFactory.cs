// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
#if iOS
using Foundation;
using UIKit;
#endif

namespace Xamarin.Forms.Auth
{
    internal class HttpClientFactory : IAuthHttpClientFactory
    {
#if !ANDROID
        // The HttpClient is a singleton per ClientApplication so that we don't have a process wide singleton.
        public const long MaxResponseContentBufferSizeInBytes = 1024 * 1024;
#endif

        private readonly HttpClient _httpClient;

        public HttpClientFactory()
        {
#if iOS
            // See https://aka.ms/msal-net-httpclient for details
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                _httpClient = new HttpClient(new NSUrlSessionHandler())
                {
                    MaxResponseContentBufferSize = MaxResponseContentBufferSizeInBytes
                };
            }
#elif ANDROID
            // See https://aka.ms/msal-net-httpclient for details
            _httpClient = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler());
#else
            _httpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true })
            {
                MaxResponseContentBufferSize = MaxResponseContentBufferSizeInBytes
            };
#endif
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _httpClient?.Dispose();
            }
        }
    }
}
