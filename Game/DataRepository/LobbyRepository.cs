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
using Game.DataClasses.Database;
using Game.DataClasses.Lobby;
using Game.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Game.DataRepository
{
    #region Lobby Repository
    public class LobbyRepository
    {
        public LobbyRepository()
        {

        }

        #region Get Character List(Account ID)
        public LobbyCharacterInfo[]  GetCharacterList(int accountId)
        {
            IConnectionFactory conFactory = ConnectionFactory.Instance;
            MySqlConnection conCharacters = (MySqlConnection)conFactory.GetConnection();
            MySqlConnection conItems = (MySqlConnection)conFactory.GetConnection();
            List<LobbyCharacterInfo> characters = new List<LobbyCharacterInfo>();

            using (MySqlCommand charCommand = conCharacters.CreateCommand())
            {
                using (MySqlCommand itemCommand = conItems.CreateCommand())
                {
                    charCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    charCommand.CommandText = "usp_Lobby_GetCharacterList";
                    charCommand.Parameters.AddWithValue("accountId", accountId);

                    itemCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    itemCommand.CommandText = "usp_Lobby_GetCharacterEquipment";
                    itemCommand.Parameters.AddWithValue("characterId", 0);

                    using (DbDataReader charReader = charCommand.ExecuteReader())
                    {
                        while (charReader.Read())
                        {
                            LobbyCharacterInfo character = new LobbyCharacterInfo();
                            itemCommand.Parameters["characterId"].Value = charReader.GetInt64(0);

                            #region Character Info
                            character.Name = charReader.GetString(1);
                            character.ModelInfo.Race = charReader.GetInt32(2);
                            character.ModelInfo.Sex = charReader.GetInt32(3);
                            character.Level = charReader.GetInt32(4);
                            character.ExpPercentage = 0;//reader.GetInt32(5); // TODO : Is this really the exp?
                            character.Hp = charReader.GetInt32(6);
                            character.Mp = charReader.GetInt32(7);
                            character.Job = charReader.GetInt32(8);
                            character.JobLevel = charReader.GetInt32(9);
                            character.SkinColor = (UInt32)charReader.GetValue(10);
                            character.ModelInfo.ModelId[0] = charReader.GetInt32(11);
                            character.ModelInfo.ModelId[1] = charReader.GetInt32(12);
                            character.ModelInfo.ModelId[2] = charReader.GetInt32(13);
                            character.ModelInfo.ModelId[3] = charReader.GetInt32(14);
                            character.ModelInfo.ModelId[4] = charReader.GetInt32(15);
                            character.ModelInfo.TextureId = charReader.GetInt32(16);
                            character.CreateTime = charReader.GetDateTime(17).ToString();
                            character.LoginTime = charReader.GetDateTime(18);
                            #endregion

                            #region Equip Info
                            using (DbDataReader itemReader = itemCommand.ExecuteReader())
                            {
                                while (itemReader.Read())
                                {
                                    int pos = itemReader.GetInt32(0);
                                    character.ModelInfo.WearInfo[pos] = itemReader.GetInt32(1);
                                    character.WearItemLevelInfo[pos] = itemReader.GetInt32(2);
                                    character.WearItemEnhanceInfo[pos] = itemReader.GetInt32(3);
                                    character.WearItemElementalType[pos] = itemReader.GetByte(4);
                                }
                            }
                            #endregion

                            characters.Add(character);
                        }
                    }
                }

            }

            conFactory.Close(conCharacters);
            conFactory.Close(conItems);

            return characters.ToArray();
        }
        #endregion

        #region Name Exists(name)
        public bool NameExists(string name)
        {
            IConnectionFactory conFactory = ConnectionFactory.Instance;
            MySqlConnection con = (MySqlConnection)conFactory.GetConnection();
            bool exists = true;

            try
            {
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Lobby_NameExists";
                    cmd.Parameters.AddWithValue("cName", name);
                    exists = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                ConsoleUtils.ShowSQL("{0} (At: {1})", ex.Message, "LobbyRepository.NameExists()");
                exists = true;
            }
            finally
            {
                conFactory.Close(con);
            }

            return exists;
        }
        #endregion
        
        #region Create Character(Account ID, Character Info)
        public bool CreateCharacter(int accountId, LobbyCharacterInfo character, ItemBase[] startItems, int startX, int startY)
        {
            IConnectionFactory conFactory = ConnectionFactory.Instance;
            MySqlConnection con = (MySqlConnection)conFactory.GetConnection();
            long id = -1;
            
            using (MySqlTransaction tran = con.BeginTransaction())
            {
                try
                {
                    using (MySqlCommand command = con.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "usp_Lobby_CreateCharacter";
                        command.Parameters.AddWithValue("accountId", accountId);
                        command.Parameters.AddWithValue("cName", character.Name);
                        command.Parameters.AddWithValue("race", character.ModelInfo.Race);
                        command.Parameters.AddWithValue("sex", character.ModelInfo.Sex);
                        command.Parameters.AddWithValue("job", character.Job);
                        command.Parameters.AddWithValue("skinColor", character.SkinColor);
                        command.Parameters.AddWithValue("hairId", character.ModelInfo.ModelId[0]);
                        command.Parameters.AddWithValue("faceId", character.ModelInfo.ModelId[1]);
                        command.Parameters.AddWithValue("bodyId", character.ModelInfo.ModelId[2]);
                        command.Parameters.AddWithValue("handsId", character.ModelInfo.ModelId[3]);
                        command.Parameters.AddWithValue("feetId", character.ModelInfo.ModelId[4]);
                        command.Parameters.AddWithValue("textureId", character.ModelInfo.TextureId);
                        command.Parameters.AddWithValue("startX", startX);
                        command.Parameters.AddWithValue("startY", startY);
                        command.Parameters.Add(new MySqlParameter("characterId", MySqlDbType.UInt64) { Direction = System.Data.ParameterDirection.Output });
                        
                        command.ExecuteNonQuery();
                        id = Convert.ToInt64(command.Parameters["characterId"].Value);
                    }

                    using (MySqlCommand command = con.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "usp_Character_InsertItem";
                        command.Parameters.AddWithValue("charId", id);
                        command.Parameters.AddWithValue("iCode", 0);
                        command.Parameters.AddWithValue("position", 0);
                        command.Parameters.AddWithValue("amount", 0);
                        command.Parameters.AddWithValue("iLevel", 0);

                        foreach (ItemBase item in startItems)
                        {
                            command.Parameters["iCode"].Value = item.Id;
                            command.Parameters["position"].Value = item.WearType;
                            command.Parameters["amount"].Value = 1;
                            command.Parameters["iLevel"].Value = item.Level;
                            command.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    ConsoleUtils.ShowSQL("{0} (At: {1})", ex.Message, "LobbyRepository.CreateCharacter");
                    tran.Rollback();
                    return false;
                }
            }

            conFactory.Close(con);

            return true;
        }
        #endregion

        #region Delete Character(Account ID, Character Name)
        public bool DeleteCharacter(int accountId, string characterName)
        {
            IConnectionFactory conFactory = ConnectionFactory.Instance;
            MySqlConnection con = (MySqlConnection)conFactory.GetConnection();
            bool deleted = false;

            try
            {
                using (MySqlCommand command = con.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "usp_Lobby_DeleteCharacter";
                    command.Parameters.AddWithValue("accountId", accountId);
                    command.Parameters.AddWithValue("characterName", characterName);
                    command.ExecuteNonQuery();
                    deleted = true;
                }
            }
            catch (Exception ex)
            {
                ConsoleUtils.ShowSQL("{0} (At: {1})", ex.Message, "LobbyRepository.DeleteCharacter");
                deleted = false;
            }
            finally
            {
                conFactory.Close(con);
            }
            return deleted;
        }
        #endregion
    }
    #endregion
}
