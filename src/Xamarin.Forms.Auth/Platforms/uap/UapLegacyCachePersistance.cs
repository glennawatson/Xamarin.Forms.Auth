// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Xamarin.Forms.Auth
{
    internal class UapLegacyCachePersistence : ILegacyCachePersistence
    {
        private const string LocalSettingsContainerName = "ActiveDirectoryAuthenticationLibrary";

        private const string CacheValue = "CacheValue";
        private const string CacheValueSegmentCount = "CacheValueSegmentCount";
        private const string CacheValueLength = "CacheValueLength";
        private const int MaxCompositeValueLength = 1024;

        private readonly ICryptographyManager _cryptographyManager;

        public UapLegacyCachePersistence(ICryptographyManager cryptographyManager)
        {
            _cryptographyManager = cryptographyManager;
        }

        byte[] ILegacyCachePersistence.LoadCache()
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.CreateContainer(LocalSettingsContainerName,
                    ApplicationDataCreateDisposition.Always);
                return GetCacheValue(localSettings.Containers[LocalSettingsContainerName].Values);
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
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.CreateContainer(LocalSettingsContainerName, ApplicationDataCreateDisposition.Always);
                SetCacheValue(localSettings.Containers[LocalSettingsContainerName].Values, serializedCache);

            }
            catch (Exception ex)
            {
                MsalLogger.Default.WarningPiiWithPrefix(ex, "Failed to save adal cache: ");
            }
        }

        internal void SetCacheValue(IPropertySet containerValues, byte[] value)
        {
            byte[] encryptedValue = _cryptographyManager.Encrypt(value);
            containerValues[CacheValueLength] = encryptedValue.Length;
            if (encryptedValue.Length == 0)
            {
                containerValues[CacheValueSegmentCount] = 1;
                containerValues[CacheValue + 0] = null;
            }
            else
            {
                int segmentCount = (encryptedValue.Length / MaxCompositeValueLength) + ((encryptedValue.Length % MaxCompositeValueLength == 0) ? 0 : 1);
                byte[] subValue = new byte[MaxCompositeValueLength];
                for (int i = 0; i < segmentCount - 1; i++)
                {
                    Array.Copy(encryptedValue, i * MaxCompositeValueLength, subValue, 0, MaxCompositeValueLength);
                    containerValues[CacheValue + i] = subValue;
                }

                int copiedLength = (segmentCount - 1) * MaxCompositeValueLength;
                Array.Copy(encryptedValue, copiedLength, subValue, 0, encryptedValue.Length - copiedLength);
                containerValues[CacheValue + (segmentCount - 1)] = subValue;
                containerValues[CacheValueSegmentCount] = segmentCount;
            }
        }

        internal byte[] GetCacheValue(IPropertySet containerValues)
        {
            if (!containerValues.ContainsKey(CacheValueLength))
            {
                return null;
            }

            int encyptedValueLength = (int)containerValues[CacheValueLength];
            int segmentCount = (int)containerValues[CacheValueSegmentCount];

            byte[] encryptedValue = new byte[encyptedValueLength];
            if (segmentCount == 1)
            {
                encryptedValue = (byte[])containerValues[CacheValue + 0];
            }
            else
            {
                for (int i = 0; i < segmentCount - 1; i++)
                {
                    Array.Copy((byte[])containerValues[CacheValue + i], 0, encryptedValue, i * MaxCompositeValueLength, MaxCompositeValueLength);
                }
            }

            Array.Copy((byte[])containerValues[CacheValue + (segmentCount - 1)], 0, encryptedValue, (segmentCount - 1) * MaxCompositeValueLength, encyptedValueLength - (segmentCount - 1) * MaxCompositeValueLength);
            return _cryptographyManager.Decrypt(encryptedValue);
        }
    }
}
