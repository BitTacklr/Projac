using NUnit.Framework;
using Projac.SqlClient;

namespace Projac.Sql.Tests.SqlClient
{
    [TestFixture]
    public partial class SqlClientSyntaxTests
    {
        private SqlClientSyntax _syntax;

        [SetUp]
        public void SetUp()
        {
            _syntax = new SqlClientSyntax();
        }

        private SqlClientSyntax Sql { get { return _syntax; } }
    }
}
