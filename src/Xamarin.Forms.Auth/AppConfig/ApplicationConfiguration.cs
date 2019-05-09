// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    internal sealed class ApplicationConfiguration : IApplicationConfiguration
    {
        // For telemetry, the ClientName of the application.
        public string ClientName { get; internal set; }

        // For telemetry, the ClientVersion of the application.
        public string ClientVersion { get; internal set; }

        public bool UseCorporateNetwork { get; internal set; }

        public string IosKeychainSecurityGroup { get; internal set; }

        public IHttpManager HttpManager { get; internal set; }

        public Uri AuthorityInfo { get; internal set; }

        public string ClientId { get; internal set; }

        public Uri RedirectUri { get; internal set; }

        public bool EnablePiiLogging { get; internal set; }

        public LogLevel LogLevel { get; internal set; } = LogLevel.Info;

        public bool IsDefaultPlatformLoggingEnabled { get; internal set; }

        public IAuthHttpClientFactory HttpClientFactory { get; internal set; }

        public bool IsExtendedTokenLifetimeEnabled { get; set; }

        public LogCallback LoggingCallback { get; internal set; }

        public string Component { get; internal set; }

        public IDictionary<string, string> ExtraQueryParameters { get; internal set; } = new Dictionary<string, string>();
    }
}
