// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    internal sealed class ApplicationConfiguration : IApplicationConfiguration
    {
        public IHttpManager HttpManager { get; internal set; }

        /// <inheritdoc />
        public Uri Authority { get; internal set; }

        /// <inheritdoc />
        public string TokenEndpointSuffix { get; internal set; } = "/token";

        /// <inheritdoc />
        public string AuthorizeEndpointSuffix { get; internal set; } = "/authorize";

        /// <inheritdoc />
        public string ClientId { get; internal set; }

        /// <inheritdoc />
        public Uri RedirectUri { get; internal set; }

        /// <inheritdoc />
        public bool EnablePiiLogging { get; internal set; }

        /// <inheritdoc />
        public LogLevel LogLevel { get; internal set; } = LogLevel.Info;

        /// <inheritdoc />
        public bool IsDefaultPlatformLoggingEnabled { get; internal set; }

        /// <inheritdoc />
        public IAuthHttpClientFactory HttpClientFactory { get; internal set; }

        /// <inheritdoc />
        public bool IsExtendedTokenLifetimeEnabled { get; set; }

        /// <inheritdoc />
        public LogCallback LoggingCallback { get; internal set; }

        public string Component { get; internal set; }

        /// <inheritdoc />
        public IDictionary<string, string> ExtraQueryParameters { get; internal set; } = new Dictionary<string, string>();

#if iOS
        /// <summary>
        /// Gets or sets the iOS keychain security group to use.
        /// </summary>
        public string IosKeychainSecurityGroup { get; internal set; }
#endif // iOS
    }
}
