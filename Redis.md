# Projac.Redis

Projac.Redis brings Projac's declarative style of authoring projections to projections that target Redis (TM).

It's available on NuGet: [Projac.Redis](https://www.nuget.org/packages/Projac.Redis/)

# Authoring projections

## The Declarative Style

Similar to Projac's declarative style for sql projections above, one can author projections that target Redis in a declarative way. A fundamental difference is that the handling and interpretation of messages is directly tied to the execution of actions against Redis. Why? Because replicating the entire Redis api into a set of statements was not a goal at this point. Only asynchronous projection handling is supported at the moment.

```csharp
var projection =
  new RedisProjectionBuilder().
    When<PortfolioAdded>((connection, message) =>
    {
      var db = connection.GetDatabase();
      return db.HashSetAsync(message.Id.ToString("N"), "Name", message.Name);
    }).
    When<PortfolioRemoved>((connection, message) =>
    {
      var db = connection.GetDatabase();
      return db.HashDeleteAsync(message.Id.ToString("N"), "Name");
    }).
    When<PortfolioRenamed>((connection, message) =>
    {
      var db = connection.GetDatabase();
      return db.HashSetAsync(message.Id.ToString("N"), "Name", message.Name);
    }).
    Build();
```

# Executing projections

How and when you decide to execute the projections is still left as an exercise to you. Typically they will sit behind a message subscription that pushes the appropriate messages into them, causing the execution of actions against Redis. You can use the AsyncRedisProjector to perform the actual execution.