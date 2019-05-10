// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Builds the configuration for the public client.
    /// </summary>
    public sealed class PublicClientApplicationBuilder : AbstractApplicationBuilder<PublicClientApplicationBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicClientApplicationBuilder"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to read from.</param>
        internal PublicClientApplicationBuilder(ApplicationConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Creates a PublicClientApplicationBuilder from public client application
        /// configuration options.
        /// </summary>
        /// <param name="options">Public client applications configuration options.</param>
        /// <returns>A <see cref="PublicClientApplicationBuilder"/> from which to set more
        /// parameters, and to create a public client application instance.</returns>
        public static PublicClientApplicationBuilder CreateWithApplicationOptions(PublicClientApplicationOptions options)
        {
            var config = new ApplicationConfiguration();
            return new PublicClientApplicationBuilder(config).WithOptions(options);
        }

        /// <summary>
        /// Creates a PublicClientApplicationBuilder from a clientID.
        /// </summary>
        /// <param name="clientId">Client ID (also known as App ID) of the application as registered in the
        /// application registration portal/.</param>
        /// <returns>A <see cref="PublicClientApplicationBuilder"/> from which to set more
        /// parameters, and to create a public client application instance.</returns>
        public static PublicClientApplicationBuilder Create(string clientId)
        {
            var config = new ApplicationConfiguration();
            return new PublicClientApplicationBuilder(config).WithClientId(clientId);
        }

#if !ANDROID && !MAC
#pragma warning disable RCS1163 // Unused parameter.
                               /// <summary>
                               /// Generates with the iOS security group chain.
                               /// </summary>
                               /// <param name="keychainSecurityGroup">The name of the iOS security group.</param>
                               /// <returns>The builder.</returns>
        public PublicClientApplicationBuilder WithIosKeychainSecurityGroup(string keychainSecurityGroup)
#pragma warning restore RCS1163 // Unused parameter.
        {
#if iOS
            Config.IosKeychainSecurityGroup = keychainSecurityGroup;
#endif // iOS
            return this;
        }
#endif

        /// <summary>
        /// Builds the application from the specified settings inside the builder.
        /// </summary>
        /// <returns>The public client.</returns>
        public IPublicClientApplication Build()
        {
            return BuildConcrete();
        }

        /// <summary>
        /// Builds a concrete instance of the client application.
        /// </summary>
        /// <returns>The client application.</returns>
        internal PublicClientApplication BuildConcrete()
        {
            return new PublicClientApplication(BuildConfiguration());
        }

        /// <inheritdoc />
        internal override void Validate()
        {
            base.Validate();

            if (!Uri.TryCreate(Config.RedirectUri.ToString(), UriKind.Absolute, out Uri uriResult))
            {
                throw new InvalidOperationException(AuthErrorMessage.InvalidRedirectUriReceived(Config.RedirectUri.ToString()));
            }
        }
    }
}
