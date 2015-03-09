# Projac.Elasticsearch

Projac.Elasticsearch brings Projac's declarative style of authoring projections to projections that target Elasticsearch (TM).

It's available on NuGet: [Projac.Elasticsearch](https://www.nuget.org/packages/Projac.Elasticsearch/)

# Authoring projections

## The Declarative Style

Similar to Projac's declarative style for sql projections above, one can author projections that target Elasticsearch in a declarative way. A fundamental difference is that the handling and interpretation of messages is directly tied to the execution of actions against Elasticsearch. Why? Because replicating the entire Elasticsearch api into a set of statements was not a goal at this point. Only asynchronous projection handling is supported at the moment.

```csharp
var projection =
  new ElasticsearchProjectionBuilder().
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

How and when you decide to execute the projections is still left as an exercise to you. Typically they will sit behind a message subscription that pushes the appropriate messages into them, causing the execution of actions against Elasticsearch. You can use the AsyncElasticsearchProjector to perform the actual execution.