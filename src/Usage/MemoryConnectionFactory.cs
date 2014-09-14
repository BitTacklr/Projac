using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using NEventStore.Persistence.Sql;

namespace Usage
{
    public class MemoryConnectionFactory : IConnectionFactory
    {
        private readonly ConnectionStringSettings _settings;
        private readonly DbProviderFactory _dbProviderFactory;

        public MemoryConnectionFactory(ConnectionStringSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _dbProviderFactory = DbProviderFactories.GetFactory(settings.ProviderName);
        }

        public IDbConnection Open()
        {
            var connection = _dbProviderFactory.CreateConnection();
            connection.ConnectionString = _settings.ConnectionString;
            connection.Open();
            return connection;
        }

        public Type GetDbProviderFactoryType()
        {
            return _dbProviderFactory.GetType();
        }
    }
}