// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal class RequestContext
    {
        public RequestContext(string clientId, ICoreLogger logger)
        {
            ClientId = string.IsNullOrWhiteSpace(clientId) ? "unset_client_id" : clientId;
            Logger = logger;
        }

        public string ClientId { get; set; }

        public ICoreLogger Logger { get; set; }
    }
}
