# Projac.Connector

Projac.Connector brings Projac's declarative style of authoring projections to projections that target any store for which you can bring your own connection.

It's available on NuGet: [Projac.Connector](https://www.nuget.org/packages/Projac.Connector/)

# Authoring projections

## The Declarative Style

Similar to Projac's declarative style for sql projections, one can author projections that target Elasticsearch, Redis, RavenDb, WindowsAzure Table Storage, etc ... in a declarative way. A fundamental difference is that the handling and interpretation of messages is directly tied to the execution of actions against the respective store. Why? Because replicating the entire connection api into a set of statements would be too ambitious and brittle a goal. Only asynchronous projection handling is supported at the moment. Given we're dealing with I/O intensive operations that decision seems reasonable.

### Elasticsearch

```csharp
public class PortfolioProjection : ConnectedProjection<ElasticsearchClient>
{
  public PortfolioProjection()
  {
    When<PortfolioAdded>((client, message) =>
      client.IndexAsync(
        "index",
        "portfolio",
        message.Id.ToString("N"),
        JsonConvert.SerializeObject(new
        {
          name = message.Name
        })));

    When<PortfolioRemoved>((client, message) =>
      client.DeleteAsync(
        "index",
        "portfolio",
        message.Id.ToString("N")));

    When<PortfolioRenamed>((client, message) =>
      client.UpdateAsync(
        "index",
        "portfolio",
        message.Id.ToString("N"),
        JsonConvert.SerializeObject(new
          {
            Script = "ctx._source.name=name;",
            Params = new
              {
                name = message.Name
              }
          })));
  }
}
```

### Redis

```csharp
public class PortfolioProjection : ConnectedProjection<ConnectionMultiplexer>
{
  public PortfolioProjection()
  {
    When<PortfolioAdded>((connection, message) =>
    {
      var db = connection.GetDatabase();
      return db.HashSetAsync(message.Id.ToString("N"), "Name", message.Name);
    });

    When<PortfolioRemoved>((connection, message) =>
    {
      var db = connection.GetDatabase();
      return db.HashDeleteAsync(message.Id.ToString("N"), "Name");
    });

    When<PortfolioRenamed>((connection, message) =>
    {
      var db = connection.GetDatabase();
      return db.HashSetAsync(message.Id.ToString("N"), "Name", message.Name);
    });
  }
}
```

### RavenDb

```csharp
public class PortfolioProjection : ConnectedProjection<IAsyncDocumentSession>
{
  public PortfolioProjection()
  {
    When<PortfolioAdded>((session, message) => 
      session.StoreAsync(
      new PortfolioDocument
      {
        Id = message.Id,
        Name = message.Name
      }));

    When<PortfolioRemoved>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id);
        session.Delete(document);
      });

    When<PortfolioRenamed>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id);
        document.Name = message.Name;
      });
  }
}
```

### Windows Azure Table Storage

```csharp
public class PortfolioProjection : ConnectedProjection<CloudTableClient>
{
  public PortfolioProjection()
  {
    When<RebuildProjection>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.CreateIfNotExistsAsync();
    });

    When<PortfolioAdded>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.ExecuteAsync(
        TableOperation.Insert(
          new PortfolioModel(message.Id)
          {
            Name = message.Name
          }));
    });

    When<PortfolioRemoved>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.ExecuteAsync(
        TableOperation.Delete(new PortfolioModel(message.Id)
        {
          ETag = "*"
        }));   
    });

    When<PortfolioRenamed>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.ExecuteAsync(
        TableOperation.Merge(new PortfolioModel(message.Id)
        {
          Name = message.Name,
          ETag = "*"
        }));
    });
  }
}
```

### Anonymous

Next to the *ConnectedProjection* approach, one can also use the *AnonymousConnectedProjectionBuilder* approach if you prefer to go class-less.

```csharp
var projection =
  new AnonymousConnectedProjectionBuilder<ElasticsearchClient>().
    When<PortfolioAdded>((client, message) =>
      client.IndexAsync(
        "index",
        "portfolio",
        message.Id.ToString("N"),
        JsonConvert.SerializeObject(new
        {
          name = message.Name
        }))).
    When<PortfolioRemoved>((client, message) =>
      client.DeleteAsync(
        "index",
        "portfolio",
        message.Id.ToString("N"))).
    When<PortfolioRenamed>((client, message) =>
      client.UpdateAsync(
        "index",
        "portfolio", 
        message.Id.ToString("N"), 
        JsonConvert.SerializeObject(new
          {
            Script = "ctx._source.name=name;",
            Params = new
              {
                name = message.Name
              }
          }))).
    Build();
```

# Executing projections

How and when you decide to execute the projections is still left as an exercise to you. Typically they will sit behind a message subscription that pushes the appropriate messages into them, causing the execution of actions against the store. You can use the ConnectedProjector to perform the actual execution.

# Testing projections

Projac.Connector comes with an *API* that allows you to write tests for your projections at the right level of abstraction. Depending on the store you are integrating with, you may want to extend the syntax with extension methods that tuck away any boilerplate code. The general idea is that you author a scenario using *givens*, which are just messages, and verify that the store contains the expected data (and only the expected data) after projecting those messages using the projection under test.

Given the following RavenDb projection ...

```csharp
public class PortfolioProjection : ConnectedProjection<IAsyncDocumentSession>
{
  public PortfolioProjection()
  {
    When<PortfolioAdded>((session, message) => 
      session.StoreAsync(
      new PortfolioDocument
      {
        Id = message.Id,
        Name = message.Name
      }));

    When<PortfolioRemoved>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id);
        session.Delete(document);
      });

    When<PortfolioRenamed>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id);
        document.Name = message.Name;
      });
  }
}
```

... we could test it as shown below ...

```csharp
[TestFixture]
public class PortfolioProjectionTests
{
  [Test]
  public Task when_a_portfolio_was_deleted()
  {
      var portfolioId = Guid.NewGuid();
      return RavenProjectionScenario.For(new PortfolioProjection())
          .Given(
              new PortfolioAdded { Id = portfolioId, Name = "My portfolio" },
              new PortfolioRemoved { Id = portfolioId }
          )
          .ExpectNone();
  }

  [Test]
  public Task when_a_portfolio_was_renamed()
  {
      var portfolioId = Guid.NewGuid();
      return RavenProjectionScenario.For(new PortfolioProjection())
          .Given(
              new PortfolioAdded { Id = portfolioId, Name = "My portfolio" },
              new PortfolioRenamed { Id = portfolioId, Name = "Your portfolio" }
          )
          .Expect(new PortfolioDocument
          {
              Id = portfolioId,
              Name = "Your portfolio"
          });
  }
}
```

... using the following test syntax extension methods ...

```csharp
public static class RavenProjectionScenario
{
    public static ConnectedProjectionScenario<IAsyncDocumentSession> For(
        ConnectedProjectionHandler<IAsyncDocumentSession>[] handlers)
    {
        if (handlers == null) throw new ArgumentNullException("handlers");
        return new ConnectedProjectionScenario<IAsyncDocumentSession>(
            ConcurrentResolve.WhenEqualToHandlerMessageType(handlers));
    }

    public static Task ExpectNone(this ConnectedProjectionScenario<IAsyncDocumentSession> scenario)
    {
        return scenario
            .Verify(async session =>
            {
                using (var streamer = await session.Advanced.StreamAsync<RavenJObject>(Etag.Empty))
                {
                    if (await streamer.MoveNextAsync())
                    {
                        var storedDocumentIdentifiers = new List<string>();
                        do
                        {
                            storedDocumentIdentifiers.Add(streamer.Current.Key);
                        } while (await streamer.MoveNextAsync());

                        return VerificationResult.Fail(
                            string.Format("Expected no documents, but found {0} document(s) ({1}).",
                                storedDocumentIdentifiers.Count,
                                string.Join(",", storedDocumentIdentifiers)));
                    }
                    return VerificationResult.Pass();
                }
            })
            .Assert();
    }

    public static Task Expect(this ConnectedProjectionScenario<IAsyncDocumentSession> scenario, params object[] documents)
    {
        if (documents == null) 
            throw new ArgumentNullException("documents");

        if (documents.Length == 0)
        {
            return scenario.ExpectNone();
        }
        return scenario
            .Verify(async session =>
            {
                using (var streamer = await session.Advanced.StreamAsync<object>(Etag.Empty))
                {
                    var storedDocuments = new List<object>();
                    var storedDocumentIdentifiers = new List<string>();
                    while (await streamer.MoveNextAsync())
                    {
                        storedDocumentIdentifiers.Add(streamer.Current.Key);
                        storedDocuments.Add(streamer.Current.Document);
                    }

                    if (documents.Length != storedDocumentIdentifiers.Count)
                    {
                        if (storedDocumentIdentifiers.Count == 0)
                        {
                            return VerificationResult.Fail(
                                string.Format("Expected {0} document(s), but found 0 documents.",
                                    documents.Length));
                        }
                        return VerificationResult.Fail(
                            string.Format("Expected {0} document(s), but found {1} document(s) ({2}).",
                                documents.Length,
                                storedDocumentIdentifiers.Count,
                                string.Join(",", storedDocumentIdentifiers)));
                    }

                    var expectedDocuments = documents.Select(JToken.FromObject).ToArray();
                    var actualDocuments = storedDocuments.Select(JToken.FromObject).ToArray();

                    if (!expectedDocuments.SequenceEqual(actualDocuments, new JTokenEqualityComparer()))
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine("Expected the following documents:");
                        foreach (var expectedDocument in expectedDocuments)
                        {
                            builder.AppendLine(expectedDocument.ToString());
                        }
                        builder.AppendLine();
                        builder.AppendLine("But found the following documents:");
                        foreach (var actualDocument in actualDocuments)
                        {
                            builder.AppendLine(actualDocument.ToString());
                        }
                        return VerificationResult.Fail(builder.ToString());
                    }
                    return VerificationResult.Pass();
                }
            })
            .Assert();
    }

    public static async Task Assert(this ConnectedProjectionTestSpecification<IAsyncDocumentSession> specification)
    {
        if (specification == null) throw new ArgumentNullException("specification");
        using (var store = new EmbeddableDocumentStore
        {
            RunInMemory = true,
            DataDirectory = Path.GetTempPath()
        })
        {
            store.Configuration.Storage.Voron.AllowOn32Bits = true;
            store.Initialize();
            using (var session = store.OpenAsyncSession())
            {
                await new ConnectedProjector<IAsyncDocumentSession>(specification.Resolver).
                    ProjectAsync(session, specification.Messages);
                await session.SaveChangesAsync();

                var result = await specification.Verification(session, CancellationToken.None);
                if (result.Failed)
                {
                    throw new AssertionException(result.Message);
                }
            }
        }
    }
}
```
