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
using Common.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Common.Service
{
    public delegate void Disconnect(Session session);

    public class SocketService
    {
        /// <summary>
        /// Event triggered when a session disconects
        /// </summary>
        public event Disconnect OnSocketDisconnect;

        /// <summary>
        /// Size of a packet header
        /// </summary>
        private const int HeaderSize = Packet.HeaderSize;

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
        /// Is service started?
        /// </summary>
        private bool Started;

        /// <summary>
        /// Creates a new SocketService
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <param name="isEncrypted">Is data encrypted?</param>
        /// <param name="key">Encryption key</param>
        public SocketService(string ip, ushort port, bool isEncrypted, SessionFactory sessionFactory, IController controller)
        {
            this.Encrypted = isEncrypted;
            this._SessionFactory = sessionFactory;
            this.Controller = controller;
            this.Started = false;

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
        public bool StartListening()
        {
            if (this.Started)
            {
                ConsoleUtils.ShowFatalError(
                    "Trying to start an already started SocketService.\nAt {0}",
                    "SocketService.StartListening()"
                );
                return false;
            }
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
                    e.Message, "SocketService.StartListening()"
                );

                listener.Close();
                return false;
            }

            this.Started = true;
            ConsoleUtils.ShowInfo(
                "Listening to connection on {0}:{1}",
                this.IpEndPoint.Address.ToString(),
                this.IpEndPoint.Port
            );

            return true;
        }

        /// <summary>
        /// Starts to connect to an IP and Port
        /// </summary>
        /// <returns>If it has connected</returns>
        public bool StartConnection()
        {
            if (this.Started)
            {
                ConsoleUtils.ShowFatalError(
                    "Trying to start an already started SocketService.\nAt {0}",
                    "SocketService.StartConnection()"
                );
                return false;
            }
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.BeginConnect(this.IpEndPoint, new AsyncCallback(ConnectCallback), socket);
            }
            catch (Exception e)
            {
                ConsoleUtils.ShowFatalError(
                    "Unexpected error: {0}\nAt {1}",
                    e.Message, "SocketService.StartConnection()"
                );

                socket.Close();
                return false;
            }

            this.Started = true;
            ConsoleUtils.ShowInfo(
                "Connecting to {0}:{1}",
                this.IpEndPoint.Address.ToString(),
                this.IpEndPoint.Port
            );

            return true;
        }

        /// <summary>
		/// Triggered when it connects to Auth
		/// </summary>
		/// <param name="ar"></param>
		private void ConnectCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            if (!socket.Connected)
            {
                ConsoleUtils.ShowWarning(
                    "Could not connect to {0}:{1}. Check your settings and try again...\nAt {2}",
                    this.IpEndPoint.Address.ToString(),
                    this.IpEndPoint.Port,
                    "SocketService.ConnectCallback"
                );
                return;
            }
            socket.EndConnect(ar);

            // Initializes session
            Session session = this._SessionFactory.CreateSession(socket);

            // Starts to receive data
            socket.BeginReceive(
                session._NetworkData.Buffer,
                0,
                NetworkData.MaxBuffer,
                SocketFlags.None,
                new AsyncCallback(ReadCallback),
                session
            );
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
                    this.SocketDisconnected(session);
                    session._NetworkData._Socket.Close();
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
                            bytesToRead -= byteCount;
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
            catch (SocketException e)
            {
                // 10054 : Socket closed, not an error
				if (!(e.ErrorCode == 10054))
					ConsoleUtils.ShowError(e.Message);

                this.SocketDisconnected(session);
                session._NetworkData._Socket.Close();
            }
            catch (Exception e)
            {
                ConsoleUtils.ShowError(e.Message);
                this.SocketDisconnected(session);
                session._NetworkData._Socket.Close();
            }
        }

        /// <summary>
        /// Called when a packet is completely received.
        /// </summary>
        /// <param name="session">The session who sent this packet</param>
        /// <param name="packet">Packet data</param>
        public void PacketReceived(Session session, byte[] packet)
        {
            ConsoleUtils.HexDump(packet, "Packet Received");
            Task.Factory.StartNew(() => { this.Controller.ProcessRequest(session, packet); });
        }

        /// <summary>
        /// Sends a Packet to a client
        /// </summary>
        /// <param name="session"></param>
        /// <param name="packet"></param>
        public void SendPacket(Session session, Packet packet)
        {
            byte[] data = packet.Write();

            ConsoleUtils.HexDump(data, "Sending Packet");

            if (this.Encrypted)
            {
                data = session._NetworkData.OutCipher.DoCipher(ref data);
            }

            session._NetworkData._Socket.BeginSend(
                data,
                0,
                data.Length,
                SocketFlags.None,
                new AsyncCallback(SendCallback),
                session
            );
        }

        /// <summary>
        /// Called when packet send is complete
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Session session = (Session)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = session._NetworkData._Socket.EndSend(ar);
            }
            catch (Exception)
            {
                ConsoleUtils.ShowNotice("Failed to send packet to client.");
            }
        }

        private void SocketDisconnected(Session session)
        {
            if (this.OnSocketDisconnect != null)
            {
                this.OnSocketDisconnect(session);
            }
        }
    }

}

