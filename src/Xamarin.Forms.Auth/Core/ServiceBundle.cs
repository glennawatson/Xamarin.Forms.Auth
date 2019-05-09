// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class ServiceBundle : IServiceBundle
    {
        internal ServiceBundle(
            ApplicationConfiguration config)
        {
            Config = config;

            DefaultLogger = new AuthLogger(
                config.LogLevel,
                config.EnablePiiLogging,
                config.IsDefaultPlatformLoggingEnabled,
                config.LoggingCallback);

            PlatformProxy = PlatformProxyFactory.CreatePlatformProxy(DefaultLogger);
            HttpManager = config.HttpManager ?? new HttpManager(config.HttpClientFactory);
        }

        public ICoreLogger DefaultLogger { get; }

        /// <inheritdoc />
        public IHttpManager HttpManager { get; }

        /// <inheritdoc />
        public IPlatformProxy PlatformProxy { get; }

        /// <inheritdoc />
        public IApplicationConfiguration Config { get; }

        public static ServiceBundle Create(ApplicationConfiguration config)
        {
            return new ServiceBundle(config);
        }
    }
}
