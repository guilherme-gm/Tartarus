using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataClasses
{
    public abstract class SessionFactory
    {
        public abstract Session CreateSession(Socket socket);
    }
}
