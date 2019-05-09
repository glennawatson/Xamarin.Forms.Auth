// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Auth
{
    internal class AcquireTokenCommonParameters
    {
        public IEnumerable<string> Scopes { get; set; }

        public IDictionary<string, string> ExtraQueryParameters { get; set; }

        public string Claims { get; set; }

        public Uri AuthorityOverride { get; set; }
    }
}
