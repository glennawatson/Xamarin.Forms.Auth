// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text;

namespace Xamarin.Forms.Auth
{
    internal static class Base64UrlHelpers
    {
        private const char Base64PadCharacter = '=';
        private const char Base64Character62 = '+';
        private const char Base64Character63 = '/';
        private const char Base64UrlCharacter62 = '-';
        private const char Base64UrlCharacter63 = '_';
        private static readonly Encoding TextEncoding = Encoding.UTF8;

        private static readonly string DoubleBase64PadCharacter = string.Format(CultureInfo.InvariantCulture, "{0}{0}", Base64PadCharacter);

        /// <summary>
        /// The following functions perform base64url encoding which differs from regular base64 encoding as follows
        /// * padding is skipped so the pad character '=' doesn't have to be percent encoded
        /// * the 62nd and 63rd regular base64 encoding characters ('+' and '/') are replace with ('-' and '_')
        /// The changes make the encoding alphabet file and URL safe
        /// See RFC4648, section 5 for more info.
        /// </summary>
        /// <param name="arg">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string Encode(string arg)
        {
            if (arg == null)
            {
                return null;
            }

            return Encode(TextEncoding.GetBytes(arg));
        }

        public static string DecodeToString(string arg)
        {
            byte[] decoded = DecodeToBytes(arg);
            return CoreHelpers.CreateString(decoded);
        }

        public static byte[] DecodeToBytes(string arg)
        {
            string s = arg;
            s = s.Replace(Base64UrlCharacter62, Base64Character62)
                .Replace(Base64UrlCharacter63, Base64Character63);

            switch (s.Length % 4)
            {
                // Pad
                case 0:
                    break; // No pad chars in this case
                case 2:
                    s += DoubleBase64PadCharacter;
                    break; // Two pad chars
                case 3:
                    s += Base64PadCharacter;
                    break; // One pad char
                default:
                    throw new ArgumentException("Illegal base64url string!", nameof(arg));
            }

            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        internal static string Encode(byte[] arg)
        {
            if (arg == null)
            {
                return null;
            }

            string s = Convert.ToBase64String(arg);
            s = s.Split(Base64PadCharacter)[0]; // RemoveAccount any trailing padding
            s = s.Replace(Base64Character62, Base64UrlCharacter62)
                .Replace(Base64Character63, Base64UrlCharacter63);

            return s;
        }
    }
}
