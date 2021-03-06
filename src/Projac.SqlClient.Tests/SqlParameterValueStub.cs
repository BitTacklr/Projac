﻿using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Projac.Sql.Tests.SqlClient
{
    internal class SqlParameterValueStub : IDbParameterValue
    {
        public DbParameter ToDbParameter(string parameterName)
        {
            return new SqlParameter { ParameterName = parameterName, Value = DBNull.Value };
        }
    }
}
