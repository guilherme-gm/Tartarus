
using Common.RC4;
using System.Net.Sockets;
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
namespace Common.DataClasses
{
	public class NetworkData
	{
        public const int MaxBuffer = 1024;

        public Socket _Socket { get; set; }
        public byte[] Buffer;
        public byte[] Message;
        public int Offset { get; set; }
        public XRC4Cipher InCipher { get; set; }
        public XRC4Cipher OutCipher { get; set; }

        public NetworkData(Socket socket)
        {
            this._Socket = socket;
            this.Buffer = new byte[MaxBuffer];
            this.Message = new byte[MaxBuffer];
        }
    }

}

