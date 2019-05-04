// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Handles encrypting different values.
    /// </summary>
    internal interface ICryptographyManager
    {
        /// <summary>
        /// Creates a base 64 url encoded value with a SHA 256 hash.
        /// </summary>
        /// <param name="input">The value to encrypt.</param>
        /// <returns>The encrypted value.</returns>
        string CreateBase64UrlEncodedSha256Hash(string input);

        /// <summary>
        /// Creates a code verifier value.
        /// </summary>
        /// <returns>The code verifier value.</returns>
        string GenerateCodeVerifier();

        /// <summary>
        /// Create a encrypted string with SHA 256.
        /// </summary>
        /// <param name="input">The value to encrypt.</param>
        /// <returns>The encrypted value.</returns>
        string CreateSha256Hash(string input);

        /// <summary>
        /// Create a encrypted byte array with SHA 256.
        /// </summary>
        /// <param name="input">The value to encrypt.</param>
        /// <returns>The encrypted value.</returns>
        byte[] CreateSha256HashBytes(string input);

        /// <summary>
        /// Encrypt a value.
        /// </summary>
        /// <param name="message">The value to encrypt.</param>
        /// <returns>The encrypted value.</returns>
        string Encrypt(string message);

        /// <summary>
        /// Decrypt a value.
        /// </summary>
        /// <param name="encryptedMessage">The value to decrypt.</param>
        /// <returns>The decrypted value.</returns>
        string Decrypt(string encryptedMessage);

        /// <summary>
        /// Encrypt a value.
        /// </summary>
        /// <param name="message">The value to encrypt.</param>
        /// <returns>The encrypted value.</returns>
        byte[] Encrypt(byte[] message);

        /// <summary>
        /// Decrypt a value.
        /// </summary>
        /// <param name="encryptedMessage">The value to decrypt.</param>
        /// <returns>The decrypted value.</returns>
        byte[] Decrypt(byte[] encryptedMessage);

        /// <summary>
        /// Signs a value with a certificate.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <param name="certificate">The certificate to sign with.</param>
        /// <returns>The signature.</returns>
        byte[] SignWithCertificate(string message, X509Certificate2 certificate);
    }
}
