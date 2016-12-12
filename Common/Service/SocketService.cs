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
using Common.Utils;
using System;
using System.Net;
using System.Net.Sockets;

namespace Common.Service
{
    public class SocketService
    {
        /// <summary>
        /// Size of a packet header
        /// </summary>
        private const int HeaderSize = 7;

        /// <summary>
        /// Informs if data sent and received is encrypted
        /// </summary>
		private bool Encrypted;

        /// <summary>
        /// Ip and Port used to listen connections
        /// </summary>
        private IPEndPoint IpEndPoint;

        /// <summary>
        /// Session Factory used to initialize connections
        /// </summary>
        private SessionFactory _SessionFactory;

        /// <summary>
        /// The controller that will handle the packets
        /// </summary>
        private IController Controller;

        /// <summary>
        /// Creates a new SocketService
        /// </summary>
        /// <param name="ip">Listen IP</param>
        /// <param name="port">Listen Port</param>
        /// <param name="isEncrypted">Is data encrypted?</param>
        /// <param name="key">Encryption key</param>
        public SocketService(string ip, ushort port, bool isEncrypted, SessionFactory sessionFactory, IController controller)
        {
            this.Encrypted = isEncrypted;
            this._SessionFactory = sessionFactory;
            this.Controller = controller;

            try
            {
                IPAddress ipAddress;
                if (!IPAddress.TryParse(ip, out ipAddress))
                {
                    ConsoleUtils.ShowError("Failed to parse Server IP ({0}), defaulting to 127.0.0.1", ip);
                    ipAddress = IPAddress.Parse("127.0.0.1");
                }

                this.IpEndPoint = new IPEndPoint(ipAddress, port);
            }
            catch (Exception e)
            {
                ConsoleUtils.ShowFatalError(
                    "Unexpected error: {0}\nAt {1}",
                    e.Message, "SocketService()"
                );
            }
        }

        /// <summary>
        /// Starts to listen for incoming connections.
        /// </summary>
        /// <returns>true in case of success, false otherwise</returns>
        public bool Start()
        {
            Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(this.IpEndPoint);
                listener.Listen(100);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            }
            catch (Exception e)
            {
                ConsoleUtils.ShowFatalError(
                    "Unexpected error: {0}\nAt {1}",
                    e.Message, "SocketService.Start()"
                );

                listener.Close();
                return false;
            }

            ConsoleUtils.ShowInfo(
                "Listening to connection on {0}:{1}",
                this.IpEndPoint.Address.ToString(),
                this.IpEndPoint.Port
            );

            return true;
        }

        /// <summary>
        /// Called when a client tries to connect
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket connector = listener.EndAccept(ar);

            // Starts to accept another connection
            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

            Session client = this._SessionFactory.CreateSession(connector);

            connector.BeginReceive(
                client._NetworkData.Buffer,
                0,
                NetworkData.MaxBuffer,
                SocketFlags.None,
                new AsyncCallback(ReadCallback),
                client
            );
        }

        /// <summary>
        /// Called when client sends data to server
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallback(IAsyncResult ar)
        {
            Session session = (Session)ar.AsyncState;

            try
            {
                int bytesToRead = session._NetworkData._Socket.EndReceive(ar);
                int byteCount;

                if (bytesToRead <= 0)
                {
                    // TODO: Disconect
                    return;
                }

                int curOffset = 0;
                byte[] data;

                if (this.Encrypted)
                {
                    data = session._NetworkData.InCipher.DoCipher(
                        ref session._NetworkData.Buffer,
                        bytesToRead
                    );
                }
                else
                {
                    data = session._NetworkData.Buffer;
                }

                do
                {
                    int packetOffset = session._NetworkData.Offset;

                    if (packetOffset + bytesToRead < HeaderSize)
                    {
                        // Packet header incomplete, only copy bytes
                        Buffer.BlockCopy(data, curOffset, session._NetworkData.Message, packetOffset, bytesToRead);
                        packetOffset += bytesToRead;
                        bytesToRead = 0;
                    }
                    else
                    {
                        if (packetOffset < HeaderSize)
                        {
                            byteCount = HeaderSize - packetOffset;
                            Buffer.BlockCopy(data, curOffset, session._NetworkData.Message, packetOffset, byteCount);
                            curOffset += byteCount;
                            packetOffset += byteCount;
                        }
                        // Packet header complete
                        int length = BitConverter.ToInt32(session._NetworkData.Message, 0);

                        if (packetOffset + bytesToRead < length)
                        {
                            // Packet data incomplete, only copy bytes
                            Buffer.BlockCopy(data, curOffset, session._NetworkData.Message, packetOffset, bytesToRead);
                            packetOffset += bytesToRead;
                            bytesToRead = 0;
                        }
                        else
                        {
                            // Packet data complete, notify
                            byte[] packet = new byte[length];

                            // Join previous data with the one received now into a packet
                            byteCount = packetOffset;
                            Buffer.BlockCopy(session._NetworkData.Message, 0, packet, 0, byteCount);
                            byteCount = length - byteCount;
                            Buffer.BlockCopy(data, curOffset, packet, packetOffset, byteCount);

                            // Clear offsets
                            packetOffset = 0;
                            bytesToRead -= byteCount;
                            curOffset += byteCount;

                            // Notify packet receivement
                            PacketReceived(session, packet);
                        }
                    }
                    session._NetworkData.Offset = packetOffset;
                } while (bytesToRead > 0);
            }
            catch (Exception e)
            {

            }

            // Put socket to wait for another receive
            session._NetworkData._Socket.BeginReceive(
                session._NetworkData.Buffer,
                0,
                NetworkData.MaxBuffer,
                SocketFlags.None,
                new AsyncCallback(ReadCallback),
                session
            );
        }

        /// <summary>
        /// Called when a packet is completely received.
        /// </summary>
        /// <param name="session">The session who sent this packet</param>
        /// <param name="packet">Packet data</param>
        public void PacketReceived(Session session, byte[] packet)
        {
            this.Controller.ProcessRequest(session, packet);
        }

    }

}

