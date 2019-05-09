// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Newtonsoft.Json.Linq;

namespace Xamarin.Forms.Auth
{
    internal static class JsonUtils
    {
        public static string GetExistingOrEmptyString(JObject json, string key)
        {
            if (json.TryGetValue(key, out var val))
            {
                return val.ToObject<string>();
            }

            return string.Empty;
        }

        public static string ExtractExistingOrEmptyString(JObject json, string key)
        {
            if (json.TryGetValue(key, out var val))
            {
                string strVal = val.ToObject<string>();
                json.Remove(key);
                return strVal;
            }

            return string.Empty;
        }

        public static long ExtractParsedIntOrZero(JObject json, string key)
        {
            string strVal = ExtractExistingOrEmptyString(json, key);
            if (!string.IsNullOrWhiteSpace(strVal) && long.TryParse(strVal, out long result))
            {
                return result;
            }

            return 0;
        }
    }
}
