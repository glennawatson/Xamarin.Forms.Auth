// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Util;

namespace Xamarin.Forms.Auth
{
    internal class AndroidPlatformLogger : IPlatformLogger
    {
        public void Error(string errorMessage)
        {
            Log.Error(null, errorMessage);
        }

        public void Warning(string message)
        {
            Log.Warn(null, message);
        }

        public void Verbose(string message)
        {
            Log.Verbose(null, message);
        }

        public void Information(string message)
        {
            Log.Info(null, message);
        }
    }
}
