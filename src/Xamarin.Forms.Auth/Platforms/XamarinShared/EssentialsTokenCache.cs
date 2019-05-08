// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Essentials;

namespace Xamarin.Forms.Auth
{
    internal class EssentialsTokenCache : ITokenCache
    {
        /// <inheritdoc />
        public async Task SaveAccessAndRefreshToken(Uri authority, string clientId, OAuth2TokenResponse tokenResponse)
        {
            try
            {
                await SecureStorage.SetAsync($"oauth_token_{clientId}_{authority}", JsonConvert.SerializeObject(tokenResponse)).ConfigureAwait(false);
            }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
            catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        /// <inheritdoc />
        public async Task<OAuth2TokenResponse> GetAccessToken(Uri authority, string clientId)
        {
            try
            {
                return JsonConvert.DeserializeObject<OAuth2TokenResponse>(await SecureStorage.GetAsync($"oauth_token_{clientId}_{authority}").ConfigureAwait(false));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public Task RemoveAccount(Uri authority, string clientId)
        {
            SecureStorage.Remove($"oauth_token_{clientId}_{authority}");

            return Task.CompletedTask;
        }
    }
}
