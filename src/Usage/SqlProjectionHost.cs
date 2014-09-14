using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NEventStore.Client;
using NEventStore.Persistence;
using Paramol.Executors;
using Paramol.SqlClient;
using Projac;
using Usage.Framework;
using Usage.SystemMessages;

namespace Usage
{
    public class SqlProjectionHost
    {
        private readonly ConnectionStringSettings _settings;
        private readonly SqlProjectionDescriptor[] _descriptors;
        private readonly List<PollingClient> _clients;
        private readonly List<IDisposable> _disposables;
        private readonly List<Task> _runningObservers;

        private static readonly SqlProjection HostProjection = new SqlProjectionBuilder().
            When<BuildProjection>(_ =>
                TSql.NonQueryStatement(
                    "INSERT INTO [dbo].[Projac] ([Id],[Identifier],[Version],[Checkpoint]) " +
                    "VALUES (@Id,@Identifier,@Version,@Checkpoint)",
                    new
                    {
                        Id = TSql.Binary(_.Identifier.ToMD5Hash(), 16),
                        Identifier = TSql.NVarCharMax(_.Identifier),
                        Version = TSql.NVarCharMax(_.Version),
                        Checkpoint = TSql.BigInt(Checkpoint.None)
                    })).
            When<RebuildProjection>(_ =>
                TSql.NonQueryStatement(
                    "UPDATE [dbo].[Projac] SET [Version] = @Version, [Checkpoint] = @Checkpoint " +
                    "WHERE [Id] = @Id",
                    new
                    {
                        Id = TSql.Binary(_.Identifier.ToMD5Hash(), 16),
                        Version = TSql.NVarCharMax(_.NewVersion),
                        Checkpoint = TSql.BigInt(Checkpoint.None)
                    })).
            When<SetProjectionCheckpoint>(_ =>
                TSql.NonQueryStatement(
                    "UPDATE [dbo].[Projac] SET [Checkpoint] = @Checkpoint " +
                    "WHERE [Id] = @Id",
                    new
                    {
                        Id = TSql.Binary(_.Identifier.ToMD5Hash(), 16),
                        Checkpoint = TSql.BigInt(_.Checkpoint)
                    })).
            Build();

        public SqlProjectionHost(ConnectionStringSettings settings, SqlProjectionDescriptor[] descriptors)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (descriptors == null) throw new ArgumentNullException("descriptors");
            _settings = settings;
            _descriptors = descriptors;
            _clients = new List<PollingClient>();
            _disposables = new List<IDisposable>();
            _runningObservers =new List<Task>();
        }

        public void Initialize()
        {
            InitializeHostSchemaAsync();
            InitializeProjectionSchemaAsync();
        }

        private void InitializeHostSchemaAsync()
        {
            new SqlCommandExecutor(_settings).
                ExecuteNonQuery(
                    TSql.NonQueryStatement(
@"IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Projac'))
BEGIN
	EXEC('CREATE TABLE [dbo].[Projac] (
		    [Id] BINARY(16) NOT NULL CONSTRAINT [PK_Projac] PRIMARY KEY, 
		    [Identifier] NVARCHAR(MAX) NOT NULL, 
		    [Version] NVARCHAR(MAX) NOT NULL, 
		    [Checkpoint] BIGINT NOT NULL
          )')
END"));
        }

        private void InitializeProjectionSchemaAsync()
        {
            var queryExecutor = new SqlCommandExecutor(_settings);
            var nonQueryExecutor = new TransactionalSqlCommandExecutor(_settings, IsolationLevel.Serializable);

            foreach (var descriptor in _descriptors)
            {
                var storedDescriptor = queryExecutor.
                    ExecuteReader(
                        TSql.QueryStatementFormat(
                            @"SELECT [Identifier], [Version], [Checkpoint] FROM [dbo].[Projac] WHERE [Id] = {0}",
                            TSql.Binary(descriptor.Identifier.ToMD5Hash(), 16))).
                    Read<StoredDescriptor>().
                    SingleOrDefault();

                if (storedDescriptor != null)
                {
                    if (storedDescriptor.Version != descriptor.Version)
                    {
                        //Rebuild
                        new SqlProjector(descriptor.SchemaProjection.Concat(HostProjection), nonQueryExecutor).
                            Project(
                                new RebuildProjection(
                                    descriptor.Identifier,
                                    storedDescriptor.Version,
                                    descriptor.Version));
                    }
                }
                else
                {
                    //Build
                    new SqlProjector(descriptor.SchemaProjection.Concat(HostProjection), nonQueryExecutor).
                        Project(
                            new BuildProjection(
                                descriptor.Identifier,
                                descriptor.Version));
                }
            }
        }

        public void Start(IPersistStreams connection)
        {
            if (connection == null) 
                throw new ArgumentNullException("connection");

            var queryExecutor = new SqlCommandExecutor(_settings);
            foreach (var descriptor in _descriptors)
            {
                var storedDescriptor = queryExecutor.
                    ExecuteReader(
                        TSql.QueryStatementFormat(
                            @"SELECT [Identifier], [Version], [Checkpoint] FROM [dbo].[Projac] WHERE [Id] = {0}",
                            TSql.Binary(descriptor.Identifier.ToMD5Hash(), 16))).
                    Read<StoredDescriptor>().
                    Single();

                var client = new PollingClient(connection);
                _clients.Add(client);
                var observer = client.ObserveFrom(storedDescriptor.Checkpoint.ToString(CultureInfo.InvariantCulture));
                _disposables.Add(observer);
                var subscription = observer.
                    Subscribe(
                        new CommitObserver(
                            descriptor.Identifier,
                            new SqlProjector(
                                descriptor.Projection.Concat(HostProjection),
                                new TransactionalSqlCommandExecutor(_settings, IsolationLevel.ReadCommitted)))
                    );
                _disposables.Add(subscription);
                _runningObservers.Add(observer.Start());
            }
        }

        class StoredDescriptor
        {
            public string Identifier { get; set; }
            public string Version { get; set; }
            public long Checkpoint { get; set; }
        }
    }
}
