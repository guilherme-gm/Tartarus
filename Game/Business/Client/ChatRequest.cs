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
using CG = Game.DataClasses.Network.ClientGame;
using GC = Game.DataClasses.Network.GameClient;
using Game.DataClasses;
using Game.DataClasses.Objects;
using Game.DataClasses.GameWorld;

namespace Game.Business.Client
{
    #region Chat Request Packet
    public class ChatRequest : ICommand
    {
        #region Execute Packet
        public void Execute(Session session, Packet message)
        {
            CG.ChatRequest packet = (CG.ChatRequest) message;

            if (session._Client == null)
                return;
            Player player = ((User)session._Client).Character;
            if (player == null)
                return;
            
            // TODO : Other chat types
            switch (packet.Type)
            {
                case ChatType.Normal:
                    {
                        if (packet.Message.StartsWith("/"))
                        {
                            // TODO : Commands
                            return;
                        }

                        GC.ChatLocal chatLocal = new GC.ChatLocal();
                        chatLocal.GID = player.GID;
                        chatLocal.Type = ChatType.Normal;
                        chatLocal.Message = packet.Message;
                        DataClasses.Server.ClientSockets.SendArea(player.Region, chatLocal);

                        GC.Result result = new GC.Result();
                        result.RequestMessageId = packet.Id;
                        result.ResultCode = (ushort) CG.ChatRequest.Result.Success;
                        DataClasses.Server.ClientSockets.SendSelf(session, result);
                    }
                    break;
                case ChatType.Yell:
                    break;
                case ChatType.Adv:
                    {
                        // TODO : Party Chat
                        //if (player.Party == null)
                        //  return;

                        GC.Chat chat = new GC.Chat();
                        chat.Sender = player.Name;
                        chat.Type = ChatType.Adv;
                        chat.Message = packet.Message;
                        DataClasses.Server.ClientSockets.SendAll(chat);

                        GC.Result result = new GC.Result();
                        result.RequestMessageId = packet.Id;
                        result.ResultCode = (ushort)CG.ChatRequest.Result.Success;
                        DataClasses.Server.ClientSockets.SendSelf(session, result);
                    }
                    break;
                case ChatType.Whisper:
                    break;
                case ChatType.Global:
                    {
                        GC.Chat chat = new GC.Chat();
                        chat.Sender = player.Name;
                        chat.Type = ChatType.Global;
                        chat.Message = packet.Message;
                        DataClasses.Server.ClientSockets.SendAll(chat);

                        GC.Result result = new GC.Result();
                        result.RequestMessageId = packet.Id;
                        result.ResultCode = (ushort)CG.ChatRequest.Result.Success;
                        DataClasses.Server.ClientSockets.SendSelf(session, result);
                    }
                    break;
                case ChatType.Emotion:
                    break;
                case ChatType.GM:
                    break;
                case ChatType.GMWhisper:
                    break;
                case ChatType.Party:
                    {
                        // TODO : Party Chat
                        //if (player.Party == null)
                        //  return;

                        GC.Chat chat = new GC.Chat();
                        chat.Sender = player.Name;
                        chat.Type = ChatType.Party;
                        chat.Message = packet.Message;
                        //DataClasses.Server.ClientSockets.SendParty(chat);
                    }
                    break;
                case ChatType.Guild:
                    break;
                case ChatType.AttackTeam:
                    break;
                case ChatType.Notice:
                    break;
                case ChatType.Exp:
                    break;
                case ChatType.Damage:
                    break;
                case ChatType.Item:
                    break;
                case ChatType.Battle:
                    break;
                case ChatType.Summon:
                    break;
                case ChatType.Etc:
                    break;
                case ChatType.NPC:
                    break;
                case ChatType.Debug:
                    break;
                case ChatType.PartySystem:
                    break;
                case ChatType.GuildSystem:
                    break;
                case ChatType.QuestSystem:
                    break;
                case ChatType.RaidSystem:
                    break;
                case ChatType.FriendSystem:
                    break;
                case ChatType.AllianceSystem:
                    break;
                case ChatType.HuntaholicSystem:
                    break;
                default:
                    break;
            }

        }
        #endregion
    }
    #endregion
}
