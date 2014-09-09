using System.Data;
using System.Data.Common;
using Paramol;

namespace Projac.Tests.Framework
{
    public class SqlNonQueryCommandStub : SqlNonQueryCommand
    {
        public SqlNonQueryCommandStub(string text, DbParameter[] parameters, CommandType type) : base(text, parameters, type)
        {
        }
    }
}