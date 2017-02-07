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
using Game.DataClasses.GameWorld;
using System;
using System.IO;

namespace Game.DataClasses.Network.GameClient
{
    #region Stat Info
    public class StatInfo : Packet
    {
        #region Get/Set
        public uint Handle { get; set; }
        public CreatureStat Stat { get; set; }
        public CreatureAttribute Attribute { get; set; }

        public byte Type { get; set; }
        #endregion

        #region Constructor/Reader/Write
        public StatInfo()
        {
            this.Id = (ushort)GameClientPackets.StatInfo;
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
            writer.Write(this.Handle);

            writer.Write(this.Stat.StatId);
            writer.Write(this.Stat.Strength);
            writer.Write(this.Stat.Vitality);
            writer.Write(this.Stat.Dexterity);
            writer.Write(this.Stat.Agility);
            writer.Write(this.Stat.Intelligence);
            writer.Write(this.Stat.Wisdom);
            writer.Write(this.Stat.Luck);

            writer.Write(this.Attribute.Critical);
            writer.Write(this.Attribute.CriticalPower);
            writer.Write(this.Attribute.AttackPointRight);
            writer.Write(this.Attribute.AttackPointLeft);
            writer.Write(this.Attribute.Defence);
            writer.Write(this.Attribute.BlockDefence);
            writer.Write(this.Attribute.MagicPoint);
            writer.Write(this.Attribute.MagicDefence);
            writer.Write(this.Attribute.AccuracyRight);
            writer.Write(this.Attribute.AccuracyLeft);
            writer.Write(this.Attribute.MagicAccuracy);
            writer.Write(this.Attribute.Avoid);
            writer.Write(this.Attribute.MagicAvoid);
            writer.Write(this.Attribute.BlockChance);
            writer.Write(this.Attribute.MoveSpeed);
            writer.Write(this.Attribute.AttackSpeed);
            writer.Write(this.Attribute.AttackRange);
            writer.Write(this.Attribute.MaxWeight);
            writer.Write(this.Attribute.CastingSpeed);
            writer.Write(this.Attribute.CoolTimeSpeed);
            writer.Write(this.Attribute.ItemChance);
            writer.Write(this.Attribute.HPRegenPercentage);
            writer.Write(this.Attribute.HPRegenPoint);
            writer.Write(this.Attribute.MPRegenPercentage);
            writer.Write(this.Attribute.MPRegenPoint);

            writer.Write(this.Type);

            // finishes packet
            base.Complete(writer);

            return stream.ToArray();
        }
        #endregion
    }
    #endregion
}