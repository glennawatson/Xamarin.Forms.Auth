// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Base class for public client application token request builders.
    /// </summary>
    /// <typeparam name="T">The type of builder item.</typeparam>
    public abstract class AbstractPublicClientAcquireTokenParameterBuilder<T>
        : AbstractAcquireTokenParameterBuilder<T>
        where T : AbstractAcquireTokenParameterBuilder<T>
    {
        internal AbstractPublicClientAcquireTokenParameterBuilder(IPublicClientApplicationExecutor publicClientApplicationExecutor)
        {
            PublicClientApplicationExecutor = publicClientApplicationExecutor;
        }

        /// <summary>
        /// Gets the public client application executor.
        /// </summary>
        internal IPublicClientApplicationExecutor PublicClientApplicationExecutor { get; }

        /// <inheritdoc />
        public override Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return ExecuteInternalAsync(cancellationToken);
        }

        internal abstract Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken);
    }
}
