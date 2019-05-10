// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// The configuration for the OAuth application.
    /// </summary>
    internal interface IApplicationConfiguration : IAppConfig
    {
        /// <summary>
        /// Gets a value indicating whether the applications can set to true in case when the STS has an outage,
        /// to be more resilient.
        /// </summary>
        bool IsExtendedTokenLifetimeEnabled { get; }

        /// <summary>
        /// Gets the authority.
        /// </summary>
        Uri Authority { get; }

        /// <summary>
        /// Gets the token end point suffix. This is added to the end of the <see cref="Authority" /> Uri.
        /// </summary>
        string TokenEndpointSuffix { get; }

        /// <summary>
        /// Gets the authorize endpoint suffix. This is added to the end of the <see cref="Authority"/>/ Uri.
        /// </summary>
        string AuthorizeEndpointSuffix { get; }
    }
}
