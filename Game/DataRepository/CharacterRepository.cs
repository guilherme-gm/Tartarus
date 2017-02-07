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
using Game.DataClasses;
using Game.DataClasses.Database;
using Game.DataClasses.GameWorld;
using Game.DataClasses.Objects;
using Game.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace Game.DataRepository
{
    #region Character Repository
    public class CharacterRepository
    {
        public CharacterRepository()
        {
        }

        #region Get Character
        public bool LoadCharacter(Player player)
        {
            IConnectionFactory conFactory = ConnectionFactory.Instance;
            MySqlConnection con = (MySqlConnection)conFactory.GetConnection();

            try
            {
                #region Loads Character Info
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Character_Load";
                    cmd.Parameters.AddWithValue("accountId", player.User.AccountId);
                    cmd.Parameters.AddWithValue("characterName", player.Name);

                    using (DbDataReader charReader = cmd.ExecuteReader())
                    {
                        if (charReader.Read())
                        {
                            int i = 0;
                            #region Map Character Info
                            player.CharacterId = charReader.GetInt64(i++);
                            player.Name = charReader.GetString(i++);
                            player.PartyId = charReader.GetInt32(i++);
                            player.GuildId = charReader.GetInt32(i++);
                            i++; // Slot
                            player.Position = new Position()
                            {
                                X = (float)charReader.GetInt32(i++),
                                Y = (float)charReader.GetInt32(i++),
                                Z = 0,
                                Layer = (byte)charReader.GetByte(i++),
                            };
                            player.RespawnPoint = new Position()
                            {
                                X = (float)charReader.GetInt32(i++),
                                Y = (float)charReader.GetInt32(i++)
                            };
                            player.Race = charReader.GetInt32(i++);
                            player.Sex = charReader.GetInt32(i++);
                            player.Level = charReader.GetInt32(i++);
                            player.MaxReachedLevel = charReader.GetInt32(i++);
                            player.Exp = charReader.GetInt64(i++);
                            player.LastDecreasedExp = charReader.GetInt64(i++);
                            player.HP = charReader.GetInt32(i++);
                            player.MP = charReader.GetInt32(i++);
                            player.Stamina = charReader.GetInt32(i++);
                            player.Havoc = charReader.GetInt32(i++);
                            player.Job = JobBase.Get(charReader.GetInt32(i++));
                            player.JobLevel = charReader.GetInt32(i++);
                            player.JP = charReader.GetInt32(i++);
                            player.TotalJP = charReader.GetInt32(i++);
                            player.PrevJobs = new JobBase[Creature.MaxPrevJobs];
                            player.PrevJobs[0] = JobBase.Get(charReader.GetInt32(i++));
                            player.PrevJobs[1] = JobBase.Get(charReader.GetInt32(i++));
                            player.PrevJobs[2] = JobBase.Get(charReader.GetInt32(i++));
                            player.PrevJobLevel = new int[Creature.MaxPrevJobs];
                            player.PrevJobLevel[0] = charReader.GetInt32(i++);
                            player.PrevJobLevel[1] = charReader.GetInt32(i++);
                            player.PrevJobLevel[2] = charReader.GetInt32(i++);
                            player.HuntaholicPoints = charReader.GetInt32(i++);
                            player.HuntaholicEnterCount = charReader.GetInt32(i++);
                            player.Gold = charReader.GetInt64(i++);
                            player.Chaos = charReader.GetInt32(i++);
                            player.SkinColor = (uint) charReader.GetValue(i++);
                            player.BaseModel = new int[5];
                            player.BaseModel[0] = charReader.GetInt32(i++);
                            player.BaseModel[1] = charReader.GetInt32(i++);
                            player.BaseModel[2] = charReader.GetInt32(i++);
                            player.BaseModel[3] = charReader.GetInt32(i++);
                            player.BaseModel[4] = charReader.GetInt32(i++);
                            player.FaceTextureId = charReader.GetInt32(i++);
                            i += 6; // Belt
                            i += 6; // Summon
                            i += 3; // Summon info
                            i += 5; // Other info
                            player.ClientInfo = charReader.GetString(i++);
                            #endregion
                        }
                    }
                }
                #endregion

                #region Loads Inventory
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Character_ItemList";
                    cmd.Parameters.AddWithValue("characterId", player.CharacterId);

                    using (DbDataReader itemReader = cmd.ExecuteReader())
                    {
                        while (itemReader.Read())
                        {
                            int i = 0;
                            Item item = Item.Create(itemReader.GetInt32(i++));

                            #region Map Item Info
                            item.Id = (ulong)itemReader.GetValue(i++);
                            item.Idx = itemReader.GetInt32(i++);
                            item.Amount = (ulong)itemReader.GetValue(i++);
                            item.Level = itemReader.GetByte(i++);
                            item.Enhance = itemReader.GetByte(i++);
                            item.Durability = itemReader.GetInt32(i++);
                            item.Endurance = (uint)itemReader.GetValue(i++);
                            item.Flag = itemReader.GetInt32(i++);
                            item.EquipPosition = (ItemBase.EquipPosition)itemReader.GetInt16(i++);

                            for (int j = 0; j < Item.MaxSockets; ++j)
                                item.Socket[j] = itemReader.GetInt32(i++);

                            item.RemainTime = itemReader.GetInt32(i++);
                            item.ElementalEffectType = itemReader.GetByte(i++);
                            item.ElementalEffectExpireTime = itemReader.GetDateTime(i++);
                            item.ElementalEffectAttackPoint = itemReader.GetInt32(i++);
                            item.ElementalMagicPoint = itemReader.GetInt32(i++);
                            item.CreateTime = itemReader.GetDateTime(i++);
                            item.UpdateTime = itemReader.GetDateTime(i++);
                            #endregion

                            player.inventory.Add(item);
                            if (item.EquipPosition != ItemBase.EquipPosition.None)
                            {
                                player.EquippedItems[(int)item.EquipPosition] = item;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ConsoleUtils.ShowSQL("{0} (At: {1})", ex.Message, "CharacterRepository.LoadCharacter()");
                return false;
            }
            finally
            {
                conFactory.Close(con);
            }

            return true;
        }
        #endregion
    }
    #endregion
}
