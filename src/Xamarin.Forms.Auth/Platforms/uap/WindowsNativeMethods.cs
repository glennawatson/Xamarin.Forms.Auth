// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Xamarin.Forms.Auth
{
    internal static class WindowsNativeMethods
    {
        private const int PROCESSOR_ARCHITECTURE_AMD64 = 9;
        private const int PROCESSOR_ARCHITECTURE_ARM = 5;
        private const int PROCESSOR_ARCHITECTURE_IA64 = 6;
        private const int PROCESSOR_ARCHITECTURE_INTEL = 0;

        [DllImport("kernel32.dll")]
        private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public static string GetProcessorArchitecture()
        {
            try
            {
                var systemInfo = new SYSTEM_INFO();
                GetNativeSystemInfo(ref systemInfo);
                switch (systemInfo.wProcessorArchitecture)
                {
                case PROCESSOR_ARCHITECTURE_AMD64:
                case PROCESSOR_ARCHITECTURE_IA64:
                    return "x64";

                case PROCESSOR_ARCHITECTURE_ARM:
                    return "ARM";

                case PROCESSOR_ARCHITECTURE_INTEL:
                    return "x86";

                default:
                    return "Unknown";
                }
            }
            catch (Exception ex)
            {
                MsalLogger.Default.WarningPii(ex);
                return "Unknown";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
        {
            public readonly short wProcessorArchitecture;
            public readonly short wReserved;
            public readonly int dwPageSize;
            public readonly IntPtr lpMinimumApplicationAddress;
            public readonly IntPtr lpMaximumApplicationAddress;
            public readonly IntPtr dwActiveProcessorMask;
            public readonly int dwNumberOfProcessors;
            public readonly int dwProcessorType;
            public readonly int dwAllocationGranularity;
            public readonly short wProcessorLevel;
            public readonly short wProcessorRevision;
        }
    }
}