// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Auth
{
    /// <inheritdoc />
    /// <summary>
    /// Parameter builder for the <see cref="IClientApplicationBase.AcquireTokenSilent(IEnumerable{string}, string)"/>
    /// operation.
    /// </summary>
    public sealed class AcquireTokenSilentParameterBuilder :
        AbstractClientAppBaseAcquireTokenParameterBuilder<AcquireTokenSilentParameterBuilder>
    {
        internal AcquireTokenSilentParameterBuilder(IClientApplicationBaseExecutor clientApplicationBaseExecutor)
            : base(clientApplicationBaseExecutor)
        {
        }

        private AcquireTokenSilentParameters Parameters { get; } = new AcquireTokenSilentParameters();

        /// <summary>
        /// Specifies if the client application should force refreshing the
        /// token from the user token cache. By default the token is taken from the
        /// the application token cache (forceRefresh=false).
        /// </summary>
        /// <param name="forceRefresh">If <c>true</c>, ignore any access token in the user token cache
        /// and attempt to acquire new access token using the refresh token for the account
        /// if one is available. This can be useful in the case when the application developer wants to make
        /// sure that conditional access policies are applied immediately, rather than after the expiration of the access token.
        /// The default is <c>false</c>.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        /// <remarks>Avoid un-necessarily setting <paramref name="forceRefresh"/> to <c>true</c> true in order to
        /// avoid negatively affecting the performance of your application.</remarks>
        public AcquireTokenSilentParameterBuilder WithForceRefresh(bool forceRefresh)
        {
            Parameters.ForceRefresh = forceRefresh;
            return this;
        }

        internal static AcquireTokenSilentParameterBuilder Create(
            IClientApplicationBaseExecutor clientApplicationBaseExecutor,
            IEnumerable<string> scopes,
            string loginHint)
        {
            return new AcquireTokenSilentParameterBuilder(clientApplicationBaseExecutor).WithScopes(scopes).WithLoginHint(loginHint);
        }

        /// <inheritdoc />
        internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
        {
            return ClientApplicationBaseExecutor.ExecuteAsync(CommonParameters, Parameters, cancellationToken);
        }

        /// <inheritdoc />
        protected override void Validate()
        {
            base.Validate();
            if (string.IsNullOrWhiteSpace(Parameters.LoginHint))
            {
                throw new AuthUiRequiredException(AuthError.UserNullError, AuthErrorMessage.MsalUiRequiredMessage);
            }
        }

        private AcquireTokenSilentParameterBuilder WithLoginHint(string loginHint)
        {
            Parameters.LoginHint = loginHint;
            return this;
        }
    }
}
