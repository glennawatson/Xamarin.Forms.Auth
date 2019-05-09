// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Auth
{
    internal class AuthenticationRequestParameters
    {
        private readonly AcquireTokenCommonParameters _commonParameters;

        public AuthenticationRequestParameters(
            IServiceBundle serviceBundle,
            AcquireTokenCommonParameters commonParameters,
            RequestContext requestContext)
        {
            _commonParameters = commonParameters;

            Authority = commonParameters.AuthorityOverride == null
                ? serviceBundle.Config.AuthorityInfo
                : commonParameters.AuthorityOverride;

            ClientId = serviceBundle.Config.ClientId;
            Scope = ScopeHelper.CreateSortedSetFromEnumerable(commonParameters.Scopes);
            RedirectUri = serviceBundle.Config.RedirectUri;
            RequestContext = requestContext;

            // Set application wide query parameters.
            ExtraQueryParameters = serviceBundle.Config.ExtraQueryParameters ?? new Dictionary<string, string>();

            // Copy in call-specific query parameters.
            if (commonParameters.ExtraQueryParameters != null)
            {
                foreach (var kvp in commonParameters.ExtraQueryParameters)
                {
                    ExtraQueryParameters[kvp.Key] = kvp.Value;
                }
            }
        }

        public RequestContext RequestContext { get; }

        public Uri Authority { get; set; }

        public SortedSet<string> Scope { get; }

        public string ClientId { get; }

        public Uri RedirectUri { get; set; }

        public IDictionary<string, string> ExtraQueryParameters { get; }

        public string Claims => _commonParameters.Claims;

        public Uri AuthorityOverride => _commonParameters.AuthorityOverride;

        public string LoginHint { get; set; }

        public bool IsRefreshTokenRequest { get; set; }

        public void LogParameters(ICoreLogger logger)
        {
            // Create Pii enabled string builder
            var builder = new StringBuilder(
                Environment.NewLine + "=== Request Data ===" + Environment.NewLine + "Authority Provided? - " +
                (Authority != null) + Environment.NewLine);
            builder.AppendLine("Client Id - " + ClientId)
                .AppendLine("Scopes - " + Scope?.AsSingleString())
                .AppendLine("Redirect Uri - " + RedirectUri?.OriginalString)
                .AppendLine("Extra Query Params Keys (space separated) - " + ExtraQueryParameters.Keys.AsSingleString());

            string messageWithPii = builder.ToString();

            // Create no Pii enabled string builder
            builder = new StringBuilder(
                Environment.NewLine + "=== Request Data ===" + Environment.NewLine + "Authority Provided? - " +
                (Authority != null) + Environment.NewLine);
            builder.AppendLine("Scopes - " + Scope?.AsSingleString())
                .AppendLine("Extra Query Params Keys (space separated) - " + ExtraQueryParameters.Keys.AsSingleString());
            logger.InfoPii(messageWithPii, builder.ToString());
        }
    }
}
