﻿/**
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
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utils
{
    // Made smaller changes
    // Code from: http://stackoverflow.com/questions/6808652/des-ecb-encryption-and-decryption
    public class XDes
    {
        private byte[] Key;

        public XDes(string pKey)
        {
            Key = new byte[8];
            byte[] temp = Encoding.ASCII.GetBytes(pKey);
            for (int i = 0; i < 8; i++)
            {
                if (temp.Length <= i) Key[i] = 0x0;
                else Key[i] = temp[i];
            }
        }

        public string Decrypt(string encryptedString)
        {
            return Decrypt(Convert.FromBase64String(encryptedString));
        }

        public string Decrypt(byte[] encryptedData)
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.Mode = CipherMode.ECB;
            desProvider.Padding = PaddingMode.Zeros;
            desProvider.Key = Key;
            using (MemoryStream stream = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        public byte[] Encrypt(string decryptedString)
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.Mode = CipherMode.ECB;
            desProvider.Padding = PaddingMode.Zeros;
            desProvider.Key = Key;
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] data = Encoding.Default.GetBytes(decryptedString);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    Console.WriteLine(BitConverter.ToString(stream.ToArray()));
                    return stream.ToArray();
                }
            }
        }
    }
}