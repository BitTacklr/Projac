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
public class PortfolioProjection : ConnectecProjection<ConnectionMultiplexer>
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
      }, message.Id.ToString("N")));

    When<PortfolioRemoved>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id.ToString("N"));
        session.Delete(document);
      });

    When<PortfolioRenamed>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id.ToString("N"));
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
