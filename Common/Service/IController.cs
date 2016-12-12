using Common.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service
{
    public interface IController
    {
        void ProcessRequest(Session session, byte[] data);
    }
}
