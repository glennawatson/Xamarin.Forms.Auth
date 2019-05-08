// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    ///     Initializes the MSAL module. This can be considered an entry point into MSAL
    ///     for initialization purposes.
    /// </summary>
    /// <remarks>
    ///     The CLR defines a module initializer, however this is not implemented in C# and to
    ///     use this it would require IL weaving, which does not seem to work on all target frameworks.
    ///     Instead, call <see cref="EnsureModuleInitialized" /> from static ctors of public entry points.
    /// </remarks>
    internal static class ModuleInitializer
    {
        private static readonly object LockObj = new object();
        private static volatile bool _isInitialized = false;

        /// <summary>
        ///     Handle all the initialization of singletons, factories, statics etc. Initialization will only happen once.
        /// </summary>
        public static void EnsureModuleInitialized()
        {
            // double check locking instead locking first to improve performance
            if (!_isInitialized)
            {
                lock (LockObj)
                {
                    InitializeModule();
                }
            }
        }

        /// <summary>
        ///     Force initialization of the module, ignoring any previous initializations. Only TESTS should call this method.
        /// </summary>
        /// <remarks>
        ///     Tests can access the internals of the module and modify the initialization pattern, so it is
        ///     acceptable for tests to reinitialize the module.
        /// </remarks>
        public static void ForceModuleInitializationTestOnly()
        {
            lock (LockObj)
            {
                InitializeModule();
            }
        }

        private static void InitializeModule()
        {
            OAuth2Logger.Default = new OAuth2Logger(Guid.Empty, null);
            _isInitialized = true;
        }
    }
}
