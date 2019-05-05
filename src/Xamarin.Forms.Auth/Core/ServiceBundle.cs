// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class ServiceBundle : IServiceBundle
    {
        internal ServiceBundle(
            IHttpClientFactory httpClientFactory = null,
            IHttpManager httpManager = null)
        {
            HttpManager = httpManager ?? new HttpManager(httpClientFactory);
            PlatformProxy = PlatformProxyFactory.GetPlatformProxy();
        }

        /// <inheritdoc />
        public IHttpManager HttpManager { get; }

        /// <inheritdoc />
        public IPlatformProxy PlatformProxy { get; }

        public static ServiceBundle CreateDefault()
        {
            return new ServiceBundle();
        }

        public static ServiceBundle CreateWithCustomHttpManager(IHttpManager httpManager)
        {
            return new ServiceBundle(httpManager: httpManager);
        }
    }
}
