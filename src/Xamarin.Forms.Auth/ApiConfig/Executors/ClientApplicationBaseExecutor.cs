// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    internal class ClientApplicationBaseExecutor : AbstractExecutor, IClientApplicationBaseExecutor
    {
        private readonly ClientApplicationBase _clientApplicationBase;

        public ClientApplicationBaseExecutor(IServiceBundle serviceBundle, ClientApplicationBase clientApplicationBase)
            : base(serviceBundle, clientApplicationBase)
        {
            _clientApplicationBase = clientApplicationBase;
        }

        public async Task<AuthenticationResult> ExecuteAsync(
            AcquireTokenCommonParameters commonParameters,
            AcquireTokenSilentParameters silentParameters,
            CancellationToken cancellationToken)
        {
            var requestContext = CreateRequestContextAndLogVersionInfo();

            var requestParameters = _clientApplicationBase.CreateRequestParameters(
                commonParameters,
                requestContext);

            var handler = new SilentRequest(ServiceBundle, requestParameters, silentParameters);
            return await handler.RunAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<AuthenticationResult> ExecuteAsync(
            AcquireTokenCommonParameters commonParameters,
            AcquireTokenByRefreshTokenParameters refreshTokenParameters,
            CancellationToken cancellationToken)
        {
            var requestContext = CreateRequestContextAndLogVersionInfo();
            if (commonParameters.Scopes == null || !commonParameters.Scopes.Any())
            {
                commonParameters.Scopes = new SortedSet<string>
                {
                    _clientApplicationBase.AppConfig.ClientId + "/.default"
                };
                requestContext.Logger.Info(LogMessages.NoScopesProvidedForRefreshTokenRequest);
            }

            var requestParameters = _clientApplicationBase.CreateRequestParameters(
                commonParameters,
                requestContext);

            requestParameters.IsRefreshTokenRequest = true;

            requestContext.Logger.Info(LogMessages.UsingXScopesForRefreshTokenRequest(commonParameters.Scopes.Count()));

            var handler = new ByRefreshTokenRequest(ServiceBundle, requestParameters, refreshTokenParameters);
            return await handler.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
