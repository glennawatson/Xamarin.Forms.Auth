// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;
using System.IO;
using System.Text;

namespace Xamarin.Forms.Auth
{
    internal class StringWriterWithEncoding : StringWriter
    {
        public StringWriterWithEncoding(Encoding encoding)
            : base(CultureInfo.InvariantCulture)
        {
            Encoding = encoding;
        }

        public override Encoding Encoding { get; }
    }
}
