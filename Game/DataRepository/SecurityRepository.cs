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
using Game.DataClasses.Lobby;
using Game.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Game.DataRepository
{
    #region Security Repository
    public class SecurityRepository
    {
        public SecurityRepository()
        {
        }

        #region Get Code(accountId)
        public string GetCode(int accountId)
        {
            IConnectionFactory conFactory = ConnectionFactory.Instance;
            MySqlConnection con = (MySqlConnection)conFactory.GetConnection();
            string code = "";

            try
            {
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Security_GetCode";
                    cmd.Parameters.AddWithValue("accountId", accountId);
                    code = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                ConsoleUtils.ShowSQL("{0} (At: {1})", ex.Message, "SecurityRepository.GetCode()");
                code = "";
            }
            finally
            {
                conFactory.Close(con);
            }

            return code;
        }
        #endregion
    }
    #endregion
}
