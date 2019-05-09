// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Log the token parameters.
    /// </summary>
    internal interface IAcquireTokenParameters
    {
        /// <summary>
        /// Logs the parameters.
        /// </summary>
        /// <param name="logger">The logger to log to.</param>
        void LogParameters(ICoreLogger logger);
    }
}
