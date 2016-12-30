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

namespace Auth
{
    #region Authentication Server Settings
    /// <summary>
    /// Stores Auth-server Settings
    /// </summary>
    public static class Settings
    {
        #region Get/Set
        public static string WindowName { get; private set; }

        public static bool OpenExternal { get; private set; }
        public static ushort ServerPort { get; private set; }

        public static bool LoginDebug { get; private set; }
        public static int ConsoleSilent { get; private set; }

        public static ushort GameServerPort { get; private set; }

        public static bool UseMD5Password { get; private set; }

        public static bool PersistServerId { get; private set; }
        
        public static string DatabaseIp { get; private set; }
        public static string DatabaseUsername { get; private set; }
        public static string DatabasePassword { get; private set; }
        public static string DatabaseName { get; private set; }
        #endregion

        #region Load From Configuration
        public static void Load()
        {
            #region Configuration Variables
            SettingsReader reader = new SettingsReader();
            reader.LoadSettings("Settings/auth-server.conf");

            WindowName = reader.ReadString("window_name", "Auth-Server", false);
            ServerPort = reader.ReadUInt16("server_port", 8841, ushort.MinValue, ushort.MaxValue);

            OpenExternal = reader.ReadBoolean("open_external", false);

            LoginDebug = reader.ReadBoolean("login_debug", true);
            ConsoleSilent = reader.ReadInt32("console_silent", 0, 0, int.MaxValue);

            GameServerPort = reader.ReadUInt16("gameserver_port", 4444, ushort.MinValue, ushort.MaxValue);
            UseMD5Password = reader.ReadBoolean("use_md5", false);

            PersistServerId = reader.ReadBoolean("persist_server_id", false);

            DatabaseIp = reader.ReadString("database_ip", "127.0.0.1", false);
            DatabaseUsername = reader.ReadString("database_username", "tartarus", false);
            DatabasePassword = reader.ReadString("database_password", "tartarus", false);
            DatabaseName = reader.ReadString("database_name", "tartarus_auth", false);
            #endregion
        }
        #endregion
    }
    #endregion
}
