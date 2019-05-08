// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    ///     Factory to manage and throw proper exceptions for MSAL.
    /// </summary>
    internal static class ExceptionFactory
    {
        public static Exception GetClientException(string errorCode, string errorMessage, Exception innerException = null)
        {
            ValidateRequiredArgs(errorCode, errorMessage);

            return new AuthException(errorCode, errorMessage, innerException);
        }

        public static Exception GetServiceException(string errorCode, string errorMessage, IHttpWebResponse httpResponse)
        {
            ValidateRequiredArgs(errorCode, errorMessage);
            return GetServiceException(errorCode, errorMessage, null, ExceptionDetail.FromHttpResponse(httpResponse));
        }

        public static Exception GetServiceException(string errorCode, string errorMessage, Exception innerException, IHttpWebResponse httpResponse)
        {
            ValidateRequiredArgs(errorCode, errorMessage);
            return GetServiceException(errorCode, errorMessage, innerException, ExceptionDetail.FromHttpResponse(httpResponse));
        }

        public static Exception GetServiceException(string errorCode, string errorMessage, ExceptionDetail exceptionDetail)
        {
            ValidateRequiredArgs(errorCode, errorMessage);
            return GetServiceException(errorCode, errorMessage, null, exceptionDetail);
        }

        public static Exception GetServiceException(
            string errorCode,
            string errorMessage,
            Exception innerException,
            ExceptionDetail exceptionDetail)
        {
            ValidateRequiredArgs(errorCode, errorMessage);

            return new AuthServiceException(errorCode, errorMessage, innerException)
            {
                Claims = exceptionDetail?.Claims,
                ResponseBody = exceptionDetail?.ResponseBody,
                StatusCode = exceptionDetail?.StatusCode ?? 0,
                Headers = exceptionDetail?.HttpResponseHeaders
            };
        }

        public static Exception GetUiRequiredException(
            string errorCode,
            string errorMessage,
            Exception innerException,
            ExceptionDetail exceptionDetail)
        {
            ValidateRequiredArgs(errorCode, errorMessage);

            return new AuthUiRequiredException(errorCode, errorMessage, innerException)
            {
                Claims = exceptionDetail?.Claims,
                ResponseBody = exceptionDetail?.ResponseBody,
                StatusCode = exceptionDetail?.StatusCode ?? 0
            };
        }

        private static void ValidateRequiredArgs(string errorCode, string errorMessage)
        {
            if (string.IsNullOrEmpty(errorCode))
            {
                throw new ArgumentNullException(nameof(errorCode));
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }
        }
    }
}
