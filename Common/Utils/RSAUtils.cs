/**
* This file is part of Tartarus Emulator.
* 
* Tartarus is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* 
* Tartarus is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with Tartarus.  If not, see<http://www.gnu.org/licenses/>.
*/
using System;
using System.Security.Cryptography;

namespace Common.Utils
{
    #region RSAUtils
    public static class RSAUtils
    {
        #region RSA Encrypt/Decrypt
        /// <summary>
        /// Encrypts data using RSA given its public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string publicKey)
        {
            try
            {
                byte[] cipher = null;

                using (RSACryptoServiceProvider csp = PemKeyUtils.GetRSAProviderFromPemString(publicKey))
                {
                    cipher = csp.Encrypt(data, false);
                }

                return cipher;
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Encrypts data using RSA given its public key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, RSAParameters publicKey)
        {
            try
            {
                byte[] cipher = null;

                using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
                {
                    csp.ImportParameters(publicKey);
                    cipher = csp.Encrypt(data, false);
                }

                return cipher;
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Decrypts an RSA-encrypted data array given its private key
        /// </summary>
        /// <param name="cipheredData"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] cipheredData, RSAParameters privateKey)
        {
            try
            {
                byte[] data = null;

                using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
                {
                    csp.ImportParameters(privateKey);
                    data = csp.Decrypt(cipheredData, false);
                }

                return data;

            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
    #endregion
}
