using System;
using System.Data;
using System.Data.Common;

namespace Paramol.Tests.Framework
{
    public class TestDbTransaction : DbTransaction
    {
        private readonly TestDbConnection _connection;

        public TestDbTransaction(TestDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            _connection = connection;
        }

        public override void Commit()
        {
        }

        public override void Rollback()
        {
        }

        protected override DbConnection DbConnection
        {
            get { return _connection; }
        }

        public override IsolationLevel IsolationLevel
        {
            get { return IsolationLevel.Unspecified; }
        }
    }
}