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
                        if (charReader.Read())
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

        public void CreateCharacter(LobbyCharacterInfo character)
        {

        }

        public bool DeleteCharacter(int charId)
        {
            return false;
        }
    }
    #endregion
}
