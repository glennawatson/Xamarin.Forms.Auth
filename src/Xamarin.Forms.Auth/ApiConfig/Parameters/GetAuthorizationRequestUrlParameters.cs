// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    internal class GetAuthorizationRequestUrlParameters : IAcquireTokenParameters
    {
        public string RedirectUri { get; set; }

        public IEnumerable<string> ExtraScopesToConsent { get; set; }

        public string LoginHint { get; set; }

        public AcquireTokenInteractiveParameters ToInteractiveParameters()
        {
            return new AcquireTokenInteractiveParameters
            {
                ExtraScopesToConsent = ExtraScopesToConsent,
                LoginHint = LoginHint,
                Prompt = Prompt.SelectAccount,
                UseEmbeddedWebView = false
            };
        }

        /// <inheritdoc />
        public void LogParameters(ICoreLogger logger)
        {
        }
    }
}
