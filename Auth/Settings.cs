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
    /// <summary>
    /// Stores Auth-server Settings
    /// </summary>
    public static class Settings
    {
        public static string WindowName { get; private set; }

        public static string ServerIp { get; private set; }
        public static ushort ServerPort { get; private set; }

        public static bool LoginDebug { get; private set; }
        public static int ConsoleSilent { get; private set; }

        public static ushort GameServerPort { get; private set; }

        public static bool UseMD5Password { get; private set; }

        public static void Load()
        {
            SettingsReader reader = new SettingsReader();
            reader.LoadSettings("Settings/auth.conf");

            WindowName = reader.ReadString("window_name", "Auth-Server", false);
            ServerPort = reader.ReadUInt16("server_port", 8841, ushort.MinValue, ushort.MaxValue);

            ServerIp = reader.ReadString("server_ip", "127.0.0.1", false);

            LoginDebug = reader.ReadBoolean("login_debug", true);
            ConsoleSilent = reader.ReadInt32("console_silent", 0, 0, int.MaxValue);

            GameServerPort = reader.ReadUInt16("gameserver_port", 4444, ushort.MinValue, ushort.MaxValue);
            UseMD5Password = reader.ReadBoolean("use_md5", false);
        }
    }
}
