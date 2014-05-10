using System;

namespace Projac.Tests.Framework
{
    internal abstract class SqlServerInstanceDiscoveryResult
    {
        public static readonly SqlServerInstanceDiscoveryResult NotFound = new SqlServerInstanceNotFound();

        public static SqlServerInstanceDiscoveryResult Found(string dataSource)
        {
            return new SqlServerInstanceFound(dataSource);
        }

        public abstract string DataSource { get; }

        class SqlServerInstanceNotFound : SqlServerInstanceDiscoveryResult
        {
            public override string DataSource
            {
                get { return null; }
            }
        }

        class SqlServerInstanceFound : SqlServerInstanceDiscoveryResult
        {
            private readonly string _dataSource;

            public SqlServerInstanceFound(string dataSource)
            {
                if (dataSource == null) throw new ArgumentNullException("dataSource");
                _dataSource = dataSource;
            }

            public override string DataSource
            {
                get { return _dataSource; }
            }
        }
    }
}