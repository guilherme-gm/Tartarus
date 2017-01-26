
using System;
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
using FileHelpers;

namespace Game.DataClasses.Database.Records
{
    /// <summary>
    /// This class represents a record of ItemBase on CSV files.
    /// </summary>
    [DelimitedRecord(","), IgnoreFirst(1)]
    public class ItemBaseRecord
    {
        #region Columns
        public int ID;
        public string DummyName;
        public int NameId;
        public int Type;
        public int Group;
        public ItemBase.ItemClass Class;
        public ItemBase.EquipPosition WearType;
        public int SetId;
        public int SetPart;
        public byte Grade;
        public int Rank;
        public int Level;
        public int Enhance;
        public int Sockets;
        public int StatusFlag;
        public int UseRace;
        public int UseClass;
        public int MinLevel;
        public int MaxLevel;
        public int TargetMinLevel;
        public int TargetMaxLevel;
        public float Range;
        public float Weight;
        public int Price;
        public int HuntaholicPoint;
        public int Durability;
        public int Endurance;
        public int Material;
        public int SummonId;
        public int ItemUseFlag;
        public int AvailablePeriod;
        public int DecreaseType;
        public float ThrowRange;
        public ItemBase.BaseEffect Base1Type;
        public double Base1Var1;
        public double Base1Var2;
        public ItemBase.BaseEffect Base2Type;
        public double Base2Var1;
        public double Base2Var2;
        public ItemBase.BaseEffect Base3Type;
        public double Base3Var1;
        public double Base3Var2;
        public ItemBase.BaseEffect Base4Type;
        public double Base4Var1;
        public double Base4Var2;
        public short Opt1Type;
        public double Opt1Var1;
        public double Opt1Var2;
        public short Opt2Type;
        public double Opt2Var1;
        public double Opt2Var2;
        public short Opt3Type;
        public double Opt3Var1;
        public double Opt3Var2;
        public short Opt4Type;
        public double Opt4Var1;
        public double Opt4Var2;
        public int EffectId;
        public short Enhance1Id;
        public float Enhance1Val1;
        public float Enhance1Val2;
        public float Enhance1Val3;
        public float Enhance1Val4;
        public short Enhance2Id;
        public float Enhance2Val1;
        public float Enhance2Val2;
        public float Enhance2Val3;
        public float Enhance2Val4;
        public int SkillId;
        public int StateId;
        public int StateLevel;
        public int StateTime;
        public byte StateType;
        public int CoolTime;
        public short CoolTimeGroup;
        public string Script;
        #endregion
    }
}
