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
namespace Common.DataClasses.Network
{
    /// <summary>
    /// Packets sent by Game server to Auth Server
    /// </summary>
    public enum GameAuthPackets : ushort
    {
        Login = 0x1000,  // 4096
    }

    /// <summary>
    /// Packets sent by Auth server to Game Server
    /// </summary>
    public enum AuthGamePackets : ushort
    {
        GameLoginResult = 0x1001, // 4097
    }
}