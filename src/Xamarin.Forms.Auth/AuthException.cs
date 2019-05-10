﻿// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json.Linq;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Base exception type thrown when an error occurs during token acquisition.
    /// </summary>
    /// <remarks>Avoid throwing this exception. Instead throw the more specialized <see cref="AuthClientException"/>
    /// or <see cref="AuthServiceException"/>.
    /// </remarks>
    [Serializable]
    public class AuthException : Exception
    {
        private const string ExceptionTypeKey = "type";
        private const string ErrorCodeKey = "error_code";
        private const string ErrorDescriptionKey = "error_description";

        private static readonly Lazy<Dictionary<string, Type>> _typeNameToType = new Lazy<Dictionary<string, Type>>(
    () => new Dictionary<string, Type>
          {
                      { typeof(AuthException).Name, typeof(AuthException) },
                      { typeof(AuthClientException).Name, typeof(AuthClientException) },
                      { typeof(AuthServiceException).Name, typeof(AuthServiceException) },
                      { typeof(AuthUiRequiredException).Name, typeof(AuthUiRequiredException) },
          });

        private string _errorCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class.
        /// </summary>
        public AuthException()
            : base(AuthErrorMessage.Unknown)
        {
            ErrorCode = AuthError.UnknownError;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class with a specified.
        /// error code.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by the client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        public AuthException(string errorCode)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class with a specified.
        /// error code and error message.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by the client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        public AuthException(string errorCode, string errorMessage)
            : base(errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthException"/> class with a specified.
        /// error code and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by the client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public AuthException(string errorCode, string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the protocol error code returned by the service or generated by the client. This is the code you can rely on for
        /// exception handling. Values for this code are typically provided in constant strings in the derived exceptions types
        /// with explanations of mitigation.
        /// </summary>
        public string ErrorCode
        {
            get => _errorCode;
            private set => _errorCode = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : value;
        }

        /// <summary>
        /// Allows re-hydration of the MsalException (or one of its derived types) from JSON generated by ToJsonString().
        /// </summary>
        /// <param name="json">A Json string representing the auth exception.</param>
        /// <returns>The auth exception.</returns>
        public static AuthException FromJsonString(string json)
        {
            JObject jobj = JObject.Parse(json);
            string type = jobj.Value<string>(ExceptionTypeKey);

            if (_typeNameToType.Value.TryGetValue(type, out Type exceptionType))
            {
                string errorCode = JsonUtils.GetExistingOrEmptyString(jobj, ErrorCodeKey);
                string errorMessage = JsonUtils.GetExistingOrEmptyString(jobj, ErrorDescriptionKey);

                AuthException ex = Activator.CreateInstance(exceptionType, errorCode, errorMessage) as AuthException;
                ex.PopulateObjectFromJson(jobj);
                return ex;
            }

            throw new AuthClientException(AuthError.JsonParseError, AuthErrorMessage.MsalExceptionFailedToParse);
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(CultureInfo.InvariantCulture, "\n\tErrorCode: {0}", ErrorCode);
        }

        /// <summary>
        /// Allows serialization of most values of the exception into JSON.
        /// </summary>
        /// <returns>The JSON string.</returns>
        public string ToJsonString()
        {
            JObject jobj = new JObject();
            PopulateJson(jobj);
            return jobj.ToString();
        }

        internal virtual void PopulateJson(JObject jobj)
        {
            jobj[ExceptionTypeKey] = GetType().Name;
            jobj[ErrorCodeKey] = ErrorCode;
            jobj[ErrorDescriptionKey] = Message;
        }

        internal virtual void PopulateObjectFromJson(JObject jobj)
        {
        }
    }
}
