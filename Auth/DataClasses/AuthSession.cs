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
using System.Net.Sockets;
using Common.DataClasses;

namespace Auth.DataClasses
{
    #region AuthSession
    public class AuthSession : Session
    {
        #region Get/Set
        public bool UsesAes { get; set; }

        public byte[] AesInfo { get; set; }
        #endregion

        #region Constructor
        public AuthSession(Socket socket, string cipherKey = "") : base(socket, cipherKey)
        {
        }
        #endregion
    }
    #endregion
}
