// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Auth
{
    internal class AcquireTokenInteractiveParameters : IAcquireTokenParameters
    {
        public Prompt Prompt { get; set; } = Prompt.SelectAccount;

        public CoreUIParent UiParent { get; } = new CoreUIParent();

        public IEnumerable<string> ExtraScopesToConsent { get; set; } = new List<string>();

        public bool UseEmbeddedWebView { get; set; }

        public string LoginHint { get; set; }

        public ICustomWebUi CustomWebUi { get; set; }

        public void LogParameters(ICoreLogger logger)
        {
            var builder = new StringBuilder();
            builder.AppendLine("=== InteractiveParameters Data ===")
                .AppendLine("LoginHint provided: " + !string.IsNullOrEmpty(LoginHint))
                .AppendLine("UseEmbeddedWebView: " + UseEmbeddedWebView)
                .AppendLine("ExtraScopesToConsent: " + string.Join(";", ExtraScopesToConsent ?? new List<string>()))
                .AppendLine("Prompt: " + Prompt.PromptValue)
                .AppendLine("HasCustomWebUi: " + (CustomWebUi != null));

            logger.Info(builder.ToString());
        }
    }
}
