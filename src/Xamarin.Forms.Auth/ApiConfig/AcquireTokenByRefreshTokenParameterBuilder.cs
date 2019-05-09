// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Parameter builder for the <see cref="IByRefreshToken.AcquireTokenByRefreshToken(IEnumerable{string}, string)"/>
    /// method.
    /// </summary>
    public sealed class AcquireTokenByRefreshTokenParameterBuilder :
        AbstractClientAppBaseAcquireTokenParameterBuilder<AcquireTokenByRefreshTokenParameterBuilder>
    {
        internal AcquireTokenByRefreshTokenParameterBuilder(IClientApplicationBaseExecutor clientApplicationBaseExecutor)
            : base(clientApplicationBaseExecutor)
        {
        }

        private AcquireTokenByRefreshTokenParameters Parameters { get; } = new AcquireTokenByRefreshTokenParameters();

        internal static AcquireTokenByRefreshTokenParameterBuilder Create(
            IClientApplicationBaseExecutor clientApplicationBaseExecutor,
            IEnumerable<string> scopes,
            string refreshToken)
        {
            return new AcquireTokenByRefreshTokenParameterBuilder(clientApplicationBaseExecutor)
                   .WithScopes(scopes).WithRefreshToken(refreshToken);
        }

        internal AcquireTokenByRefreshTokenParameterBuilder WithRefreshToken(string refreshToken)
        {
            Parameters.RefreshToken = refreshToken;
            return this;
        }

        /// <inheritdoc />
        internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
        {
            return ClientApplicationBaseExecutor.ExecuteAsync(CommonParameters, Parameters, cancellationToken);
        }
    }
}
