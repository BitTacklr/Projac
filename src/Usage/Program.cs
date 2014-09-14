using System;
using System.Configuration;
using System.Data.SqlClient;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using Usage.Messages;

namespace Usage
{
    class Program
    {
        static void Main()
        {
            var settings = new ConnectionStringSettings(
                "ProjacUsage",
                new SqlConnectionStringBuilder
                {
                    DataSource = "(localdb)\\ProjectsV12",
                    InitialCatalog = "ProjacUsage",
                    IntegratedSecurity = true
                }.ConnectionString,
                "System.Data.SqlClient");

            var eStore = Wireup.Init().
                UsingSqlPersistence(new MemoryConnectionFactory(settings)).
                WithDialect(new MsSqlDialect()).
                InitializeStorageEngine().
                UsingJsonSerialization().
                DoNotDispatchCommits().
                Build();

            var host = new SqlProjectionHost(settings, new[]
            {
                PortfolioProjection.Descriptor
            });
            host.Initialize();
            host.Start(eStore.Advanced);

            SimulateEventProduction(eStore);

            Console.ReadLine();
        }

        private static void SimulateEventProduction(IStoreEvents eStore)
        {
            var random = new Random();
            var ticks = DateTime.Now.Ticks;
            for (var i = 0; i < 1000; i++)
            {
                var id = Guid.NewGuid();
                var streamId = string.Format("{0}-{1}", ticks, i);
                using (var stream = eStore.CreateStream(streamId))
                {
                    stream.Add(new EventMessage
                    {
                        Body = new PortfolioAdded
                        {
                            Id = id,
                            Name = streamId
                        }
                    });
                    if(random.Next() % random.Next(1, 10) == 0)
                        stream.Add(new EventMessage
                        {
                            Body = new PortfolioRenamed
                            {
                                Id = id,
                                Name = "renamed-" + streamId
                            }
                        });
                    if (random.Next() % 99 == 0)
                        stream.Add(new EventMessage
                        {
                            Body = new PortfolioRemoved
                            {
                                Id = id
                            }
                        });
                    stream.CommitChanges(Guid.NewGuid());
                }
            }
        }
    }
}
