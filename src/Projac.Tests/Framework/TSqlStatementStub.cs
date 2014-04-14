using System.Data.SqlClient;

namespace Projac.Tests.Framework
{
    public class TSqlStatementStub : ITSqlStatement
    {
        public void WriteTo(SqlCommand command)
        {
        }
    }
}