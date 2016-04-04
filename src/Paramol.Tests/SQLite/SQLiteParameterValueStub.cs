using System;
using System.Data.Common;
using System.Data.SQLite;

namespace Paramol.Tests.SQLite
{
    internal class SQLiteParameterValueStub : IDbParameterValue
    {
        public DbParameter ToDbParameter(string parameterName)
        {
            return new SQLiteParameter { ParameterName = parameterName, Value = DBNull.Value };
        }
    }
}