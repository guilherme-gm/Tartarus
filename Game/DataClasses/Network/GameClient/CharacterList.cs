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
using Common.DataClasses.Network;
using Game.DataClasses.Lobby;
using System;
using System.IO;

namespace Game.DataClasses.Network.GameClient
{
    public class CharacterList : Packet
    {
        public uint CurrentServerTime { get; set; }
        public ushort LastLoginIndex { get; set; }
        public ushort Count { get; set; }
        public LobbyCharacterInfo[] Characters { get; set; }

        public CharacterList()
        {
            this.Id = (ushort)GameClientPackets.CharacterList;
        }

        public override void Read(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override byte[] Write()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            // Writes a fake header
            writer.Write(new byte[HeaderSize]);

            // Write packet body
            writer.Write(this.CurrentServerTime);
            writer.Write(this.LastLoginIndex);
            writer.Write(this.Count);

            foreach (LobbyCharacterInfo info in this.Characters)
            {
                // Model Info
                writer.Write(info.ModelInfo.Sex);
                writer.Write(info.ModelInfo.Race);
                for (int i = 0; i < 5; i++)
                    writer.Write(info.ModelInfo.ModelId[i]);
                writer.Write(info.ModelInfo.TextureId);
                for (int i = 0; i < 24; i++)
                    writer.Write(info.ModelInfo.WearInfo[i]);

                // LobbyCharacterInfo
                writer.Write(info.Level);
                writer.Write(info.Job);
                writer.Write(info.JobLevel);
                writer.Write(info.ExpPercentage);
                writer.Write(info.Hp);
                writer.Write(info.Mp);
                writer.Write(info.Permission);
                writer.Write(info.Banned);
                this.WriteString(writer, info.Name, 19);
                writer.Write(info.SkinColor);
                this.WriteString(writer, info.CreateTime, 30);
                this.WriteString(writer, "9999/31/12 00:00:00", 30);
                for (int i = 0; i < 24; i++)
                    writer.Write(info.WearItemEnhanceInfo[i]);
                for (int i = 0; i < 24; i++)
                    writer.Write(info.WearItemLevelInfo[i]);
                for (int i = 0; i < 24; i++)
                    writer.Write(info.WearItemElementalType[i]);
            }

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }

    }
}
