using System.Data;
using System.Data.Common;

namespace Paramol.Tests.Framework
{
    public class TestDbConnection : DbConnection
    {
        private ConnectionState _connectionState;

        public TestDbConnection()
        {
            _connectionState = ConnectionState.Closed;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return null;
        }

        public override void Close()
        {
        }

        public override void ChangeDatabase(string databaseName)
        {
        }

        public override void Open()
        {
            _connectionState = ConnectionState.Open;
        }

        public override string ConnectionString { get; set; }

        public override string Database
        {
            get { return ""; }
        }

        public override ConnectionState State
        {
            get { return _connectionState; }
        }

        public override string DataSource
        {
            get { return ""; }
        }

        public override string ServerVersion
        {
            get { return ""; }
        }

        protected override DbCommand CreateDbCommand()
        {
            return null;
        }
    }
}