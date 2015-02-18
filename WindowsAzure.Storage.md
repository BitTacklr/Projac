# Projac.WindowsAzure.Storage

Projac.WindowsAzure.Storage brings Projac's declarative style of authoring projections to projections that target Microsoft Windows Azure Table Storage (TM).

It's available on NuGet: [Projac.WindowsAzure.Storage](https://www.nuget.org/packages/Projac.WindowsAzure.Storage/)

# Authoring projections

## The Declarative Style

Similar to Projac's declarative style for sql projections above, one can author projections that target Microsoft Windows Azure Table Storage in a declarative way. A fundamental difference is that the handling and interpretation of messages is directly tied to the execution of actions against Table Storage. Why? Because replicating the entire Table Storage api into a set of statements was not a goal at this point. Only asynchronous projection handling is supported at the moment.

```csharp
var projection =
  new CloudTableProjectionBuilder().
    When<RebuildProjection>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.CreateIfNotExistsAsync();
    }).
    When<PortfolioAdded>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.ExecuteAsync(
        TableOperation.Insert(
          new PortfolioModel(message.Id)
          {
            Name = message.Name
          }));
    }).
    When<PortfolioRemoved>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.ExecuteAsync(
        TableOperation.Delete(new PortfolioModel(message.Id)
        {
          ETag = "*"
        }));   
    }).
    When<PortfolioRenamed>((client, message) =>
    {
      var table = client.GetTableReference("Portfolio");
      return table.ExecuteAsync(
        TableOperation.Merge(new PortfolioModel(message.Id)
        {
          Name = message.Name,
          ETag = "*"
        }));
    }).
    Build();
```

# Executing projections

How and when you decide to execute the projections is still left as an exercise to you. Typically they will sit behind a message subscription that pushes the appropriate messages into them, causing the execution of actions against Table Storage. You can use the AsyncCloudTableProjector to perform the actual execution.