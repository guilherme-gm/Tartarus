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
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Common.Utils
{
    /// <summary>
    /// Class used to parse Settings file
    /// </summary>
    public class SettingsReader
    {
        /// <summary>
        /// Maximum Settings File nesting with import call.
        /// This avoids reader to get into a infinite loop
        /// because of misconfiguration
        /// </summary>
        private const int MaximumDepth = 5;

        /// <summary>
        /// Dictionary with all settings entries
        /// </summary>
        private Dictionary<string, object> SettingsDictionary;

        /// <summary>
        /// Starts a new Settings File Reader
        /// </summary>
        public SettingsReader()
        {
            this.SettingsDictionary = new Dictionary<string, object>();
        }

        /// <summary>
        /// Loads a settings file to parse it later
        /// </summary>
        /// <param name="path">file path</param>
        public void LoadSettings(string path)
        {
            this.LoadSettings(path, 0);
        }

        /// <summary>
        /// Loads a settings file to parse it later, this is the
        /// real function, with depth counter to avoid infinite loops
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="depth">depth counter (to avoid infinite loops)</param>
        private void LoadSettings(string path, int depth)
        {
            if (depth == MaximumDepth)
            {
                ConsoleUtils.ShowError("Too many recursive calls ({0}) to settings import detected. Loading of {1} aborted.", MaximumDepth, path);
                return;
            }
            else if (!File.Exists(path))
            {
                ConsoleUtils.ShowError("Could not find Settings File at {0}. Skipping it.", path);
                return;
            }

            string json = File.ReadAllText(path);
            JObject jsonObj = JObject.Parse(json);
            Dictionary<string, object> newSettings = jsonObj.ToObject<Dictionary<string, object>>();

            object import;
            List<string> imports = null;

            if (newSettings.TryGetValue("import", out import))
            {
                JToken token = jsonObj.GetValue("import");
                if (token.Type == JTokenType.Array)
                {
                    imports = token.ToObject<List<string>>();
                }
                else
                {
                    imports = new List<string>();
                    imports.Add(token.ToObject<string>());
                }
                newSettings.Remove("import");
            }

            foreach (KeyValuePair<string, object> kvp in newSettings)
            {
                if (this.SettingsDictionary.ContainsKey(kvp.Key))
                {
                    this.SettingsDictionary[kvp.Key] = kvp.Value;
                }
                else
                {
                    this.SettingsDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            if (imports != null)
            {
                foreach (string item in imports)
                {
                    LoadSettings(item, depth + 1);
                }
            }
        }

        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public byte ReadByte(string name, byte defaultValue, byte min, byte max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            byte value;
            if (!byte.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public sbyte ReadSByte(string name, sbyte defaultValue, sbyte min, sbyte max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            sbyte value;
            if (!sbyte.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }
        
        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public ushort ReadUInt16(string name, ushort defaultValue, ushort min, ushort max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            ushort value;
            if (!ushort.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public short ReadInt16(string name, short defaultValue, short min, short max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            short value;
            if (!short.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }
        
        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public uint ReadUInt32(string name, uint defaultValue, uint min, uint max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            uint value;
            if (!uint.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }
        
        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public int ReadInt32(string name, int defaultValue, int min, int max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            int value;
            if (!int.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }
        
        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public ulong ReadUInt64(string name, ulong defaultValue, ulong min, ulong max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            ulong value;
            if (!ulong.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }
        
        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        /// <returns>setting valid value</returns>
        public long ReadInt64(string name, long defaultValue, long min, long max)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            long value;
            if (!long.TryParse(entry.ToString(), out value))
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' must be a number, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            if (value < min || value > max)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' ({1}) is out of allowed range ({2} - {3}). Changing back to default ({4})",
                    name, value, min, max, defaultValue
                );
                value = defaultValue;
            }

            return value;
        }
        
        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <param name="empty">Can this setting be empty?</param>
        /// <returns>setting valid value</returns>
        public string ReadString(string name, string defaultValue, bool empty)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            string value = entry.ToString();

            if (value.Trim().Equals("") && !empty)
            {
                ConsoleUtils.ShowWarning(
                    "Value of setting '{0}' not be empty, changing back to default ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Reads setting <para>name</para> and validates it
        /// </summary>
        /// <param name="name">name in settings file</param>
        /// <param name="defaultValue">default value, used if something goes wrong</param>
        /// <returns>setting valid value</returns>
        public bool ReadBoolean(string name, bool defaultValue)
        {
            object entry;
            if (!this.SettingsDictionary.TryGetValue(name, out entry))
            {
                ConsoleUtils.ShowError(
                    "Setting '{0}' not found. Using default value ({1}).",
                    name, defaultValue
                );
                return defaultValue;
            }

            bool value;
            if (!bool.TryParse(entry.ToString(), out value))
            {
                string val = entry.ToString();

                // Checks for 0/1 instead of true/false
                if (val.Equals("1"))
                {
                    value = true;
                }
                else if (val.Equals("0"))
                {
                    value = false;
                }
                else
                {
                    ConsoleUtils.ShowWarning(
                        "Value of setting '{0}' must be true(1) or false(0), changing back to default ({1}).",
                        name, defaultValue
                    );
                    return defaultValue;
                }
            }

            return value;
        }
    }
}