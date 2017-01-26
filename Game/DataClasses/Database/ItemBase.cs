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
using FileHelpers;
using Game.DataClasses.Database.Records;
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
            ReadDatabase("Database/ItemDatabase.csv", true, false);
        }

        private static void ReadDatabase(string fileName, bool required, bool allowReplace)
        {
            int i = 0;

            // File exists check
            if (!File.Exists(fileName) && required)
            {
                ConsoleUtils.ShowFatalError("Could not find database file '{0}'.", fileName);
                return;
            }

            // Starts the engine
            var engine = new FileHelperAsyncEngine<ItemBaseRecord>();
            engine.ErrorMode = ErrorMode.SaveAndContinue;
            using (engine.BeginReadFile(fileName))
            {
                // Read entry
                foreach (ItemBaseRecord record in engine)
                {
                    ItemBase item = RecordToEntry(record);

                    // Inserts entry in Database
                    if (Db.ContainsKey(item.Id))
                    {
                        if (!allowReplace)
                        {
                            ConsoleUtils.ShowError("Duplicated Id '{0}' detected in '{1}' at line '{2}'. Skipping entry.", item.Id, fileName, engine.LineNumber);
                            continue;
                        }
                        Db[item.Id] = item;
                    }
                    else
                    {
                        Db.Add(item.Id, item);
                    }
                    i++;
                }

                // Show errors
                foreach (var err in engine.ErrorManager.Errors)
                {
                    ConsoleUtils.ShowError("Could not parse entry in '{0}' at line '{1}'. Skipping line. {2}", fileName, err.LineNumber, err.ExceptionInfo.ToString());
                }
            }

            ConsoleUtils.ShowInfo("'{0}' entries read from '{1}'.", i, fileName);
        }
        
        private static ItemBase RecordToEntry(ItemBaseRecord record)
        {
            ItemBase item = new ItemBase();

            // Map
            item.Id = record.ID;
            item.NameId = record.NameId;
            item.Type = record.Type;
            item.Group = record.Group;
            item.Class = record.Class;
            item.WearType = record.WearType;
            item.SetId = record.SetId;
            item.SetPart = record.SetPart;
            item.Grade = record.Grade;
            item.Rank = record.Rank;
            item.Level = record.Level;
            item.Enhance = record.Enhance;
            item.Sockets = record.Sockets;
            item.StatusFlag = record.StatusFlag;
            item.UseRace = record.UseRace;
            item.UseClass = record.UseClass;
            item.MinLevel = record.MinLevel;
            item.MaxLevel = record.MaxLevel;
            item.TargetMinLevel = record.TargetMinLevel;
            item.TargetMaxLevel = record.TargetMaxLevel;
            item.Range = record.Range;
            item.Weight = record.Weight;
            item.Price = record.Price;
            item.HuntaholicPoint = record.HuntaholicPoint;
            item.Durability = record.Durability;
            item.Endurance = record.Endurance;
            item.Material = record.Material;
            item.SummonId = record.SummonId;
            item.ItemUseFlag = record.ItemUseFlag;
            item.AvailablePeriod = record.AvailablePeriod;
            item.DecreaseType = record.DecreaseType;
            item.ThrowRange = record.ThrowRange;
            item.BaseType = new short[MaxBaseType]
                { record.Base1Type, record.Base2Type, record.Base3Type, record.Base4Type };
            item.BaseVar1 = new double[MaxBaseType]
                { record.Base1Var1, record.Base2Var1, record.Base3Var1, record.Base4Var1 };
            item.BaseVar2 = new double[MaxBaseType]
                { record.Base1Var2, record.Base2Var2, record.Base3Var2, record.Base4Var2 };
            item.OptType = new short[MaxOptType]
                { record.Opt1Type, record.Opt2Type, record.Opt3Type, record.Opt4Type };
            item.OptVar1 = new double[MaxOptType]
                { record.Opt1Var1, record.Opt2Var1, record.Opt3Var1, record.Opt4Var1 };
            item.OptVar2 = new double[MaxOptType]
                { record.Opt1Var2, record.Opt2Var2, record.Opt3Var2, record.Opt4Var2 };
            item.EffectId = record.EffectId;
            item.EnhanceId = new short[MaxEnhanceType]
                { record.Enhance1Id, record.Enhance2Id };
            item.EnhanceVal1 = new float[MaxEnhanceType]
                { record.Enhance1Val1, record.Enhance2Val1 };
            item.EnhanceVal2 = new float[MaxEnhanceType]
                { record.Enhance1Val2, record.Enhance2Val2 };
            item.EnhanceVal3 = new float[MaxEnhanceType]
                { record.Enhance1Val3, record.Enhance2Val3 };
            item.EnhanceVal4 = new float[MaxEnhanceType]
                { record.Enhance1Val4, record.Enhance2Val4 };
            item.SkillId = record.SkillId;
            item.StateId = record.StateId;
            item.StateLevel = record.StateLevel;
            item.StateTime = record.StateTime;
            item.StateType = record.StateType;
            item.CoolTime = record.CoolTime;
            item.CoolTimeGroup = record.CoolTimeGroup;
            item.Script = record.Script;

            return item;
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

        #region Enums
        public enum EquipPosition : short
        {
            None = -1,
            RightHand = 0,
            LeftHand,
            Armor,
            Helm,
            Glove,
            Boots,
            Belt,
            Mantle,
            Armulet,
            Ring,
            // 10 - (Second Ring - skipped)
            Ear = 11,
            Face,
            Hair,
            DecoRightHand,
            DecoLeftHand,
            DecoArmor,
            DecoHelm,
            DecoGlove,
            DecoBoots,
            DecoMantle,
            DecoShoulder,
            Ride,
            Bag,
            TwoFingerRing = 94,
            TwoHand = 99,
            Skill
        }

        public enum ItemClass : int
        {
            Etc = 0,
            DoubleAxe = 95,
            DoubleSword,
            // 97 - Empty
            DoubleDagger = 98,
            EveryWeapon,
            EtcWeapon,
            OneHandSword,
            TwoHandSword,
            Dagger,
            TwoHandSpear,
            TwoHandAxe,
            OneHandMace,
            TwoHandMace,
            HeavyBow,
            LightBow,
            CrossBow,
            OneHandStaff,
            TwoHandStaff,
            OneHandAxe,
            // 114~199 - Empty
            Armor = 200,
            FighterArmor,
            HunterArmor,
            MagicianArmor,
            SummonerArmor,
            // 205~209 - Empty
            Shield = 210,
            // 211~219 - Empty
            Helm = 220,
            // 221~229 - Empty
            Boots = 230,
            // 231~239 - Empty
            Glove = 240,
            // 241~249 - Empty
            Belt = 250,
            // 251~259 - Empty
            Mantle = 260,
            // 261~299 - Empty
            EtcAccessory = 300,
            Mask,
            Cube,
            // 303~399 - Empty
            BoostChip = 400,
            SoulStone,
            // 402~450 - Empty
            EtherealStone = 451,
            // 452~600 - Empty
            DecoShield = 601,
            DecoArmor,
            DecoHelm,
            DecoGlove,
            DecoBoots,
            DecoMantle,
            DecoShoulder,
            DecoHair,
            DecoOneHandSword,
            DecoTwoHandSword,
            DecoDagger,
            DecoTwoHandSpear,
            DecoTwoHandAxe,
            DecoOneHandMace,
            DecoTwoHandMace,
            DecoHeavyBow,
            DecoLightBow,
            DecoCrossbow,
            DecoOneHandStaff,
            DecoTwoHandStaff,
            DecoOneHandAxe
        }
        #endregion

        #region Properties
        public int Id { get; private set; }
        public int NameId { get; private set; }
        public int Type { get; private set; }
        public int Group { get; private set; }
        public ItemClass Class { get; private set; }
        public EquipPosition WearType { get; private set; }
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
