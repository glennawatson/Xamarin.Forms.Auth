// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text;

namespace Xamarin.Forms.Auth
{
    internal class AcquireTokenSilentParameters : IAcquireTokenParameters
    {
        public bool ForceRefresh { get; set; }

        public string LoginHint { get; set; }

        /// <inheritdoc />
        public void LogParameters(ICoreLogger logger)
        {
            var builder = new StringBuilder();
            builder.AppendLine("=== OnBehalfOfParameters ===")
                .AppendLine("LoginHint provided: " + !string.IsNullOrEmpty(LoginHint))
                .AppendLine("ForceRefresh: " + ForceRefresh);
            logger.Info(builder.ToString());
        }
    }
}
