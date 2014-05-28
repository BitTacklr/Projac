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

Alternatively you can use a positional syntax, reminiscent of ODBC parameters, where parameter names will be auto assigned and formatted into the text. Don't thank me, thank @tojans for the suggestion.

```csharp
TSql.NonQueryFormat(
  "INSERT INTO [Item] (Id, Name) VALUES ({0}, {1})",
  TSql.Int(message.Id), TSql.VarChar(message.Value, 40));
```

## Declarative Projections

Syntactic sugar to allow you to specify projections without the need for a dedicated class. The code should speak for itself, but does require some playing around with. Mind you, only non query T-SQL statements are supported (pit of success).

```csharp
var specification =
  TSql.Projection().
    When<PortfolioAdded>(@event =>
      TSql.NonQuery(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    ).
    When<PortfolioRemoved>(@event =>
      TSql.NonQuery(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) }
    ).
    When<PortfolioRenamed>(@event =>
      TSql.NonQuery(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    ).
    Build();
```

How and when you decide to execute the projection specification is still left as an exercise to you. Typically, you'll turn the handlers into a map where you can lookup the handler based on the type of event to be handled. Familiarity with plain old ADO.NET is assumed. You can take a look at the ```TSqlNonQueryStatementFlusher``` (in the tests under Usage) to get an idea of how to flush the resulting statements.

## Projection Handler

Your projection handlers should either accept an ```IObserver<TSqlNonQueryStatement>``` to push their SQL statements on or the projection handling methods should return ```IEnumerable<TSqlNonQueryStatement>```. This is not something that is part of the library. This is your code and optionally the framework you depend upon. The declarative projection syntax above only supports the ```Enumerable``` approach.

```csharp
// Observable approach - void IHandle.Handle(TMessage message)

public class PortfolioListProjectionHandler : 
  IHandle<PortfolioAdded>,
  IHandle<PortfolioRemoved>,
  IHandle<PortfolioRenamed> {
  
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

  public void Handle(PortfolioRenamed @event) {
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
  IHandle<PortfolioRenamed> {

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

  public IEnumerable<TSqlNonQueryStatement> Handle(PortfolioRenamed @event) {
    yield return
      TSql.NonQuery(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }
}

```

## FSharp support

F# is a wonderful language that makes some of the Projac bits - like declarative projections - obsolete since you can use the language itself as a DSL to get the same result. Projac has been extended with support for F#'s FSharpOption&gt;T&lt; such that it blends more with the native language (room for improvement no doubt). Below an example of a native declarative projection that is leveraging pattern matching.

```fsharp
open System;
open Projac;

type PortfolioCreated = { PortfolioId : int; Name : string }
type PhotoAddToPortfolio = { PortfolioId : int; PhotoId : int }
type PhotoRemovedFromPortfolio = { PortfolioId : int; PhotoId : int }
type PortfolioArchived = { PortfolioId : int; Name : string }

let PortfolioProjection (message:Object) =
    seq {
        match message with
            | :? PortfolioCreated as m -> 
                yield TSql.NonQueryFormat(
                    "INSERT INTO [PortfolioPhotoCount] ([Id], [Name], [PhotoCount]) VALUES ({0}, {1}, {2})", 
                    TSql.Int(m.PortfolioId), 
                    TSql.VarCharMax(m.Name), 
                    TSql.Int(0))
            | :? PhotoAddToPortfolio as m ->
                yield TSql.NonQueryFormat(
                    "UPDATE [PortfolioPhotoCount] SET [PhotoCount] = [PhotoCount] + 1 WHERE [Id] = {0}", 
                    TSql.Int(m.PortfolioId))
            | :? PhotoRemovedFromPortfolio as m ->
                yield TSql.NonQueryFormat(
                    "UPDATE [PortfolioPhotoCount] SET [PhotoCount] = [PhotoCount] - 1 WHERE [Id] = {0}", 
                    TSql.Int(m.PortfolioId))
            | :? PortfolioArchived as m ->
                yield TSql.NonQueryFormat(
                    "DELETE FROM [PortfolioPhotoCount] WHERE [Id] = {0}", 
                    TSql.Int(m.PortfolioId))
            | _ -> ()
    }
```
