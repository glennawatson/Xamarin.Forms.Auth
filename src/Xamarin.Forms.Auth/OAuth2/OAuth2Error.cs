// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// OAuth2 errors that are only used internally. All error codes used when propagating exceptions should
    /// be made public.
    /// </summary>
    internal static class OAuth2Error
    {
        public const string LoginRequired = "login_required";
        public const string AuthorizationPending = "authorization_pending";
    }
}
