// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#if iOS
using UIKit;
#endif

#if ANDROID
using Android.App;
#endif

#if MAC
using AppKit;
#endif

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Builder for an Interactive token request.
    /// </summary>
    public sealed class AcquireTokenInteractiveParameterBuilder :
        AbstractPublicClientAcquireTokenParameterBuilder<AcquireTokenInteractiveParameterBuilder>
    {
        internal AcquireTokenInteractiveParameterBuilder(IPublicClientApplicationExecutor publicClientApplicationExecutor)
            : base(publicClientApplicationExecutor)
        {
        }

        private AcquireTokenInteractiveParameters Parameters { get; } = new AcquireTokenInteractiveParameters();

        /// <summary>
        /// Specifies if the public client application should used an embedded web browser
        /// or the system default browser.
        /// </summary>
        /// <param name="useEmbeddedWebView">If <c>true</c>, will use an embedded web browser,
        /// otherwise will attempt to use a system web browser. The default depends on the platform:
        /// <c>false</c> for Xamarin.iOS and Xamarin.Android, and <c>true</c> for .NET Framework,
        /// and UWP.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public AcquireTokenInteractiveParameterBuilder WithUseEmbeddedWebView(bool useEmbeddedWebView)
        {
            Parameters.UseEmbeddedWebView = useEmbeddedWebView;
            return this;
        }

        /// <summary>
        /// Sets the <paramref name="loginHint"/>, in order to avoid select account
        /// dialogs in the case the user is signed-in with several identities.
        /// </summary>
        /// <param name="loginHint">Identifier of the user. Generally in UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c>.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public AcquireTokenInteractiveParameterBuilder WithLoginHint(string loginHint)
        {
            Parameters.LoginHint = loginHint;
            return this;
        }

        /// <summary>
        /// Adds extra scopes to consent to.
        /// </summary>
        /// <param name="extraScopesToConsent">Scopes that you can request the end user to consent upfront,
        /// in addition to the scopes for the protected Web API for which you want to acquire a security token.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public AcquireTokenInteractiveParameterBuilder WithExtraScopesToConsent(IEnumerable<string> extraScopesToConsent)
        {
            Parameters.ExtraScopesToConsent = extraScopesToConsent;
            return this;
        }

        /// <summary>
        /// Specifies the what the interactive experience is for the user.
        /// </summary>
        /// <param name="prompt">Requested interactive experience. The default is <see cref="Prompt.SelectAccount"/>.
        /// </param>
        /// <returns>The builder to chain the .With methods.</returns>
        public AcquireTokenInteractiveParameterBuilder WithPrompt(Prompt prompt)
        {
            Parameters.Prompt = prompt;
            return this;
        }

#if ANDROID
        /// <summary>
        /// Sets a reference to the current Activity that triggers the browser to be shown. Required
        /// for MSAL to be able to show the browser when using Xamarin.Android.
        /// </summary>
        /// <param name="activity">The current Activity.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        [CLSCompliant(false)]
        public AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindow(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException(nameof(activity));
            }

            return WithParentObject((object)activity);
        }
#endif

#if iOS
        /// <summary>
        /// Sets a reference to the current ViewController that triggers the browser to be shown.
        /// </summary>
        /// <param name="viewController">The current ViewController.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        [CLSCompliant(false)]
        public AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindow(UIViewController viewController)
        {
            if (viewController == null)
            {
                throw new ArgumentNullException(nameof(viewController));
            }

            return WithParentObject((object)viewController);
        }
#endif

#if MAC
        /// <summary>
        /// Sets a reference to the current NSWindow. The browser pop-up will be centered on it. If ommited,
        /// it will be centered on the screen.
        /// </summary>
        /// <param name="nsWindow">The current NSWindow.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        [CLSCompliant(false)]
        public AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindow(NSWindow nsWindow)
        {
            if (nsWindow == null)
            {
                throw new ArgumentNullException(nameof(nsWindow));
            }

            return WithParentObject((object)nsWindow);
        }
#endif

        /// <summary>
        ///  Sets a reference to the ViewController (if using Xamarin.iOS), Activity (if using Xamarin.Android)
        ///  IWin32Window or IntPtr (if using .Net Framework). Used for invoking the browser.
        /// </summary>
        /// <remarks>Mandatory only on Android. Can also be set via the PublicClientApplcation builder.</remarks>
        /// <param name="parent">The parent as an object, so that it can be used from shared NetStandard assemblies.</param>
        /// <returns>The builder to chain the .With methods.</returns>
        public AcquireTokenInteractiveParameterBuilder WithParentActivityOrWindow(object parent)
        {
            return WithParentObject(parent);
        }

        internal static AcquireTokenInteractiveParameterBuilder Create(
            IPublicClientApplicationExecutor publicClientApplicationExecutor,
            IEnumerable<string> scopes)
        {
            return new AcquireTokenInteractiveParameterBuilder(publicClientApplicationExecutor)
                .WithCurrentSynchronizationContext()
                .WithScopes(scopes);
        }

        /// <inheritdoc />
        internal override Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken)
        {
            return PublicClientApplicationExecutor.ExecuteAsync(CommonParameters, Parameters, cancellationToken);
        }

        // This is internal so that we can configure this from the extension methods for ICustomWebUi
        internal void SetCustomWebUi(ICustomWebUi customWebUi)
        {
            Parameters.CustomWebUi = customWebUi;
        }

        internal AcquireTokenInteractiveParameterBuilder WithCurrentSynchronizationContext()
        {
            Parameters.UiParent.SynchronizationContext = SynchronizationContext.Current;
            return this;
        }

        /// <inheritdoc />
        protected override void Validate()
        {
            base.Validate();

#if ANDROID
            if (Parameters.UiParent.Activity == null)
            {
                throw new InvalidOperationException(AuthErrorMessage.ActivityRequiredForParentObjectAndroid);
            }
#endif
        }

#pragma warning disable RCS1163 // Unused parameter.
        private AcquireTokenInteractiveParameterBuilder WithParentObject(object parent)
#pragma warning restore RCS1163 // Unused parameter.
        {
#if ANDROID
            if (parent is Activity activity)
            {
                Parameters.UiParent.Activity = activity;
                Parameters.UiParent.CallerActivity = activity;
            }
#elif iOS
            if (parent is UIViewController uiViewController)
            {
                Parameters.UiParent.CallerViewController = uiViewController;
            }
#elif MAC
            if (parent is NSWindow nsWindow)
            {
                Parameters.UiParent.CallerWindow = nsWindow;
            }
#endif

            return this;
        }
    }
}
