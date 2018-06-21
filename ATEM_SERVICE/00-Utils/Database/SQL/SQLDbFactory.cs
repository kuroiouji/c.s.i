using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.SQL
{
    public enum SQLDbType
    {
        SQLServer
    }

    public class Factory
    {
        public static ISQLDb Create(SQLDbType type, object context)
        {
            if (type == SQLDbType.SQLServer)
                return new Utils.SQL.SQLSvrDb(context);

            return null;
        }
    }
}
