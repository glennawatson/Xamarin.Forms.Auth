// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text;

namespace Xamarin.Forms.Auth
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Create an array of bytes representing the UTF-8 encoding of the given string.
        /// </summary>
        /// <param name="stringInput">String to get UTF-8 bytes for</param>
        /// <returns>Array of UTF-8 character bytes</returns>
        public static byte[] ToByteArray(this string stringInput)
        {
            return new UTF8Encoding().GetBytes(stringInput);
        }
    }
}
