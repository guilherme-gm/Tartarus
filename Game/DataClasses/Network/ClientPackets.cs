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
        Login = 0x0001,             // 1
        TimeSync = 0x0002,          // 2
        MoveRequest = 0x0005,       // 5
        RegionUpdate = 0x0007,      // 7
        ChatRequest = 0x0014,       // 20
        ReturnLobby = 0x0017,       // 23
        RequestReturnLobby = 0x0019,// 25
        Version = 0x0032,           // 50
        ChangeLocation = 0x0384,    // 900
        CharacterList = 0x07D1,     // 2001
        CreateCharacter = 0x07D2,   // 2002
        DeleteCharacter = 0x07D3,   // 2003
        AccountWithAuth = 0x07D5,   // 2005
        CheckCharacterName = 0x07D6,// 2006
        SystemSpecs = 0x1F40,       // 8000
        SecurityNo = 0x232D,        // 9005
        Unknown = 0x270F,           // 9999
    }

    public enum GameClientPackets : ushort
    {
        Result = 0x0000,            // 0
        TimeSync = 0x0002,          // 2
        Enter = 0x0003,             // 3
        LoginResult = 0x0004,       // 4
        Move = 0x0008,              // 8
        RegionAck = 0x000B,         // 11
        ChatLocal = 0x0015,         // 21
        Chat = 0x0016,              // 22
        WearInfo = 0x00CA,          // 202
        Inventory = 0x00CF,         // 207
        BeltSlotInfo = 0x00D8,      // 216
        StatusChange = 0x01F4,      // 500
        Property = 0x01FB,          // 507
        QuestList = 0x0258,         // 600
        ChangeLocation = 0x0385,    // 901
        WeatherInfo = 0x0386,       // 902
        StatInfo = 0x03E8,          // 1000
        GoldUpdate = 0x03E9,        // 1001
        LevelUpdate = 0x03EA,       // 1002
        ExpUpdate = 0x03EB,         // 1003
        GameTime = 0x044D,          // 1101
        CharacterList = 0x07D4,     // 2004
        OpenUrl = 0x2328,           // 9000
        UrlList = 0x2329,           // 9001
        RequestSecurityNo = 0x232C, // 9003
    }
}
