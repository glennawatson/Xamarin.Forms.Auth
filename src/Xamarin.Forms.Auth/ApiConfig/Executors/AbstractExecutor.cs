// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal abstract class AbstractExecutor
    {
        private readonly ClientApplicationBase _clientApplicationBase;

        protected AbstractExecutor(IServiceBundle serviceBundle, ClientApplicationBase clientApplicationBase)
        {
            ServiceBundle = serviceBundle;
            _clientApplicationBase = clientApplicationBase;
        }

        protected IServiceBundle ServiceBundle { get; }

        protected RequestContext CreateRequestContextAndLogVersionInfo()
        {
            var requestContext = new RequestContext(
                _clientApplicationBase.AppConfig.ClientId,
                AuthLogger.Create(ServiceBundle.Config));

            var logValue = $"AUTH '{ServiceBundle.PlatformProxy.GetProductName()}', file version '{AssemblyUtils.GetAssemblyFileVersionAttribute()}', informational version '{AssemblyUtils.GetAssemblyInformationalVersion()}'";
            requestContext.Logger.Info(logValue);

            return requestContext;
        }
    }
}
