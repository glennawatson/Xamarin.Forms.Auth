// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    internal static class UriBuilderExtensions
    {
        public static void AppendQueryParameters(this UriBuilder builder, string queryParams)
        {
            if (builder == null || string.IsNullOrEmpty(queryParams))
            {
                return;
            }

            if (builder.Query.Length > 1)
            {
                builder.Query = builder.Query.Substring(1) + "&" + queryParams;
            }
            else
            {
                builder.Query = queryParams;
            }
        }
    }
}
