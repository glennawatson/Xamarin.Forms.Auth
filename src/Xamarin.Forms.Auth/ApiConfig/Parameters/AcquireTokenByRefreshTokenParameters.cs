// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class AcquireTokenByRefreshTokenParameters : IAcquireTokenParameters
    {
        public string RefreshToken { get; set; }

        public void LogParameters(ICoreLogger logger)
        {
        }
    }
}
