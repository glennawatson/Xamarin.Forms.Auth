// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;

namespace Xamarin.Forms.Auth
{
    internal class CoreUIParent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreUIParent"/> class.
        /// </summary>
        /// <param name="activity">parent activity for the call. REQUIRED.</param>
        public CoreUIParent(Activity activity)
        {
            Activity = activity ?? throw new ArgumentException("passed in activity is null", nameof(activity));
            CallerActivity = activity;
        }

        /// <summary>
        /// Gets or sets the Caller Android Activity.
        /// </summary>
        public Activity CallerActivity { get; set; }

        internal bool UseEmbeddedWebview { get; set; }

        internal Activity Activity { get; set; }
    }
}
