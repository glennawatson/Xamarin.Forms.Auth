// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Structure containing static members that you can use to specify how the interactive overrides
    /// of AcquireTokenAsync in <see cref="PublicClientApplication"/> should prompt the user.
    /// </summary>
    /// <remarks>Only the .NET Framework platforms allows <c>UIBehavior.Never</c>.</remarks>

    // Hide this for .net core at build time, but it needs to be public at runtime to support NetStandard
#if NET_CORE_BUILDTIME
    internal 
#else
    public
#endif
        struct UIBehavior : IEquatable<UIBehavior>
    {
        /// <summary>
        /// AcquireToken will send <c>prompt=select_account</c> to Azure AD's authorize endpoint
        /// which would present to the user a list of accounts from which one can be selected for
        /// authentication.
        /// </summary>
        public static readonly UIBehavior SelectAccount = new UIBehavior("select_account");

        /// <summary>
        /// The user will be prompted for credentials by the service. It is achieved
        /// by sending <c>prompt=login</c> to the Azure AD service.
        /// </summary>
        public static readonly UIBehavior ForceLogin = new UIBehavior("login");

        /// <summary>
        /// The user will be prompted to consent even if consent was granted before. It is achieved
        /// by sending <c>prompt=consent</c> to Azure AD.
        /// </summary>
        public static readonly UIBehavior Consent = new UIBehavior("consent");

        /// <summary>
        /// Does not request any specific UI to the service, which therefore decides based on the
        /// number of signed-in identities.
        /// This UIBehavior is, for the moment, recommended for Azure AD B2C scenarios where
        /// the developer does not want the user to re-select the account (for instance apply
        /// policies like EditProfile, or ResetPassword, which should apply to the currently signed-in account.
        /// It's not recommended to use this UIBehavior in Azure AD scenarios for the moment.
        /// </summary>
        public static readonly UIBehavior NoPrompt = new UIBehavior("no_prompt");

#if DESKTOP || WINDOWS_APP
        /// <summary>
        /// Only available on .NET platform. AcquireToken will send <c>prompt=attempt_none</c> to 
        /// Azure AD's authorize endpoint and the library will use a hidden webview (and its cookies) to authenticate the user.
        /// This can fail, and in that case a <see cref="MsalUiRequiredException"/> will be thrown.
        /// </summary>
        public static readonly UIBehavior Never = new UIBehavior("attempt_none");
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="UIBehavior"/> struct.
        /// </summary>
        /// <param name="promptValue">The value to prompt for.</param>
        private UIBehavior(string promptValue)
        {
            PromptValue = promptValue;
        }

        internal string PromptValue { get; }

        /// <summary>
        /// operator overload to equality check.
        /// </summary>
        /// <param name="x">first value.</param>
        /// <param name="y">second value.</param>
        /// <returns>true if the objects are equal.</returns>
        public static bool operator ==(UIBehavior x, UIBehavior y)
        {
            return x.PromptValue == y.PromptValue;
        }

        /// <summary>
        /// operator overload to equality check.
        /// </summary>
        /// <param name="x">first value.</param>
        /// <param name="y">second value.</param>
        /// <returns>true if the objects are not equal.</returns>
        public static bool operator !=(UIBehavior x, UIBehavior y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Equals method override to compare UIBehavior structs.
        /// </summary>
        /// <param name="obj">object to compare against.</param>
        /// <returns>true if object are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj is UIBehavior other && Equals(other);
        }

        /// <summary>
        /// Override to compute hashcode.
        /// </summary>
        /// <returns>hash code of the PromptValue.</returns>
        public override int GetHashCode()
        {
            return PromptValue != null ? StringComparer.InvariantCulture.GetHashCode(PromptValue) : 0;
        }

        /// <inheritdoc />
        public bool Equals(UIBehavior other)
        {
            return string.Equals(PromptValue, other.PromptValue, StringComparison.InvariantCulture);
        }
    }
}
