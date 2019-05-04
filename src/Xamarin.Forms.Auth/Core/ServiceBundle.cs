// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.PlatformsCommon.Factories;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.TelemetryCore;
using Microsoft.Identity.Client.WsTrust;

namespace Xamarin.Forms.Auth
{
    internal class ServiceBundle : IServiceBundle
    {
        internal ServiceBundle(
            IHttpClientFactory httpClientFactory = null,
            IHttpManager httpManager = null,
            ITelemetryReceiver telemetryReceiver = null,
            IValidatedAuthoritiesCache validatedAuthoritiesCache = null,
            IAadInstanceDiscovery aadInstanceDiscovery = null,
            IWsTrustWebRequestManager wsTrustWebRequestManager = null,
            bool shouldClearCaches = false)
        {
            HttpManager = httpManager ?? new HttpManager(httpClientFactory);
            TelemetryManager = new TelemetryManager(telemetryReceiver ?? Telemetry.GetInstance());
            ValidatedAuthoritiesCache = validatedAuthoritiesCache ?? new ValidatedAuthoritiesCache(shouldClearCaches);
            AadInstanceDiscovery = aadInstanceDiscovery ?? new AadInstanceDiscovery(HttpManager, TelemetryManager, shouldClearCaches);
            WsTrustWebRequestManager = wsTrustWebRequestManager ?? new WsTrustWebRequestManager(HttpManager);
            PlatformProxy = PlatformProxyFactory.GetPlatformProxy();
        }

        /// <inheritdoc />
        public IHttpManager HttpManager { get; }

        /// <inheritdoc />
        /// <inheritdoc />
        public IValidatedAuthoritiesCache ValidatedAuthoritiesCache { get; }

        /// <inheritdoc />
        public IAadInstanceDiscovery AadInstanceDiscovery { get; }

        /// <inheritdoc />
        public IWsTrustWebRequestManager WsTrustWebRequestManager { get; }

        /// <inheritdoc />
        public IPlatformProxy PlatformProxy { get; }

        public static ServiceBundle CreateDefault(ITelemetryReceiver telemetryReceiver = null)
        {
            return new ServiceBundle(telemetryReceiver: telemetryReceiver);
        }

        public static ServiceBundle CreateWithCustomHttpManager(IHttpManager httpManager, ITelemetryReceiver telemetryReceiver = null)
        {
            return new ServiceBundle(httpManager: httpManager, telemetryReceiver: telemetryReceiver, shouldClearCaches: true);
        }
    }
}
