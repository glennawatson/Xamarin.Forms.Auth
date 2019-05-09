// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Foundation;

namespace Xamarin.Forms.Auth
{
    internal class CoreCustomUrlProtocol : NSUrlProtocol
    {
        private NSUrlConnection _connection;

        [Export("initWithRequest:cachedResponse:client:")]
        public CoreCustomUrlProtocol(
            NSUrlRequest request,
            NSCachedUrlResponse cachedResponse,
            INSUrlProtocolClient client)
            : base(request, cachedResponse, client)
        {
        }

        [Export("canInitWithRequest:")]
        public static new bool CanInitWithRequest(NSUrlRequest request)
        {
            if (request.Url.Scheme.Equals("https", StringComparison.CurrentCultureIgnoreCase))
            {
                return GetProperty("ADURLProtocol", request) == null;
            }

            return false;
        }

        [Export("canonicalRequestForRequest:")]
        public static new NSUrlRequest GetCanonicalRequest(NSUrlRequest request)
        {
            return request;
        }

        public override void StartLoading()
        {
            if (Request == null)
            {
                return;
            }

            NSMutableUrlRequest mutableRequest = (NSMutableUrlRequest)Request.MutableCopy();
            SetProperty(new NSString("YES"), "ADURLProtocol", mutableRequest);
            _connection = new NSUrlConnection(mutableRequest, new CoreCustomConnectionDelegate(this), true);
        }

        public override void StopLoading()
        {
            _connection.Cancel();
        }

        private class CoreCustomConnectionDelegate : NSUrlConnectionDataDelegate
        {
            private readonly CoreCustomUrlProtocol _handler;
            private readonly INSUrlProtocolClient _client;

            public CoreCustomConnectionDelegate(CoreCustomUrlProtocol handler)
            {
                _handler = handler;
                _client = handler.Client;
            }

            public override void ReceivedData(NSUrlConnection connection, NSData data)
            {
                _client.DataLoaded(_handler, data);
            }

            public override void FailedWithError(NSUrlConnection connection, NSError error)
            {
                _client.FailedWithError(_handler, error);
                connection.Cancel();
            }

            public override void ReceivedResponse(NSUrlConnection connection, NSUrlResponse response)
            {
                _client.ReceivedResponse(_handler, response, NSUrlCacheStoragePolicy.NotAllowed);
            }

            public override NSUrlRequest WillSendRequest(
                NSUrlConnection connection,
                NSUrlRequest request,
                NSUrlResponse response)
            {
                NSMutableUrlRequest mutableRequest = (NSMutableUrlRequest)request.MutableCopy();
                if (response != null)
                {
                    RemoveProperty("ADURLProtocol", mutableRequest);
                    _client.Redirected(_handler, mutableRequest, response);
                    connection.Cancel();
                    if (!request.Headers.ContainsKey(new NSString("x-ms-PkeyAuth")))
                    {
                        mutableRequest[BrokerConstants.ChallengeHeaderKey] = BrokerConstants.ChallengeHeaderValue;
                    }

                    return mutableRequest;
                }

                if (!request.Headers.ContainsKey(new NSString(BrokerConstants.ChallengeHeaderKey)))
                {
                    mutableRequest[BrokerConstants.ChallengeHeaderKey] = BrokerConstants.ChallengeHeaderValue;
                }

                return mutableRequest;
            }

            public override void FinishedLoading(NSUrlConnection connection)
            {
                _client.FinishedLoading(_handler);
                connection.Cancel();
            }
        }
    }
}
