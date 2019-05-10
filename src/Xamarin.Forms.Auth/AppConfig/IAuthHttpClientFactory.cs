// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Factory responsible for creating HttpClient
    /// .Net recommends to use a single instance of HttpClient.
    /// </summary>
    /// <remarks>
    /// Implementations must be thread safe. Consider creating and configuring an HttpClient in the constructor
    /// of the factory, and returning the same object in <see cref="GetHttpClient"/>.
    /// </remarks>
    public interface IAuthHttpClientFactory : IDisposable
    {
        /// <summary>
        /// Method returning an Http client that will be used to
        /// communicate with Azure AD. This enables advanced scenarios.
        /// </summary>
        /// <returns>An Http client.</returns>
        HttpClient GetHttpClient();
    }
}
