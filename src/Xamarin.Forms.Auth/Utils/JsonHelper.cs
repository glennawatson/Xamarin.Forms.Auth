// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

using Newtonsoft.Json;

namespace Xamarin.Forms.Auth
{
    internal static class JsonHelper
    {
        internal static string SerializeToJson<T>(T toEncode)
        {
            return JsonConvert.SerializeObject(toEncode);
        }

        internal static T DeserializeFromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        internal static T TryToDeserializeFromJson<T>(string json, RequestContext requestContext = null)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            T result = default;
            try
            {
                result = DeserializeFromJson<T>(json);
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                requestContext?.Logger?.WarningPii(ex);
            }

            return result;
        }

        internal static T DeserializeFromJson<T>(byte[] jsonByteArray)
        {
            if (jsonByteArray == null || jsonByteArray.Length == 0)
            {
                return default;
            }

            var serializer = new JsonSerializer();
            using (MemoryStream stream = new MemoryStream(jsonByteArray))
            {
                using (var sr = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        return serializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }
        }
    }
}
