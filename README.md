# Projac

Projac provides a set of simple abstractions that allow one to write projections targeting Microsoft SQL Server(TM). It doesn't shove any ```IEventHandler<T>```, ```IHandle<T>```, or ```IMessageHandler<T>``` down your throat. Use your own or the ones provided by the framework you're integrating with.

It stills needs a lot of ironing, but it's a start.

## SqlStatement

Abstracts the text and the parameters to be sent to the database. Only non-query text statements are supported (INSERT, UPDATE, DELETE), since they're the only ones that make sense for writing projections that perform well.

## Sql

Syntactic sugar for writing SQL statements in the projection handlers. Parameters can be defined by passing in an anonymous type. Properties magically become parameters of the SQL statement. The parameters prefixed with the ```@``` defined in the text refer - by convention - to the properties defined in the anonymous type.

```csharp
Sql.Statement(
  "INSERT INTO [Table] (Id, Name) VALUES (@P1, @P2)",
  new { P1 = @event.Id, P2 = @event.Name });
```

## Projection Handler

Your projection handlers should accept an ```IObserver<SqlStatement>``` to push their SQL statements on. Except for the observer, this is not something that is part of the library. This is your code and optionally the framework you depend upon.

```csharp
public class PortfolioListProjectionHandler : 
  IHandle<PortfolioAdded>,
  IHandle<PortfolioRemoved>,
  IHandle<PortfolioModified> {
  
  readonly IObserver<SqlStatement> statements;

  public PortfolioListProjectionHandler(IObserver<SqlStatement> statements) {
    this.statements = statements;
  }

  public void Handle(PortfolioAdded @event) {
    statements.OnNext(
      Sql.Statement(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = @event.Id, P2 = @event.Name }));
  }

  public void Handle(PortfolioRemoved @event) {
    statements.OnNext(
      Sql.Statement(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = @event.Id }));
  }

  public void Handle(PortfolioModified @event) {
    statements.OnNext(
      Sql.Statement(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = @event.Id, P2 = @event.Name }));
  }
}

```

## SqlStatementCollector

Collects ```SqlStatements``` from one or more projection handlers. It's an implementation of ```IObserver<SqlStatement>```. Which statements are collected highly depends on how you configure the wiring of the collector to the projection handlers. It's a game of lifetime scopes best played using your Inversion of Control Container of choice or the one that comes with the messaging framework you are integrating with.

## SqlStatementFlusher

The default implementation of ```ISqlStatementFlusher``` is ```TSqlStatementFlusher```. It flushes each of the sql statements passed in, to the underlying MS SQL Server database, wrapping the whole in one big transaction. The idea is to pass the collected ```SqlStatements``` from the collector onto the flusher. It doesn't take genius to realize that batching and thresholded flushing are within arms reach.