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
using Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Game.DataClasses.Database
{
    public class ItemBase
    {
        public const int MaxBaseType = 4;
        public const int MaxOptType = 4;
        public const int MaxEnhanceType = 2;

        public static Dictionary<int, ItemBase> Db;

        #region Database Loading
        public static void Load()
        {
            if (Db != null)
            {
                ConsoleUtils.ShowFatalError("Trying to load already loaded Item Database. Load Aborted.");
                return;
            }

            Db = new Dictionary<int, ItemBase>();
            StringUtils.ReadDatabase("Database/ItemDatabase.csv", 76, ReadEntry, false, true);
        }

        private static void ReadEntry(string fileName, int lineNum, string[] columns, string[] values, bool allowReplace)
        {
            // Read Entry
            ItemBase item = new ItemBase();
            int j = 0;
            try
            {
                item.ID = int.Parse(values[j++]);
                j++; // DummyName
                item.NameId = int.Parse(values[j++]);
                item.Type = int.Parse(values[j++]);
                item.Group = int.Parse(values[j++]);
                item.Class = int.Parse(values[j++]);
                item.WearType = int.Parse(values[j++]);
                item.SetId = int.Parse(values[j++]);
                item.SetPart = int.Parse(values[j++]);
                item.Grade = byte.Parse(values[j++]);
                item.Rank = int.Parse(values[j++]);
                item.Level = int.Parse(values[j++]);
                item.Enhance = int.Parse(values[j++]);
                item.Sockets = int.Parse(values[j++]);
                item.StatusFlag = int.Parse(values[j++]);
                item.UseRace = int.Parse(values[j++]);
                item.UseClass = int.Parse(values[j++]);
                item.MinLevel = int.Parse(values[j++]);
                item.MaxLevel = int.Parse(values[j++]);
                item.TargetMinLevel = int.Parse(values[j++]);
                item.TargetMaxLevel = int.Parse(values[j++]);
                item.Range = float.Parse(values[j++]);
                item.Weight = float.Parse(values[j++]);
                item.Price = int.Parse(values[j++]);
                item.HuntaholicPoint = int.Parse(values[j++]);
                item.Durability = int.Parse(values[j++]);
                item.Endurance = int.Parse(values[j++]);
                item.Material = int.Parse(values[j++]);
                item.SummonId = int.Parse(values[j++]);
                item.ItemUseFlag = int.Parse(values[j++]);
                item.AvailablePeriod = int.Parse(values[j++]);
                item.DecreaseType = int.Parse(values[j++]);
                item.ThrowRange = float.Parse(values[j++]);
                item.BaseType = new short[MaxBaseType];
                item.BaseVar1 = new double[MaxBaseType];
                item.BaseVar2 = new double[MaxBaseType];
                for (int k = 0; k < MaxBaseType; ++k) {
                    item.BaseType[k] = short.Parse(values[j++]);
                    item.BaseVar1[k] = double.Parse(values[j++]);
                    item.BaseVar2[k] = double.Parse(values[j++]);
                }
                item.OptType = new short[MaxOptType];
                item.OptVar1 = new double[MaxOptType];
                item.OptVar2 = new double[MaxOptType];
                for (int k = 0; k < MaxOptType; ++k) {
                    item.OptType[k] = short.Parse(values[j++]);
                    item.OptVar1[k] = double.Parse(values[j++]);
                    item.OptVar2[k] = double.Parse(values[j++]);
                }
                item.EffectId = int.Parse(values[j++]);
                item.EnhanceId = new short[MaxEnhanceType];
                item.EnhanceVal1 = new float[MaxEnhanceType];
                item.EnhanceVal2 = new float[MaxEnhanceType];
                item.EnhanceVal3 = new float[MaxEnhanceType];
                item.EnhanceVal4 = new float[MaxEnhanceType];
                for (int k = 0; k < MaxEnhanceType; ++k) {
                    item.EnhanceId[k] = short.Parse(values[j++]);
                    item.EnhanceVal1[k] = float.Parse(values[j++]);
                    item.EnhanceVal2[k] = float.Parse(values[j++]);
                    item.EnhanceVal3[k] = float.Parse(values[j++]);
                    item.EnhanceVal4[k] = float.Parse(values[j++]);
                }
                item.SkillId = int.Parse(values[j++]);
                item.StateId = int.Parse(values[j++]);
                item.StateLevel = int.Parse(values[j++]);
                item.StateTime = int.Parse(values[j++]);
                item.StateType = byte.Parse(values[j++]);
                item.CoolTime = int.Parse(values[j++]);
                item.CoolTimeGroup = short.Parse(values[j++]);
                item.Script = values[j++];
            }
            catch (Exception)
            {
                ConsoleUtils.ShowError("Could not parse column '{0}' in '{1}' at line '{2}'. Skipping line.", columns[j - 1], fileName, lineNum);
                return;
            }

            // Inserts entry in Database
            if (Db.ContainsKey(item.ID))
            {
                if (!allowReplace)
                {
                    ConsoleUtils.ShowError("Duplicated code detected in '{0}' at line '{1}'. Skipping entry.", fileName, lineNum);
                    return;
                }
                Db[item.ID] = item;
            }
            else
            {
                Db.Add(item.ID, item);
            }
        }
        #endregion

        #region Database Commands
        public static ItemBase Get(int itemId)
        {
            ItemBase item;
            if (!Db.TryGetValue(itemId, out item))
            {
                return null;
            }
            return item;
        }
        #endregion

        #region Properties
        public int ID { get; private set; }
        public int NameId { get; private set; }
        public int Type { get; private set; }
        public int Group { get; private set; }
        public int Class { get; private set; }
        public int WearType { get; private set; }
        public int SetId { get; private set; }
        public int SetPart { get; private set; }
        public byte Grade { get; private set; }
        public int Rank { get; private set; }
        public int Level { get; private set; }
        public int Enhance { get; private set; }
        public int Sockets { get; private set; }
        public int StatusFlag { get; private set; }
        public int UseRace { get; private set; }
        public int UseClass { get; private set; }
        public int MinLevel { get; private set; }
        public int MaxLevel { get; private set; }
        public int TargetMinLevel { get; private set; }
        public int TargetMaxLevel { get; private set; }
        public float Range { get; private set; }
        public float Weight { get; private set; }
        public int Price { get; private set; }
        public int HuntaholicPoint { get; private set; }
        public int Durability { get; private set; }
        public int Endurance { get; private set; }
        public int Material { get; private set; }
        public int SummonId { get; private set; }
        public int ItemUseFlag { get; private set; }
        public int AvailablePeriod { get; private set; }
        public int DecreaseType { get; private set; }
        public float ThrowRange { get; private set; }
        public short[] BaseType { get; private set; }
        public double[] BaseVar1 { get; private set; }
        public double[] BaseVar2 { get; private set; }
        public short[] OptType { get; private set; }
        public double[] OptVar1 { get; private set; }
        public double[] OptVar2 { get; private set; }
        public int EffectId { get; private set; }
        public short[] EnhanceId { get; private set; }
        public float[] EnhanceVal1 { get; private set; }
        public float[] EnhanceVal2 { get; private set; }
        public float[] EnhanceVal3 { get; private set; }
        public float[] EnhanceVal4 { get; private set; }
        public int SkillId { get; private set; }
        public int StateId { get; private set; }
        public int StateLevel { get; private set; }
        public int StateTime { get; private set; }
        public byte StateType { get; private set; }
        public int CoolTime { get; private set; }
        public short CoolTimeGroup { get; private set; }
        public string Script { get; private set; }
        #endregion


    }
}
