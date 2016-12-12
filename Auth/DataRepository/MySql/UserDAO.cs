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
using Auth.DataClasses;
using Auth.Utils;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Auth.DataRepository.MySql
{
	public class UserDAO : IUserDAO
	{
        private IConnectionFactory ConFactory;

        public UserDAO()
        {
            ConFactory = ConnectionFactory.Instance;
        }

		public User Select(string id, string password)
		{
            User user = null;

            MySqlConnection con = (MySqlConnection) ConFactory.GetConnection();

            MySqlCommand command = con.CreateCommand();
            command.CommandText = "SELECT `account_id`, `userid`, `permission`, `last_serverid` FROM Login WHERE `userid` = @id AND `password` = @password";
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("password", password);

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User();
                    user.AccountId = reader.GetInt32(0);
                    user.UserId = reader.GetString(1);
                    user.Permission = reader.GetInt32(2);
                    user.LastServerId = reader.GetInt32(3);
                }
            }

            ConFactory.Close(con);

            return user;
		}

	}

}

