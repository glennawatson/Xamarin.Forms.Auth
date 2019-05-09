﻿// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Xamarin.Forms.Auth
{
    internal enum AuthorizationStatus
    {
        Success,
        ErrorHttp,
        ProtocolError,
        UserCancel,
        UnknownError
    }
}