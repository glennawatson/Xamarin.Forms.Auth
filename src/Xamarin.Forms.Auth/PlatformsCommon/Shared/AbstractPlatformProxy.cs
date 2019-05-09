// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal abstract class AbstractPlatformProxy : IPlatformProxy
    {
        private readonly Lazy<string> _callingApplicationName;
        private readonly Lazy<string> _callingApplicationVersion;
        private readonly Lazy<ICryptographyManager> _cryptographyManager;
        private readonly Lazy<string> _deviceId;
        private readonly Lazy<string> _deviceModel;
        private readonly Lazy<string> _operatingSystem;
        private readonly Lazy<IPlatformLogger> _platformLogger;
        private readonly Lazy<string> _processorArchitecture;
        private readonly Lazy<string> _productName;
        private readonly Lazy<ITokenCache> _tokenCache;

        protected AbstractPlatformProxy(ICoreLogger logger)
        {
            Logger = logger;
            _deviceModel = new Lazy<string>(InternalGetDeviceModel);
            _operatingSystem = new Lazy<string>(InternalGetOperatingSystem);
            _processorArchitecture = new Lazy<string>(InternalGetProcessorArchitecture);
            _callingApplicationName = new Lazy<string>(InternalGetCallingApplicationName);
            _callingApplicationVersion = new Lazy<string>(InternalGetCallingApplicationVersion);
            _deviceId = new Lazy<string>(InternalGetDeviceId);
            _productName = new Lazy<string>(InternalGetProductName);
            _cryptographyManager = new Lazy<ICryptographyManager>(InternalGetCryptographyManager);
            _platformLogger = new Lazy<IPlatformLogger>(InternalGetPlatformLogger);
            _tokenCache = new Lazy<ITokenCache>(InternalGetTokenCache);
        }

        /// <inheritdoc />
        public abstract bool IsSystemWebViewAvailable { get; }

        /// <inheritdoc />
        public ICryptographyManager CryptographyManager => _cryptographyManager.Value;

        /// <inheritdoc />
        public IPlatformLogger PlatformLogger => _platformLogger.Value;

        /// <inheritdoc />
        public ITokenCache TokenCache => _tokenCache.Value;

        protected IWebUIFactory OverloadWebUiFactory { get; set; }

        protected IFeatureFlags OverloadFeatureFlags { get; set; }

        protected ICoreLogger Logger { get; }

        /// <inheritdoc />
        public virtual IFeatureFlags GetFeatureFlags()
        {
            return OverloadFeatureFlags ?? CreateFeatureFlags();
        }

        /// <inheritdoc />
        public void SetFeatureFlags(IFeatureFlags featureFlags)
        {
            OverloadFeatureFlags = featureFlags;
        }

        /// <inheritdoc />
        public IWebUIFactory GetWebUiFactory()
        {
            return OverloadWebUiFactory ?? CreateWebUiFactory();
        }

        /// <inheritdoc />
        public void SetWebUiFactory(IWebUIFactory webUiFactory)
        {
            OverloadWebUiFactory = webUiFactory;
        }

        /// <inheritdoc />
        public string GetDeviceModel()
        {
            return _deviceModel.Value;
        }

        /// <inheritdoc />
        public abstract string GetEnvironmentVariable(string variable);

        /// <inheritdoc />
        public string GetOperatingSystem()
        {
            return _operatingSystem.Value;
        }

        /// <inheritdoc />
        public string GetProcessorArchitecture()
        {
            return _processorArchitecture.Value;
        }

        /// <inheritdoc />
        public string GetCallingApplicationName()
        {
            return _callingApplicationName.Value;
        }

        /// <inheritdoc />
        public string GetCallingApplicationVersion()
        {
            return _callingApplicationVersion.Value;
        }

        /// <inheritdoc />
        public string GetDeviceId()
        {
            return _deviceId.Value;
        }

        /// <inheritdoc />
        public string GetProductName()
        {
            return _productName.Value;
        }

        protected abstract IWebUIFactory CreateWebUiFactory();

        protected abstract IFeatureFlags CreateFeatureFlags();

        protected abstract string InternalGetDeviceModel();

        protected abstract string InternalGetOperatingSystem();

        protected abstract string InternalGetProcessorArchitecture();

        protected abstract string InternalGetCallingApplicationName();

        protected abstract string InternalGetCallingApplicationVersion();

        protected abstract string InternalGetDeviceId();

        protected abstract string InternalGetProductName();

        protected abstract ICryptographyManager InternalGetCryptographyManager();

        protected abstract IPlatformLogger InternalGetPlatformLogger();

        protected abstract ITokenCache InternalGetTokenCache();
    }
}
