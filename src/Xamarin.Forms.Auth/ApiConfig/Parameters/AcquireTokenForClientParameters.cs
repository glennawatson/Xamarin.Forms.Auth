// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text;

namespace Xamarin.Forms.Auth
{
    internal class AcquireTokenForClientParameters : IAcquireTokenParameters
    {
        public bool ForceRefresh { get; set; }

        public bool SendX5C { get; set; }

        /// <inheritdoc />
        public void LogParameters(ICoreLogger logger)
        {
            var builder = new StringBuilder();
            builder.AppendLine("=== AcquireTokenForClientParameters ===")
                .AppendLine("SendX5C: " + SendX5C)
                .AppendLine("ForceRefresh: " + ForceRefresh);
            logger.Info(builder.ToString());
        }
    }
}
