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
using Game.DataClasses;
using Game.DataClasses.GameWorld;
using Game.DataClasses.Lobby;
using Game.DataRepository;
using System.Collections.Generic;
using GC = Game.DataClasses.Network.GameClient;
using System;
using Game.DataClasses.Database;

namespace Game.DataClasses
{
	public class Player : Creature
	{
        #region Object Creation
        private static uint LastUsedHandle = 0x0806;
        private static Stack<uint> HandlePool = new Stack<uint>();
        
        public static Player Create(User user, string name)
        {
            uint gid;
            
            // Locks this pool so other threads can't access it
            // during the handle retrieving
            lock(HandlePool)
            {
                if (HandlePool.Count > 0)
                {
                    gid = HandlePool.Pop();
                }
                else
                {
                    gid = LastUsedHandle++;
                }
            }

            Player player = new Player(gid, user, name);
            return player;
        }
        #endregion

        public User User { get; set; }
        
        public Inventory inventory { get; set; }
        
		public State[] state { get; set; }
        
        public long CharacterId { get; set; }

        public string Name { get; set; }

        // TODO : Maybe this should be changed by a Party structure?
        public int PartyId { get; set; }
        
        public int GuildId { get; set; }

        public Position RespawnPoint { get; set; }

        public int Sex { get; set; }

        public int HuntaholicEnterCount { get; set; }

        public int HuntaholicPoints { get; set; }

        public long Gold { get; set; }

        public int Chaos { get; set; }

        public int[] BaseModel { get; set; }

        public uint SkinColor { get; set; }

        public int FaceTextureId { get; set; }
        
        public string ClientInfo { get; set; }
        
        private Player(uint gid, User user, string name) : base(gid)
        {
            this.User = user;
            this.Name = name;
            this.inventory = new Inventory();
            this.Position = new Position();
        }

        public bool Load()
        {
            // Loads character base
            CharacterRepository repo = new CharacterRepository();
            if (!repo.LoadCharacter(this))
                return false;

            // TODO : Loads character inventory

            CalculateStats();
            CalculateStatsByState();

            return true;
        }

        public void CalculateStats()
        {
            this.Attributes = new CreatureAttribute();

            // Loads character base stat info
            this.Stats.LoadBase();

            // Get level bonus stats
            for (int i = 0; i < PrevJobs.Length; ++i)
            {
                if (PrevJobs[i] != null)
                    CalcuteJobStats(PrevJobs[i], PrevJobLevel[i]);
            }

            // TODO : Get equipped item stats

            // Calculate Attributes
            // TODO : Does equipment stats really affects attributes calc?
            CalculateAttributes();

            GC.StatInfo statInfo = new GC.StatInfo()
            {
                Handle = this.GID,
                Stat = this.Stats,
                Attribute = this.Attributes,
                Type = 0
            };
            Server.ClientSockets.SendPacket(this.User._Session, statInfo);
        }

        private void CalculateAttributes()
        {
            // Formulas from: http://rappelz.wikia.com/wiki/Stats_%26_Ability#Formulae

            // Make temporary variables to improve readability
            CreatureStat st = this.Stats;
            CreatureAttribute at = this.Attributes;

            this.MaxHp += 33 * st.Vitality + 20 * this.Level;
            this.MaxMP += 30 * st.Intelligence + 20 * this.Level;

            // TODO : Left Hand
            //if (IsRanged) {
            //at.AttackPointRight += (short)((6 / 5f) * this.Stats.Agility + (11 / 5f) * this.Stats.Dexterity + this.Level);
            //} else {
            at.AttackPointRight += (short)((14 / 5f) * st.Strength + this.Level + 9);
            //}

            // TODO : Left Hand
            at.AccuracyRight += (short)((1 / 2f) * st.Dexterity + Level);
            at.MagicPoint += (short)(2 * st.Intelligence + this.Level);
            at.Defence += (short)((5 / 3f) * st.Vitality + this.Level);
            at.Avoid += (short)((1 / 2f) * st.Agility + this.Level);
            at.AttackSpeed += (short)(100 + (1 / 10f) * st.Agility);
            at.MagicAccuracy += (short)((4 / 10f) * st.Wisdom + (1 / 10f) * st.Dexterity + this.Level);
            at.MagicDefence += (short)(2 * st.Wisdom + this.Level);
            at.MagicAvoid += (short)((1 / 2f) * st.Wisdom + this.Level);
            at.MoveSpeed += 120;
            at.HPRegenPercentage += 5;
            at.MPRegenPercentage += 5;
            //at.BlockChance += 0;
            at.Critical += (short)((1 / 5f) * st.Luck + 3);
            at.CastingSpeed += 100;
            at.HPRegenPoint += (short)(2 * this.Level + 48);
            at.MPRegenPoint += (short)(4.1 * st.Wisdom + 2 * this.Level + 48);
            //at.BlockDefence += 0;
            at.CriticalPower += 80;
            at.CoolTimeSpeed += 100;
            at.MaxWeight += (short)(10 * st.Strength + 10 * this.Level);
            
            this.Attributes = at;
        }

        private void CalcuteJobStats(JobBase job, int level)
        {
            JobLevelBonus bonus = JobLevelBonus.Get(job.Id);
            if (bonus == null)
                return;
            int level1, level2 = 0, level3 = 0;

            if (this.JobLevel > 20)
            {
                level1 = 20;
                if (this.JobLevel > 40)
                {
                    level2 = 20;
                    level3 = this.JobLevel - 40;
                }
                else
                {
                    level2 = this.JobLevel - 20;
                }
            }
            else
            {
                level1 = this.JobLevel;
            }

            this.Stats.Strength += (short)(bonus.Str1 * level1 + bonus.Str2 * level2 + bonus.Str3 * level3);
            this.Stats.Agility += (short)(bonus.Agi1 * level1 + bonus.Agi2 * level2 + bonus.Agi3 * level3);
            this.Stats.Dexterity += (short)(bonus.Dex1 * level1 + bonus.Dex2 * level2 + bonus.Dex3 * level3);
            this.Stats.Intelligence += (short)(bonus.Int1 * level1 + bonus.Int2 * level2 + bonus.Int3 * level3);
            this.Stats.Luck += (short)(bonus.Luk1 * level1 + bonus.Luk2 * level2 + bonus.Luk3 * level3);
            this.Stats.Wisdom += (short)(bonus.Wis1 * level1 + bonus.Wis2 * level2 + bonus.Wis3 * level3);
        }

        public void CalculateStatsByState()
        {
            // Reset stats to 0
            this.StatsByState = new CreatureStat();

            // TODO : Get equipped item stats

            // TODO : Calculate Attributes

            GC.StatInfo statInfo = new GC.StatInfo()
            {
                Handle = this.GID,
                Stat = this.StatsByState,
                Attribute = this.AttributesByState,
                Type = 0
            };
            Server.ClientSockets.SendPacket(this.User._Session, statInfo);
        }
	}

}

