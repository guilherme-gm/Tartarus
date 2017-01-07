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

namespace Game.DataClasses.Network
{
    public enum ClientGamePackets : ushort
    {
        Version = 0x0032,           // 50
        CharacterList = 0x07D1,     // 2001
        CreateCharacter = 0x07D2,   // 2002
        AccountWithAuth = 0x07D5,   // 2005
        CheckCharacterName = 0x07D6,// 2006
        SystemSpecs = 0x1F40,       // 8000
        Unknown = 0x270F,           // 9999
    }

    public enum GameClientPackets : ushort
    {
        Result = 0x0000,            // 0
        CharacterList = 0x07D4,     // 2004
    }
}
