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
using Game.DataClasses.Database;
using Game.DataClasses.Objects;
using System.Collections.Generic;
using GC = Game.DataClasses.Network.GameClient;
using System;
using Game.DataClasses.Network.ClientGame;
using System.Linq;
using Common.DataClasses.Collections;

namespace Game.DataClasses.GameWorld
{
    #region Region Class
    public class Region
    {
        public const int RegionSize = 180;

        public static Region[][][] Regions;

        #region Initializing Methods
        public static void Init()
        {
            // TODO : These values must be determined by maps
            int mapWidth = 13;
            int mapHeight = 10;
            int channels = 1;

            int regionsX = (mapWidth * MapBase.MapSize) / RegionSize;
            int regionsY = (mapHeight * MapBase.MapSize) / RegionSize;

            Regions = new Region[regionsX][][];
            for (uint i = 0; i < regionsX; i++)
            {
                Regions[i] = new Region[regionsY][];
                for (uint j = 0; j < regionsY; j++)
                {
                    Regions[i][j] = new Region[channels];
                    for (uint k = 0; k < channels; k++)
                    {
                        // IMPROVE : We might be able to improve that by not initializing a
                        //          region for invalid places.
                        Regions[i][j][k] = new Region(i, j);
                    }
                }
            }
        }
        #endregion

        #region Helper Methods
        public static Region FromPosition(Position pos)
        {
            return FromPosition(pos.X, pos.Y, pos.Layer);
        }
        public static Region FromPosition(float x, float y, byte layer)
        {
            uint rx = (uint)(x / RegionSize);
            uint ry = (uint)(y / RegionSize);
            layer = 0; // TODO : Change this whne channels are supported.

            // Ensure that given region exists.
            if (Regions.Length < rx)
                return null;
            if (Regions[rx].Length < ry)
                return null;
            if (Regions[rx][ry].Length < layer)
                return null;

            return Regions[rx][ry][layer];
        }

        public static Region FromRegionXY(uint rx, uint ry, byte layer)
        {
            // Ensure that given region exists.
            if (Regions.Length < rx)
                return null;
            if (Regions[rx].Length < ry)
                return null;
            if (Regions[rx][ry].Length < layer)
                return null;

            return Regions[rx][ry][layer];
        }

        public static List<Region> GetNearbyRegions(Region region)
        {
            List<Region> nearbyRegions = new List<Region>();
            int x = -3, startY = -3;

            // Ensure that we will not get negative X or Y
            if (region.X < 3)
                x = (int)(-1 * region.X);
            if (region.Y < 3)
                startY = (int)(-1 * region.Y);

            // Loops through a box around view area and get the nearby regions
            for (; x <= 3; ++x)
            {
                for (int y = startY; y <= 3; ++y)
                {
                    if ((x * x + y * y) <= 10)
                        nearbyRegions.Add(Region.FromRegionXY((uint)(region.X + x), (uint)(region.Y + y), region.Layer));
                }
            }

            return nearbyRegions;
        }
        #endregion

        public uint X { get; set; }
        public uint Y { get; set; }
        public byte Layer { get; set; }
        public HashSet<Player> Players { get; set; }
        public HashSet<GameObject> GameObjects { get; set; }

        public Region(uint rx, uint ry)
        {
            this.X = rx;
            this.Y = ry;
            this.Players = new HashSet<Player>();
            this.GameObjects = new HashSet<GameObject>();
        }

        /// <summary>
        /// Enters in the region
        /// </summary>
        /// <param name="gobject"></param>
        public void Enter(GameObject gobject, bool isSpawn = false)
        {
            lock (this)
            {
                switch (gobject.SubType)
                {
                    case ObjectSubType.Player:
                        this.Players.Add((Player)gobject);
                        break;
                }

                // CHECK : Maybe this is only required if you're spawning?
                //  (you already know the region otherwise)
                this.GameObjects.Add(gobject);
                this.NotifyEnter(gobject, isSpawn);
            }
        }

        /// <summary>
        /// Change regions
        /// </summary>
        /// <param name="player"></param>
        internal void ChangeRegion(Player player)
        {
            Region oldRegion = player.Region;
            if (oldRegion == null)
                return;

            if (oldRegion == this)
                return;

            List<Region> oldNearby = GetNearbyRegions(oldRegion);
            List<Region> newNearby = GetNearbyRegions(this);

            List<Region> regionLeft = oldNearby.Except(newNearby).ToList();
            List<Region> regionEnter = newNearby.Except(oldNearby).ToList();

            oldRegion.Leave(player);
            this.Enter(player);

            foreach (Region r in regionEnter)
            {
                r.NotifyEnter(player);
                r.ReceiveObjects(player);
            }
        }

        /// <summary>
        /// Player has left this region
        /// </summary>
        /// <param name="player"></param>
        private void Leave(Player player)
        {
            lock (this)
            {
                this.Players.Remove(player);
                this.GameObjects.Remove(player);
            }
        }

        /// <summary>
        /// Request to Move an Creature through points
        /// </summary>
        /// <param name="creature"></param>
        /// <param name="points"></param>
        internal void MoveRequest(Creature creature, MoveRequest.MoveInfo[] points)
        {
            List<Position> movePoints = creature.PendingMovePositions;
            bool isWalking = movePoints.Count != 0;

            if (isWalking)
            {
                // TODO : Stop current move
            }

            movePoints.Clear();
            foreach(MoveRequest.MoveInfo info in points)
            {
                movePoints.Add(new Position() { X = info.ToX, Y = info.ToY });
            }

            Move(creature);
        }

        /// <summary>
        /// Move creature
        /// </summary>
        /// <param name="creature"></param>
        internal void Move(Creature creature)
        {
            // TODO : Server Side move update

            GC.Move move = new GC.Move();
            move.GID = creature.GID;
            move.Speed = (byte) (creature.Attributes.MoveSpeed + creature.AttributesByState.MoveSpeed);
            //move.StartTime =
            move.ToLayer = creature.Position.Layer;
            move.Points = new GC.Move.MoveInfo[creature.PendingMovePositions.Count];

            int i = 0;
            foreach (Position pos in creature.PendingMovePositions)
            {
                move.Points[i] = new GC.Move.MoveInfo()
                {
                    ToX = pos.X,
                    ToY = pos.Y
                };
                ++i;
            }

            Server.ClientSockets.SendArea(this, move);
        }

        private void NotifyEnter(GameObject gobject, bool isSpawn = false)
        {
            // Informs all listeners that this game object entered the area
            // IMPROVE : Maybe we can use type to merge some writings
            switch (gobject.SubType)
            {
                case ObjectSubType.Player:
                    {
                        GC.PlayerEnter enter = new GC.PlayerEnter();
                        Player player = (Player)gobject;
                        enter.Type = player.Type;
                        enter.GID = player.GID;
                        enter.X = player.Position.X;
                        enter.Y = player.Position.Y;
                        enter.Z = player.Position.Z;
                        enter.Layer = player.Position.Layer;
                        enter.SubType = player.SubType;
                        enter.Status = 0; // TODO : Status
                        enter.FaceDirection = 0; // TODO : FaceDirection
                        enter.Hp = player.HP;
                        enter.MaxHp = player.MaxHp;
                        enter.Mp = player.MP;
                        enter.MaxMp = player.MaxMP;
                        enter.Level = player.Level;
                        enter.Race = (byte)player.Race; // TODO : Different types
                        enter.SkinColor = player.SkinColor;
                        enter.IsFirstEnter = isSpawn;
                        enter.Energy = 0; // TODO : What is energy?
                        enter.Sex = (byte)player.Sex; // TODO :Different types
                        enter.FaceId = player.BaseModel[1];
                        enter.FaceTextureId = player.FaceTextureId;
                        enter.HairId = player.BaseModel[0];
                        enter.Name = player.Name;
                        enter.JobId = (ushort)player.Job.Id; // TODO :Different types
                        enter.RideGID = 0; // TODO : Ride GID
                        enter.GuildId = player.GuildId;

                        if (isSpawn)
                            Server.ClientSockets.SendAreaWithoutSelf(player.User._Session, this, enter);
                        else
                            Server.ClientSockets.SendRegionWithoutSelf(player.User._Session, this, enter);

                        GC.WearInfo wearInfo = new GC.WearInfo();
                        wearInfo.Handle = player.GID;
                        wearInfo.BaseModel = player.BaseModel;
                        wearInfo.EquippedItems = player.EquippedItems;

                        if (isSpawn)
                            Server.ClientSockets.SendAreaWithoutSelf(player.User._Session, this, wearInfo);
                        else
                            Server.ClientSockets.SendRegionWithoutSelf(player.User._Session, this, wearInfo);
                    }
                    break;
                case ObjectSubType.Npc:
                    break;
                case ObjectSubType.Item:
                    break;
                case ObjectSubType.Monster:
                    break;
                case ObjectSubType.Summon:
                    break;
                case ObjectSubType.SkillProp:
                    break;
                case ObjectSubType.FieldProp:
                    break;
                case ObjectSubType.Pet:
                    break;
                default:
                    break;
            }
        }

        internal void ReceiveObjects(Player src)
        {
            // TODO : This needs to send other kinds of objects

            foreach (GameObject gobject in this.GameObjects.AsReadOnly())
            {
                if (gobject == src)
                    continue;

                // IMPROVE : Maybe we can use type to merge some writings
                switch (gobject.SubType)
                {
                    case ObjectSubType.Player:
                        {
                            GC.PlayerEnter enter = new GC.PlayerEnter();
                            Player player = (Player)gobject;
                            enter.Type = player.Type;
                            enter.GID = player.GID;
                            enter.X = player.Position.X;
                            enter.Y = player.Position.Y;
                            enter.Z = player.Position.Z;
                            enter.Layer = player.Position.Layer;
                            enter.SubType = player.SubType;
                            enter.Status = 0; // TODO : Status
                            enter.FaceDirection = 0; // TODO : FaceDirection
                            enter.Hp = player.HP;
                            enter.MaxHp = player.MaxHp;
                            enter.Mp = player.MP;
                            enter.MaxMp = player.MaxMP;
                            enter.Level = player.Level;
                            enter.Race = (byte)player.Race; // TODO : Different types
                            enter.SkinColor = player.SkinColor;
                            enter.IsFirstEnter = true;
                            enter.Energy = 0; // TODO : What is energy?
                            enter.Sex = (byte)player.Sex; // TODO :Different types
                            enter.FaceId = player.BaseModel[1];
                            enter.FaceTextureId = player.FaceTextureId;
                            enter.HairId = player.BaseModel[0];
                            enter.Name = player.Name;
                            enter.JobId = (ushort)player.Job.Id; // TODO :Different types
                            enter.RideGID = 0; // TODO : Ride GID
                            enter.GuildId = player.GuildId;

                            Server.ClientSockets.SendSelf(src.User._Session, enter);

                            GC.WearInfo wearInfo = new GC.WearInfo();
                            wearInfo.Handle = player.GID;
                            wearInfo.BaseModel = player.BaseModel;
                            wearInfo.EquippedItems = player.EquippedItems;

                            Server.ClientSockets.SendSelf(src.User._Session, wearInfo);
                        }
                        break;
                    case ObjectSubType.Npc:
                        break;
                    case ObjectSubType.Item:
                        break;
                    case ObjectSubType.Monster:
                        break;
                    case ObjectSubType.Summon:
                        break;
                    case ObjectSubType.SkillProp:
                        break;
                    case ObjectSubType.FieldProp:
                        break;
                    case ObjectSubType.Pet:
                        break;
                    default:
                        break;
                }
            }
        }

        internal void Logout(Player player)
        {
            lock (this)
            {
                this.GameObjects.Remove(player);
                this.Players.Remove(player);

                GC.Leave leave = new GC.Leave();
                Server.ClientSockets.SendAreaWithoutSelf(player.User._Session, this, leave);
            }
        }
    }
    #endregion
}

