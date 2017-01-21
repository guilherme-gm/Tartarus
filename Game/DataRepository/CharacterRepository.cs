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
                            #region Character Info
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
                            player.PrevJobs = new JobBase[3];
                            player.PrevJobs[0] = JobBase.Get(charReader.GetInt32(i++));
                            player.PrevJobs[1] = JobBase.Get(charReader.GetInt32(i++));
                            player.PrevJobs[2] = JobBase.Get(charReader.GetInt32(i++));
                            player.PrevJobLevel = new int[3];
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
            }
            catch (Exception ex)
            {
                ConsoleUtils.ShowSQL("{0} (At: {1})", ex.Message, "SecurityRepository.GetCode()");
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
