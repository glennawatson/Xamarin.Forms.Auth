// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Http.Headers;

namespace Xamarin.Forms.Auth
{
    internal interface IHttpWebResponse 
    {
        HttpStatusCode StatusCode { get; }
        HttpResponseHeaders Headers { get; }
        string Body { get; }
    }
}
