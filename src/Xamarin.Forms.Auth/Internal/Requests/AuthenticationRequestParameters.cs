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
        public RequestContext RequestContext { get; set; }

        public Uri Authority { get; set; }

        public string TenantUpdatedCanonicalAuthority { get; set; }

        public SortedSet<string> Scope { get; set; }

        public string ClientId { get; set; }

        public string AuthorizationCode { get; set; }

        public Uri RedirectUri { get; set; }

        public string LoginHint { get; set; }

        public string ExtraQueryParameters { get; set; }

        public IAccount Account { get; set; }

        public UserAssertion UserAssertion { get; set; }

        public bool IsClientCredentialRequest { get; set; }

        public string SliceParameters { get; set; }

        public bool SendCertificate { get; set; }

        public bool IsRefreshTokenRequest { get; set; }

        public void LogState()
        {
            // Create Pii enabled string builder
            var builder = new StringBuilder(
                Environment.NewLine + "=== Request Data ===" + Environment.NewLine + "Authority Provided? - " +
                (Authority != null) + Environment.NewLine);
            builder.AppendLine("Client Id - " + ClientId)
                .AppendLine("Scopes - " + Scope?.AsSingleString())
                .AppendLine("Redirect Uri - " + RedirectUri?.OriginalString)
                .AppendLine("LoginHint provided? - " + !string.IsNullOrEmpty(LoginHint))
                .AppendLine("User provided? - " + (Account != null));
            Dictionary<string, string> dict = CoreHelpers.ParseKeyValueList(ExtraQueryParameters, '&', true, RequestContext);
            builder.AppendLine("Extra Query Params Keys (space separated) - " + dict.Keys.AsSingleString());
            dict = CoreHelpers.ParseKeyValueList(SliceParameters, '&', true, RequestContext);
            builder.AppendLine("Slice Parameters Keys(space separated) - " + dict.Keys.AsSingleString());

            string messageWithPii = builder.ToString();

            // Create no Pii enabled string builder
            builder = new StringBuilder(
                Environment.NewLine + "=== Request Data ===" + Environment.NewLine + "Authority Provided? - " +
                (Authority != null) + Environment.NewLine);
            builder.AppendLine("Scopes - " + Scope?.AsSingleString())
                .AppendLine("LoginHint provided? - " + !string.IsNullOrEmpty(LoginHint))
                .AppendLine("User provided? - " + (Account != null));
            dict = CoreHelpers.ParseKeyValueList(ExtraQueryParameters, '&', true, RequestContext);
            builder.AppendLine("Extra Query Params Keys (space separated) - " + dict.Keys.AsSingleString());
            dict = CoreHelpers.ParseKeyValueList(SliceParameters, '&', true, RequestContext);
            builder.AppendLine("Slice Parameters Keys(space separated) - " + dict.Keys.AsSingleString());
            RequestContext.Logger.Info(builder.ToString());
        }
    }
}
