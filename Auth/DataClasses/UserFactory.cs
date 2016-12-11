using Common.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common.RC4;

namespace Auth.DataClasses
{
    public class UserFactory : SessionFactory
    {
        public const string RC4Key = "}h79q~B%al;k'y $E";

        public override Session CreateSession(Socket socket)
        {
            Session session = new Session();
            session._Client = new User();
            session._NetworkData = new NetworkData(socket);
            session._NetworkData.InCipher = new XRC4Cipher(RC4Key);
            session._NetworkData.OutCipher = new XRC4Cipher(RC4Key);

            return session;
        }
    }
}
