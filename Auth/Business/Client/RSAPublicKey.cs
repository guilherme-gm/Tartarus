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
using Common.DataClasses;
using Common.DataClasses.Network;
using AC = Auth.DataClasses.Network.AuthClient;
using CA = Auth.DataClasses.Network.ClientAuth;
using System.Security.Cryptography;
using Common.Utils;
using Auth.DataClasses;

namespace Auth.Business.Client
{
    #region RSAPublicKey
    public class RSAPublicKey : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CA.RSAPublicKey packet = (CA.RSAPublicKey)message;
            AC.AesKeyIV aesKey = new AC.AesKeyIV();

            byte[] aesInfo = new byte[32];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(aesInfo);
            }

            aesKey.RSAEncryptedData = RSAUtils.Encrypt(aesInfo, packet.Key.TrimEnd('\n'));
            aesKey.DataSize = aesKey.RSAEncryptedData.Length;
            ((AuthSession)session).UsesAes = true;
            ((AuthSession)session).AesInfo = aesInfo;

            DataClasses.Server.ClientSockets.SendPacket(session, aesKey);
        }
        #endregion
    }
    #endregion
}
