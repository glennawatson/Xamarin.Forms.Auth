// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Forms.Auth
{
    internal static class ScopeHelper
    {
        private static readonly SortedSet<string> EmptySet = new SortedSet<string>();

        public static bool ScopeContains(SortedSet<string> outerSet, SortedSet<string> possibleContainedSet)
        {
            foreach (string key in possibleContainedSet)
            {
                if (!outerSet.Contains(key, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        internal static SortedSet<string> ConvertStringToLowercaseSortedSet(string singleString)
        {
            if (string.IsNullOrEmpty(singleString))
            {
                return new SortedSet<string>();
            }

            return new SortedSet<string>(singleString.ToLowerInvariant().Split(new[] { " " }, StringSplitOptions.None));
        }

        internal static SortedSet<string> CreateSortedSetFromEnumerable(IEnumerable<string> input)
        {
            if (input == null || !input.Any())
            {
                return EmptySet;
            }

            return new SortedSet<string>(input);
        }
    }
}
