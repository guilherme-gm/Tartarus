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
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Utils
{
    #region ConnectionFactory
    public class ConnectionFactory : IConnectionFactory
    {
        #region Instances
        private static ConnectionFactory _Instance;

        public static ConnectionFactory Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ConnectionFactory();
                }
                return _Instance;
            }
            private set
            {
                _Instance = value;
            }
        }

        private ConnectionFactory()
        {
            Instance = this;
        }
        #endregion

        #region Database Connection
        public DbConnection GetConnection()
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string connectionString;

            connectionString =
                string.Format("server={0};uid={1};pwd={2};database={3};",
                        Settings.DatabaseIp, Settings.DatabaseUsername,
                        Settings.DatabasePassword, Settings.DatabaseName
                );

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return null;
            }

            return conn;
        }

        public void Close(DbConnection con)
        {
            con.Close();
        }
        #endregion
    }
    #endregion
}
