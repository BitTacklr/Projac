# Projac

Projac provides a set of simple abstractions that allow one to write projections targeting Microsoft SQL Server databases. It doesn't shove any ```IEventHandler<T>```, ```IHandle<T>```, or ```IMessageHandler<T>``` down your throat. Use your own or the ones provided by the framework you're integrating with.

It stills needs a lot of ironing, but it's a start. It's on [NuGet](https://www.nuget.org/packages/Projac/) already.

## TSqlNonQueryStatement

Abstracts the text and the parameters to be sent to the database. Only non-query text statements are supported (INSERT, UPDATE, DELETE), since they're the only ones that make sense for writing projections that perform well.

## TSql

Syntactic sugar for writing SQL statements in the projection handlers. Parameters can be defined by passing in either an anonymously typed object or a strongly typed one. Properties magically become parameters of the SQL statement.

```csharp
TSql.Statement(
  "INSERT INTO [Item] (Id, Name) VALUES (@P1, @P2)",
  new { P1 = TSql.Int(message.Id), P2 = TSql.VarChar(message.Value, 40) });
```

* The parameters prefixed with the ```@``` defined in the text refer - by convention - to the properties defined in the parameter type. Properties are automatically prefixed with ```@``` during conversion.
* Use the ```TSql.<DataTypeName>(...)``` methods to specify parameters. This allows for passing in just enough meta-data next to the actual value.

## Projection Handler

Your projection handlers should either accept an ```IObserver<TSqlNonQueryStatement>``` to push their SQL statements on or the projection handling methods should return ```IEnumerable<TSqlNonQueryStatement>```. This is not something that is part of the library. This is your code and optionally the framework you depend upon.

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
      TSql.Statement(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }));
  }

  public void Handle(PortfolioRemoved @event) {
    statements.OnNext(
      TSql.Statement(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) }));
  }

  public void Handle(PortfolioModified @event) {
    statements.OnNext(
      TSql.Statement(
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
      TSql.Statement(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }

  public IEnumerable<TSqlNonQueryStatement> Handle(PortfolioRemoved @event) {
    yield return
      TSql.Statement(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) });
  }

  public IEnumerable<TSqlNonQueryStatement> Handle(PortfolioModified @event) {
    yield return
      TSql.Statement(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }
}

```
