using System.Data;
using System.Data.Common;

namespace Projac.Sql.Tests.Framework
{
    public class SqlNonQueryCommandStub : SqlNonQueryCommand
    {
        public SqlNonQueryCommandStub(string text, DbParameter[] parameters, CommandType type) : base(text, parameters, type)
        {
        }
    }
}