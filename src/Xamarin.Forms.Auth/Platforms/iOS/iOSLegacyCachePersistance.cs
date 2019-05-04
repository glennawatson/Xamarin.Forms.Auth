// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Foundation;
using Security;

namespace Xamarin.Forms.Auth
{
    internal class iOSLegacyCachePersistence : ILegacyCachePersistence
    {
        private const string NAME = "ADAL.PCL.iOS";
        private const string LocalSettingsContainerName = "ActiveDirectoryAuthenticationLibrary";

        private string keychainGroup;

        private string GetBundleId()
        {
            return NSBundle.MainBundle.BundleIdentifier;
        }

        public void SetKeychainSecurityGroup(string keychainSecurityGroup)
        {
            if (keychainSecurityGroup == null)
            {
                keychainGroup = GetBundleId();
            }
            else
            {
                keychainGroup = keychainSecurityGroup;
            }
        }

        byte[] ILegacyCachePersistence.LoadCache()
        {
            try
            {
                SecStatusCode res;
                var rec = new SecRecord(SecKind.GenericPassword)
                {
                    Generic = NSData.FromString(LocalSettingsContainerName),
                    Accessible = SecAccessible.Always,
                    Service = NAME + " Service",
                    Account = NAME + " cache",
                    Label = NAME + " Label",
                    Comment = NAME + " Cache",
                    Description = "Storage for cache"
                };

                if (keychainGroup != null)
                {
                    rec.AccessGroup = keychainGroup;
                }

                var match = SecKeyChain.QueryAsRecord(rec, out res);
                if (res == SecStatusCode.Success && match != null && match.ValueData != null)
                {
                    return match.ValueData.ToArray();

                }
            }
            catch (Exception ex)
            {
                MsalLogger.Default.WarningPiiWithPrefix(ex, "Failed to load adal cache: ");
                // Ignore as the cache seems to be corrupt
            }
            return null;
        }

        void ILegacyCachePersistence.WriteCache(byte[] serializedCache)
        {
            try
            {
                var s = new SecRecord(SecKind.GenericPassword)
                {
                    Generic = NSData.FromString(LocalSettingsContainerName),
                    Accessible = SecAccessible.Always,
                    Service = NAME + " Service",
                    Account = NAME + " cache",
                    Label = NAME + " Label",
                    Comment = NAME + " Cache",
                    Description = "Storage for cache"
                };

                if (keychainGroup != null)
                {
                    s.AccessGroup = keychainGroup;
                }

                var err = SecKeyChain.Remove(s);
                if (err != SecStatusCode.Success)
                {
                    string msg = "Failed to remove adal cache record: ";
                    MsalLogger.Default.WarningPii(msg + err, msg);
                }

                if (serializedCache != null && serializedCache.Length > 0)
                {
                    s.ValueData = NSData.FromArray(serializedCache);
                    err = SecKeyChain.Add(s);
                    if (err != SecStatusCode.Success)
                    {
                        string msg = "Failed to save adal cache record: ";
                        MsalLogger.Default.WarningPii(msg + err, msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MsalLogger.Default.WarningPiiWithPrefix(ex, "Failed to save adal cache: ");
            }
        }
    }
}
