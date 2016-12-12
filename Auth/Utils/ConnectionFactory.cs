using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Utils
{
    public class ConnectionFactory : IConnectionFactory
    {
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

        public DbConnection GetConnection()
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string connectionString;

            connectionString = "server=127.0.0.1;uid=rappelz;pwd=rappelz;database=rappelz_auth;";

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
    }
}
