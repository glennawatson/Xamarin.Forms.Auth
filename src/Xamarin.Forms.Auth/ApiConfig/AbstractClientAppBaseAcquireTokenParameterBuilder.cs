// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Base class for parameter builders common to public client application and confidential
    /// client application token acquisition operations.
    /// </summary>
    /// <typeparam name="T">The type of builder.</typeparam>
    public abstract class AbstractClientAppBaseAcquireTokenParameterBuilder<T> : AbstractAcquireTokenParameterBuilder<T>
        where T : AbstractAcquireTokenParameterBuilder<T>
    {
        internal AbstractClientAppBaseAcquireTokenParameterBuilder(IClientApplicationBaseExecutor clientApplicationBaseExecutor)
        {
            ClientApplicationBaseExecutor = clientApplicationBaseExecutor;
        }

        internal IClientApplicationBaseExecutor ClientApplicationBaseExecutor { get; }

        /// <inheritdoc />
        public override Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return ExecuteInternalAsync(cancellationToken);
        }

        internal abstract Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken);
    }
}
