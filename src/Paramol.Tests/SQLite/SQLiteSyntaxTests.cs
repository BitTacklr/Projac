using NUnit.Framework;
using Paramol.SQLite;

namespace Paramol.Tests.SQLite
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
