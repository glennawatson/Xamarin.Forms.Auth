// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains parameters used by the MSAL call accessing the cache.
    /// See also <see cref="T:TokenCacheExtensions"/> which contains extension methods
    /// to customize the cache serialization
    /// </summary>
    public sealed partial class TokenCacheNotificationArgs
    {
        /// <summary>
        /// Gets teh <see cref="TokenCache"/> involved in the transaction
        /// </summary>
        public TokenCache TokenCache { get; internal set; }

        /// <summary>
        /// Gets the ClientId (application ID) of the application involved in the cache transaction
        /// </summary>
        public string ClientId { get; internal set; }

        /// <summary>
        /// Gets the account involved in the cache transaction.
        /// </summary>
        public IAccount Account { get; internal set; }

        /// <summary>
        /// Indicates whether the state of the cache has changed, for example when tokens are being added or removed.
        /// Not all cache operations modify the state of the cache.
        /// </summary>
        public bool HasStateChanged { get; internal set; }
    }
}
