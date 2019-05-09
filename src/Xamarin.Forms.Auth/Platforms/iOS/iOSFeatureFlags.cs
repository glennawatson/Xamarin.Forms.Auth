// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Xamarin.Forms.Auth
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
    internal class iOSFeatureFlags : IFeatureFlags
    {
        /// <summary>
        /// Gets a value indicating whether fOCI has not been tested on iOS.
        /// </summary>
        public bool IsFociEnabled => false;
    }
}
