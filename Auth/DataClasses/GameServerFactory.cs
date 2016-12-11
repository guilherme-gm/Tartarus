using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common.DataClasses;

namespace Auth.DataClasses
{
    public class GameServerFactory : SessionFactory
    {
        public override Session CreateSession(Socket socket)
        {
            Session session = new Session();
            session._Client = new GameServer();
            session._NetworkData = new NetworkData(socket);

            return session;
        }
    }
}
