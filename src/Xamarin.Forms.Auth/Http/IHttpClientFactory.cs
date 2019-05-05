// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net.Http;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// A factory which produces the HttpClient used throughout the application.
    /// </summary>
    internal interface IHttpClientFactory
    {
        /// <summary>
        /// Gets the Http Client.
        /// </summary>
        HttpClient HttpClient { get; }
    }
}
