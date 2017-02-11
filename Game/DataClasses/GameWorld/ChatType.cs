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
namespace Game.DataClasses.GameWorld
{
    public enum ChatType : byte
    {
        Normal = 0x0,
        Yell = 0x1,
        Adv = 0x2,
        Whisper = 0x3,
        Global = 0x4,
        Emotion = 0x5,
        GM = 0x6,
        GMWhisper = 0x7,
        Party = 0xA,
        Guild = 0xB,
        AttackTeam = 0xC,
        Notice = 0x14,
        Exp = 0x1E,
        Damage = 0x1F,
        Item = 0x20,
        Battle = 0x21,
        Summon = 0x22,
        Etc = 0x23,
        NPC = 0x28,
        Debug = 0x32,
        PartySystem = 0x64,
        GuildSystem = 0x6E,
        QuestSystem = 0x78,
        RaidSystem = 0x82,
        FriendSystem = 0x8C,
        AllianceSystem = 0x96,
        HuntaholicSystem = 0xA0
    }
}
