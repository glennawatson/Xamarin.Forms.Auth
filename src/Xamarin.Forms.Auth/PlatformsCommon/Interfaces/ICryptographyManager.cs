// Copyright (c) 2019 Glenn Watson. All rights reserved.
// Glenn Watson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;

namespace Xamarin.Forms.Auth
{
    /// <summary>
    /// Handles common cryptography actions.
    /// </summary>
    internal interface ICryptographyManager
    {
        /// <summary>
        /// Creates a Base 64 URL encoded value with a SHA 256 hash.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The encrypted value.</returns>
        string CreateBase64UrlEncodedSha256Hash(string input);

        /// <summary>
        /// Gets a code verifier for use in OAuth style authentication.
        /// </summary>
        /// <returns>The code verifier.</returns>
        string GenerateCodeVerifier();

        /// <summary>
        /// Creates a SHA256 hash value from the input.
        /// </summary>
        /// <param name="input">The value to encode.</param>
        /// <returns>The hash.</returns>
        string CreateSha256Hash(string input);

        /// <summary>
        /// Creates a SHA256 hash value from the input.
        /// </summary>
        /// <param name="input">The value to encode.</param>
        /// <returns>The hash.</returns>
        byte[] CreateSha256HashBytes(string input);

        /// <summary>
        /// Encrypts the string value.
        /// </summary>
        /// <param name="message">The message to encrypt.</param>
        /// <returns>The encrypted message.</returns>
        string Encrypt(string message);

        /// <summary>
        /// Decrypts the encrypted string.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <returns>The decrypted message.</returns>
        string Decrypt(string encryptedMessage);

        /// <summary>
        /// Encrypts the string value.
        /// </summary>
        /// <param name="message">The message to encrypt.</param>
        /// <returns>The encrypted message.</returns>
        byte[] Encrypt(byte[] message);

        /// <summary>
        /// Decrypts the encrypted string.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <returns>The decrypted message.</returns>
        byte[] Decrypt(byte[] encryptedMessage);

        /// <summary>
        /// Sign a string value with the included certificate.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <param name="certificate">The certificate to sign with.</param>
        /// <returns>The signature.</returns>
        byte[] SignWithCertificate(string message, X509Certificate2 certificate);
    }
}
