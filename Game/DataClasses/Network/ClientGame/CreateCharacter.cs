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
using Common.DataClasses.Network;
using Game.DataClasses.Lobby;
using System.IO;

namespace Game.DataClasses.Network.ClientGame
{
    #region Create Character Packet
    public class CreateCharacter : Packet
    {
        #region Get/Set
        public LobbyCharacterInfo Character { get; set; }

        public enum ResultCode : ushort
        {
            Success = 0x0,
        }
        #endregion

        #region Read/Write
        public override void Read(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            base.Read(br);

            this.Character = new LobbyCharacterInfo();
            // Model Info
            this.Character.ModelInfo.Sex = br.ReadInt32();
            this.Character.ModelInfo.Race = br.ReadInt32();
            for (int i = 0; i < 5; i++)
                this.Character.ModelInfo.ModelId[i] = br.ReadInt32();
            this.Character.ModelInfo.TextureId = br.ReadInt32();
            for (int i = 0; i < 24; i++)
                this.Character.ModelInfo.WearInfo[i] = br.ReadInt32();

            // LobbyCharacterInfo
            this.Character.Level = br.ReadInt32();
            this.Character.Job = br.ReadInt32();
            this.Character.JobLevel = br.ReadInt32();
            this.Character.ExpPercentage = br.ReadInt32();
            this.Character.Hp = br.ReadInt32();
            this.Character.Mp = br.ReadInt32();
            this.Character.Permission = br.ReadInt32();
            this.Character.Banned = br.ReadBoolean();
            this.Character.Name = this.ReadString(br, 19);
            this.Character.SkinColor = br.ReadUInt32();
            this.Character.CreateTime = this.ReadString(br, 30);
            this.Character.DeleteTime = this.ReadString(br, 30);
            for (int i = 0; i < 24; i++)
                this.Character.WearItemEnhanceInfo[i] = br.ReadInt32();
            for (int i = 0; i < 24; i++)
                this.Character.WearItemLevelInfo[i] = br.ReadInt32();
            for (int i = 0; i < 24; i++)
                this.Character.WearItemElementalType[i] = br.ReadByte();
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #endregion
}
