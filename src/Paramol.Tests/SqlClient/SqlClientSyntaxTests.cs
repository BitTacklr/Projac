using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
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
