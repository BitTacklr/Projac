using System;
using System.Data.Common;
using System.Data.SQLite;
using Projac.Sql;

namespace Projac.SQLite.Tests
{
    internal class SQLiteParameterValueStub : IDbParameterValue
    {
        public DbParameter ToDbParameter(string parameterName)
        {
            return new SQLiteParameter { ParameterName = parameterName, Value = DBNull.Value };
        }
    }
}