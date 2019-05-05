// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// The IAccount interface represents information about a single account.
    /// The same user can be present in different tenants, that is, a user can have multiple accounts.
    /// An <c>IAccount</c> is returned in the <see cref="AuthenticationResult"/>.<see cref="AuthenticationResult.Account"/> property, and can be used as parameters
    /// of PublicClientApplication and ConfidentialClientApplication methods acquiring tokens such as <see cref="ClientApplicationBase.AcquireTokenSilentAsync(System.Collections.Generic.IEnumerable{string}, IAccount)"/>
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// Gets a string containing the displayable value in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.
        /// This can be null.
        /// </summary>
        /// <remarks>This property replaces the <c>DisplayableId</c> property of <c>IUser</c> in previous versions of MSAL.NET</remarks>
        string Username { get; }

        /// <summary>
        /// Gets a string containing the identity provider for this account.
        /// </summary>
        /// <remarks>This property replaces the <c>IdentityProvider</c> property of <c>IUser</c> in previous versions of MSAL.NET
        /// except that IdentityProvider was a URL with information about the tenant (in addition to the cloud environment), whereas Environment is only the <see cref="System.Uri.Host"/>.</remarks>
        string Environment { get; }
    }
}
