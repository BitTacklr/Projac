using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Paramol;
using Paramol.Executors;
using Paramol.SQLite;
using Projac;
using Recipes.Shared;

namespace Recipes.SQLiteIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class Usage
    {
        private string _databasePath;

        [SetUp]
        public void SetUp()
        {
            _databasePath = Path.GetTempFileName();
            SQLiteConnection.CreateFile(_databasePath);
        }

        [TearDown]
        public void TearDown()
        {
            SQLiteConnection.ClearAllPools();
            if(File.Exists(_databasePath)) File.Delete(_databasePath);
        }

        [Test]
        public Task Show()
        {
            var builder = new SQLiteConnectionStringBuilder
            {
                BinaryGUID = true,
                DataSource = _databasePath,
                DateTimeFormat = SQLiteDateFormats.ISO8601,
                FailIfMissing = true,
                DateTimeKind = DateTimeKind.Utc,
                JournalMode = SQLiteJournalModeEnum.Wal,
                SyncMode = SynchronizationModes.Full
            };

            var projector =
                new AsyncSqlProjector(
                    Resolve.WhenEqualToHandlerMessageType(new PortfolioProjection()),
                    new SQLiteExecutor(() => new SQLiteConnection(builder.ToString()))
                    );

            var portfolioId = Guid.NewGuid();
            return projector.ProjectAsync(new object[]
            {
                new CreateSchema(), 
                new PortfolioAdded { Id = portfolioId, Name = "My portfolio" },
                new PortfolioRenamed { Id = portfolioId, Name = "Your portfolio" },
                new PortfolioRemoved { Id = portfolioId }
            });
        }

        public class PortfolioProjection : SQLiteProjection
        {
            public PortfolioProjection()
            {
                When<PortfolioAdded>(@event =>
                    Sql.NonQueryStatement(
                        "INSERT INTO [Portfolio] ([Id], [Name], [PhotoCount]) VALUES (@P1, @P2, 0)",
                        new { P1 = Sql.Guid(@event.Id), P2 = Sql.String(@event.Name) }
                        ));
                When<PortfolioRemoved>(@event =>
                    Sql.NonQueryStatement(
                        "DELETE FROM [Portfolio] WHERE [Id] = @P1",
                        new { P1 = Sql.Guid(@event.Id) }
                        ));
                When<PortfolioRenamed>(@event =>
                    Sql.NonQueryStatement(
                        "UPDATE [Portfolio] SET [Name] = @P2 WHERE [Id] = @P1",
                        new { P1 = Sql.Guid(@event.Id), P2 = Sql.String(@event.Name) }
                        ));
                When<CreateSchema>(_ =>
                    Sql.NonQueryStatement(
                        @"CREATE TABLE [Portfolio] (
                            [Id] BLOB NOT NULL CONSTRAINT PK_Portfolio PRIMARY KEY,
                            [Name] NVARCHAR NOT NULL,
                            [PhotoCount] INT NOT NULL)"));
            }
        }

        public class SQLiteProjection : SqlProjection
        {
            private static readonly SQLiteSyntax Syntax = new SQLiteSyntax();

            protected SQLiteSyntax Sql
            {
                get { return Syntax; }
            }
        }

        public class CreateSchema
        {
        }

        //TODO: Executors need to become decoupled from ancient DbProviderFactory.
        public class SQLiteExecutor : IAsyncSqlNonQueryCommandExecutor
        {
            private readonly Func<SQLiteConnection> _connectionFactory;
            private readonly int _commandTimeout;

            public SQLiteExecutor(Func<SQLiteConnection> connectionFactory, int commandTimeout = 30)
            {
                if (connectionFactory == null)
                    throw new ArgumentNullException("connectionFactory");
                _connectionFactory = connectionFactory;
                _commandTimeout = commandTimeout;
            }

            public Task ExecuteNonQueryAsync(SqlNonQueryCommand command)
            {
                if (command == null)
                    throw new ArgumentNullException("command");
                return ExecuteNonQueryAsync(command, CancellationToken.None);
            }

            public async Task ExecuteNonQueryAsync(SqlNonQueryCommand command, CancellationToken cancellationToken)
            {
                if (command == null)
                    throw new ArgumentNullException("command");

                using (var dbConnection = _connectionFactory())
                {
                    await dbConnection.OpenAsync(cancellationToken);
                    try
                    {
                        using (var dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.Connection = dbConnection;
                            dbCommand.CommandTimeout = _commandTimeout;
                            dbCommand.CommandType = command.Type;
                            dbCommand.CommandText = command.Text;
                            dbCommand.Parameters.AddRange(command.Parameters);
                            await dbCommand.ExecuteNonQueryAsync(cancellationToken);
                        }
                    }
                    finally
                    {
                        dbConnection.Close();
                    }
                }
            }

            public Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands)
            {
                if (commands == null)
                    throw new ArgumentNullException("commands");
                return ExecuteNonQueryAsync(commands, CancellationToken.None);
            }

            public async Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands, CancellationToken cancellationToken)
            {
                if (commands == null)
                    throw new ArgumentNullException("commands");
                using (var dbConnection = _connectionFactory())
                {
                    await dbConnection.OpenAsync(cancellationToken);
                    try
                    {
                        using (var dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.Connection = dbConnection;
                            dbCommand.CommandTimeout = _commandTimeout;
                            var count = 0;
                            foreach (var command in commands)
                            {
                                dbCommand.CommandType = command.Type;
                                dbCommand.CommandText = command.Text;
                                dbCommand.Parameters.Clear();
                                dbCommand.Parameters.AddRange(command.Parameters);
                                await dbCommand.ExecuteNonQueryAsync(cancellationToken);
                                count++;
                            }
                            return count;
                        }
                    }
                    finally
                    {
                        dbConnection.Close();
                    }
                }
            }
        }
    }
}
