# Projac.RavenDB

Projac.RavenDB brings Projac's declarative style of authoring projections to projections that target RavenDB (TM).

It's available on NuGet: [Projac.RavenDB](https://www.nuget.org/packages/Projac.RavenDB/)

# Authoring projections

## The Declarative Style

Similar to Projac's declarative style for sql projections above, one can author projections that target RavenDB in a declarative way. A fundamental difference is that the handling and interpretation of messages is directly tied to the execution of actions against Redis. Why? Because replicating the entire Redis api into a set of statements was not a goal at this point. Only asynchronous projection handling is supported at the moment.

```csharp
var projection =
  new RavenProjectionBuilder().
    When<PortfolioAdded>((session, message) => 
      session.StoreAsync(
	    new PortfolioDocument
	    {
	      Id = message.Id,
	      Name = message.Name
	    }, message.Id.ToString("N"))).
    When<PortfolioRemoved>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id.ToString("N"));
        session.Delete(document);
      }).
    When<PortfolioRenamed>(async (session, message) =>
      {
        var document = await session.LoadAsync<PortfolioDocument>(message.Id.ToString("N"));
        document.Name = message.Name;
      }).
    Build();
```

# Executing projections

How and when you decide to execute the projections is still left as an exercise to you. Typically they will sit behind a message subscription that pushes the appropriate messages into them, causing the execution of actions against RavenDB. You can use the AsyncRavenProjector to perform the actual execution.