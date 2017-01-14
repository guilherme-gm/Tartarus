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
using Common.DataClasses;
using Common.DataClasses.Network;
using Game.DataClasses;
using Game.DataClasses.Network;

using CG = Game.DataClasses.Network.ClientGame;
using GC = Game.DataClasses.Network.GameClient;

namespace Game.Business.Client
{
    public class Login : ICommand
    {
        public void Execute(Session session, Packet message)
        {
            CG.Login packet = (CG.Login)message;

            Packet result;
            // TODO : Code this

            result = new GC.UrlList()
            {
                _UrlList = "guild.url|http://127.0.0.1/guild/login.aspx|guild_test_download.url|upload/|web_download|127.0.0.1|web_download_port|0|shop.url|http://127.0.0.1/khroos|ghelp_url|http://127.0.0.1|guild_icon_upload.ip|127.0.0.1|guild_icon_upload.port|4617"
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.StatInfo()
            {
                Handle = 0x80000600,
                Stat = new DataClasses.GameWorld.CreatureStat()
                {
                    StatId = 200,
                    Strength = 10,
                    Agility = 10,
                    Vitality = 10,
                    Dexterity = 10,
                    Intelligence = 10,
                    Luck = 10,
                    Mentality = 10
                },
                Attribute = new DataClasses.GameWorld.CreatureAttribute()
                {
                    
                },
                Type = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.StatInfo()
            {
                Handle = 0x80000600,
                Stat = new DataClasses.GameWorld.CreatureStat()
                {

                },
                Attribute = new DataClasses.GameWorld.CreatureAttribute()
                {

                },
                Type = 1
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "max_havoc",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "max_chaos",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);
            
            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "max_stamina",
                Value = 50000
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new DataClasses.Network.Both.TimeSync()
            {
                Time = 16174
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "max_havoc",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.LoginResult()
            {
                IsAccepted = true,
                Handle = 0x80000600,
                X = 164624,
                Y = 52561,
                Z = 0,
                Layer = 1,
                FaceDirection = 0,
                RegionSize = 180,
                HP = 320,
                MP = 320,
                MaxHP = 320,
                MaxMP = 320,
                Havoc = 0,
                MaxHavoc = 0,
                Sex = 2,
                Race = 4,
                SkinColor = 8487040,
                FaceId = 202,
                HairId = 102,
                Name = "Tartarus",
                CellSize = 6,
                GuildId = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Inventory()
            {
                Count = 0,
                Items = new DataClasses.GameWorld.ItemInfo[0]
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            // TODO : 0x012F

            result = new GC.WearInfo()
            {
                Handle = 0x80000600,
                ElementalEffectType = new byte[24],
                ItemCode = new int[24],
                ItemEnhance = new int[24],
                ItemLevel = new int[24]
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.GoldUpdate()
            {
                Gold = 0,
                Chaos = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "chaos",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.LevelUpdate()
            {
                Handle = 0x80000600,
                JobLevel = 1,
                Level = 1
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.ExpUpdate()
            {
                Handle = 0x80000600,
                Exp = 0,
                JP = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "job",
                Value = 200
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "job_level",
                Value = 1
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "job_0",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "jlv_0",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "job_1",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "jlv_1",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "job_2",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "jlv_2",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.BeltSlotInfo()
            {
                Handle = new uint[6]
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.GameTime()
            {
                T = 16220,
                _GameTime = 1484355440
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "huntaholic_ent",
                Value = 12
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "dk_count",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "pk_count",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "immoral",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "stamina",
                Value = 300
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "channel",
                Value = 1
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.StatusChange()
            {
                Handle = 0x80000600,
                Status = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.QuestList()
            {
                ActiveQuests = new DataClasses.GameWorld.QuestInfo[0],
                PendingQuests = new DataClasses.GameWorld.PendingQuestInfo[0]
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Chat()
            {
                Sender = "@FRIEND",
                Type = 140,
                Message = "FLIST|"
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Chat()
            {
                Sender = "@FRIEND",
                Type = 140,
                Message = "DLIST|"
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);
            
            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "playtime",
                Value = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "playtime_limit",
                Value = 1080000
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "playtime_limit2",
                Value = 1800000
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.ChangeLocation()
            {
                PrevLocationId = 0,
                CurLocationId = 100301
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.WeatherInfo()
            {
                RegionId = 100301,
                WeatherId = 0
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = false,
                Name = "client_info",
                Value = 0,
                StringValue = ""
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);

            result = new GC.Property()
            {
                Handle = 0x80000600,
                IsNumber = true,
                Name = "stamina_regen",
                Value = 30
            };
            DataClasses.Server.ClientSockets.SendPacket(session, result);
        }
    }
}
