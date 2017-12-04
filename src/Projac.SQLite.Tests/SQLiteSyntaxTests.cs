using NUnit.Framework;
using Projac.SQLite;

namespace Projac.SQLite.Tests
{
    [TestFixture]
    public partial class SQLiteSyntaxTests
    {
        private SQLiteSyntax _syntax;

        [SetUp]
        public void SetUp()
        {
            _syntax = new SQLiteSyntax();
        }

        private SQLiteSyntax Sql { get { return _syntax; } }
    }
}
