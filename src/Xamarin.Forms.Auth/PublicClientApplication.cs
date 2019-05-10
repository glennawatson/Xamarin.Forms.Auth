// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
    /// <summary>
    /// Class to be used to acquire tokens in desktop or mobile applications (Desktop / UWP / Xamarin.iOS / Xamarin.Android).
    /// public client applications are not trusted to safely keep application secrets, and therefore they only access Web APIs in the name of the user only.
    /// For details see https://aka.ms/auth-net-client-applications.
    /// </summary>
    public sealed partial class PublicClientApplication : ClientApplicationBase, IPublicClientApplication, IByRefreshToken
    {
        internal PublicClientApplication(ApplicationConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the system web view is available.
        /// </summary>
        public bool IsSystemWebViewAvailable
        {
            get => ServiceBundle.PlatformProxy.IsSystemWebViewAvailable;
        }

        /// <inheritdoc />
        [CLSCompliant(false)]
        public AcquireTokenInteractiveParameterBuilder AcquireTokenInteractive(
            IEnumerable<string> scopes)
        {
            return AcquireTokenInteractiveParameterBuilder.Create(
                ClientExecutorFactory.CreatePublicClientExecutor(this),
                scopes);
        }

        /// <inheritdoc />
        AcquireTokenByRefreshTokenParameterBuilder IByRefreshToken.AcquireTokenByRefreshToken(
            IEnumerable<string> scopes,
            string refreshToken)
        {
            return AcquireTokenByRefreshTokenParameterBuilder.Create(
                ClientExecutorFactory.CreateClientApplicationBaseExecutor(this),
                scopes,
                refreshToken);
        }
    }
}
