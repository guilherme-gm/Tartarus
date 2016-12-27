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

namespace Game
{
    /// <summary>
    /// Stores Game-server Settings
    /// </summary>
    public static class Settings
    {
        public static string WindowName { get; private set; }

        public static string ServerIp { get; private set; }
        public static ushort ServerPort { get; private set; }

        public static ushort ServerIndex { get; private set; }
        public static string ServerName { get; private set; }
        public static string NoticeUrl { get; private set; }

        public static int MinPermission { get; private set; }

        public static bool KeepDeletedCharacters { get; private set; }
        public static string ForbiddenCharacters { get; private set; }

        public static int ConsoleSilent { get; private set; }

        public static ushort MaxConnections { get; private set; }
        public static ushort AuthPort { get; private set; }

        public static string DatabaseIp { get; private set; }
        public static string DatabaseUsername { get; private set; }
        public static string DatabasePassword { get; private set; }
        public static string DatabaseName { get; private set; }

        public static void Load()
        {
            SettingsReader reader = new SettingsReader();
            reader.LoadSettings("Settings/game-server.conf");

            WindowName = reader.ReadString("window_name", "Game", false);

            ServerIp = reader.ReadString("server_ip", "127.0.0.1", false);
            ServerPort = reader.ReadUInt16("server_port", 6900, ushort.MinValue, ushort.MaxValue);

            ServerIndex = reader.ReadUInt16("server_index", 1, ushort.MinValue, ushort.MaxValue);
            ServerName = reader.ReadString("server_name", "Tartarus", false);
            NoticeUrl = reader.ReadString("notice_url", "", true);

            MinPermission = reader.ReadInt32("min_permission", 0, int.MinValue, int.MaxValue);

            KeepDeletedCharacters = reader.ReadBoolean("keep_deleted_characters", false);
            ForbiddenCharacters = reader.ReadString("forbidden_characters", "", true);

            ConsoleSilent = reader.ReadInt32("console_silent", 0, int.MinValue, int.MaxValue);

            MaxConnections = reader.ReadUInt16("max_connections", 0, ushort.MinValue, ushort.MaxValue);
            AuthPort = reader.ReadUInt16("auth_port", 4444, ushort.MinValue, ushort.MaxValue);

            DatabaseIp = reader.ReadString("database_ip", "127.0.0.1", false);
            DatabaseUsername = reader.ReadString("database_username", "tartarus", false);
            DatabasePassword = reader.ReadString("database_password", "tartarus", false);
            DatabaseName = reader.ReadString("database_name", "tartarus_game", false);
        }
    }
}
