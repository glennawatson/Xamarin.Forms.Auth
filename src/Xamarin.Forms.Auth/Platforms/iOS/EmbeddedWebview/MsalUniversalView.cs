// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using CoreGraphics;

using UIKit;

namespace Xamarin.Forms.Auth
{
    [Foundation.Register("MsalUniversalView")]
    internal class MsalUniversalView : UIView
    {
        public MsalUniversalView()
        {
            Initialize();
        }

        public MsalUniversalView(CGRect bounds)
            : base(bounds)
        {
            Initialize();
        }

        private void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }
}
