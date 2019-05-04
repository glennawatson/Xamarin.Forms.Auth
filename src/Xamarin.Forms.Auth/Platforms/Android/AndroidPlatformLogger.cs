// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Android.Util;

namespace Xamarin.Forms.Auth
{
    internal class AndroidPlatformLogger : IPlatformLogger
    {
        /// <inheritdoc />
        public void Error(string errorMessage)
        {
            Log.Error(null, errorMessage);
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
            Log.Warn(null, message);
        }

        /// <inheritdoc />
        public void Verbose(string message)
        {
            Log.Verbose(null, message);
        }

        /// <inheritdoc />
        public void Information(string message)
        {
            Log.Info(null, message);
        }
    }
}
