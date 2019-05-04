// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Contains information of a single account. A user can be present in multiple directories and thus have multiple accounts.
    /// This information is used for token cache lookup and enforcing the user session on the STS authorize endpoint.
    /// </summary>
    internal sealed class Account : IAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="username">UPN style , can be null.</param>
        /// <param name="environment">Identity provider for this account, e.g. <c>login.microsoftonline.com</c>.</param>
        public Account(string username, string environment)
        {
            Username = username;
            Environment = environment;
        }

        public string Username { get; internal set; }

        public string Environment { get; internal set; }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "Account username: {0} environment {1}",
                Username, Environment);
        }
    }
}
