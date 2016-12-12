using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Utils
{
    interface IConnectionFactory
    {
        DbConnection GetConnection();

        void Close(DbConnection con);
    }
}
