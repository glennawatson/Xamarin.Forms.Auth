// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reflection;

namespace Xamarin.Forms.Auth
{
    internal static class AssemblyUtils
    {
        public static string GetAssemblyFileVersionAttribute()
        {
            // TODO:  Pick one of these and let's finalize...
            // return typeof (MsalIdHelper).GetTypeInfo().Assembly.GetName().Version.ToString();
            return typeof(AssemblyUtils).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
        }

        public static string GetAssemblyInformationalVersion()
        {
            var attribute = typeof(AssemblyUtils).GetTypeInfo().Assembly
                                                 .GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return attribute != null ? attribute.InformationalVersion : string.Empty;
        }
    }
}
