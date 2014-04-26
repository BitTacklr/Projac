# Projac

Projac provides a set of simple abstractions that allow one to write projections targeting Microsoft SQL Server databases. It doesn't shove any ```IEventHandler<T>```, ```IHandle<T>```, or ```IMessageHandler<T>``` down your throat. Use your own or the ones provided by the framework you're integrating with.

It's on [NuGet](https://www.nuget.org/packages/Projac/) already.

## TSqlNonQueryStatement & TSqlQueryStatement

Abstracts the text and the parameters to be sent to the database. Both non-query (INSERT, UPDATE, DELETE) and query (SELECT) text statements are supported, but as a word of advice, you should generally bias towards non-query statements, since they're the only ones that make sense for writing projections that perform well.

## TSql

Syntactic sugar for writing SQL statements in the projection handlers. Parameters can be defined by passing in either an anonymously typed object or a strongly typed one. Properties magically become parameters of the SQL statement.

```csharp
TSql.NonQuery(
  "INSERT INTO [Item] (Id, Name) VALUES (@P1, @P2)",
  new { P1 = TSql.Int(message.Id), P2 = TSql.VarChar(message.Value, 40) });
```

* The parameters prefixed with the ```@``` defined in the text refer - by convention - to the properties defined in the parameter type. Properties are automatically prefixed with ```@``` during conversion.
* Use the ```TSql.<DataTypeName>(...)``` methods to specify parameters. This allows for passing in just enough meta-data next to the actual value. The data types have a deliberate focus on the TSQL data types and not the .NET type system.

## Declarative Projections

Syntactic sugar to allow you to specify projections without the need for a dedicated class. The code should speak for itself, but does require some playing around with. Mind you, only non query T-SQL statements are supported (pit of success).

```csharp
var specification =
    TSql.Projection().
        When<StartedShopping>(@event =>
            TSql.NonQuery(
                "INSERT INTO [Cart] ([CartId], [Started], [Ended]) VALUES (@CartId, @Started, NULL)",
                new
                {
                    CartId = TSql.Int(@event.CartId),
                    Started = TSql.DateTimeOffset(@event.When)
                })
        ).
        When<CheckedoutCart>(@event =>
            TSql.Compose(
                @event.Items.Select(item => TSql.NonQuery(
                    "INSERT INTO [CartContent] ([CartId], [ItemId], [Count]) VALUES (@CartId, @ItemId, @Count)",
                    new
                    {
                        CartId = TSql.Int(@event.CartId),
                        ItemId = TSql.Int(item.Id),
                        Count = TSql.Int(item.Count)
                    }))).
                Compose(
                    TSql.NonQuery(
                        "UPDATE [Cart] SET [Ended] = @Ended WHERE [CartId] = @CartId",
                        new
                        {
                            CartId = TSql.Int(@event.CartId),
                            Ended = TSql.DateTimeOffset(@event.When)
                        }))
        ).
        Build();
```

How and when you decide to execute the projection specification is still left as an exercise to you. Typically, you'll turn the handlers into a map where you can lookup the handler in based on the type of event to be handled. Familiarity with plain old ADO.NET is assumed. You can take a look at the ```TSqlNonQueryStatementFlusher``` (in the tests under Usage) to get an idea of how to flush the resulting statements.

## Projection Handler

Your projection handlers should either accept an ```IObserver<TSqlNonQueryStatement>``` to push their SQL statements on or the projection handling methods should return ```IEnumerable<TSqlNonQueryStatement>```. This is not something that is part of the library. This is your code and optionally the framework you depend upon. The declarative projection syntax above only supports the ```Enumerable``` approach.

```csharp
// Observable approach - void IHandle.Handle(TMessage message)

public class PortfolioListProjectionHandler : 
  IHandle<PortfolioAdded>,
  IHandle<PortfolioRemoved>,
  IHandle<PortfolioModified> {
  
  readonly IObserver<TSqlNonQueryStatement> statements;

  public PortfolioListProjectionHandler(IObserver<TSqlNonQueryStatement> statements) {
    this.statements = statements;
  }

  public void Handle(PortfolioAdded @event) {
    statements.OnNext(
      TSql.NonQuery(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }));
  }

  public void Handle(PortfolioRemoved @event) {
    statements.OnNext(
      TSql.NonQuery(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) }));
  }

  public void Handle(PortfolioModified @event) {
    statements.OnNext(
      TSql.NonQuery(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }));
  }
}

// Enumerable approach - IEnumerable<TSqlNonQueryStatement> IHandle.Handle(TMessage message)

public class PortfolioListProjectionHandler : 
  IHandle<PortfolioAdded>,
  IHandle<PortfolioRemoved>,
  IHandle<PortfolioModified> {

  public IEnumerable<TSqlNonQueryStatement> Handle(PortfolioAdded @event) {
    yield return
      TSql.NonQuery(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }

  public IEnumerable<TSqlNonQueryStatement> Handle(PortfolioRemoved @event) {
    yield return
      TSql.NonQuery(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) });
  }

  public IEnumerable<TSqlNonQueryStatement> Handle(PortfolioModified @event) {
    yield return
      TSql.NonQuery(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }
}

```
