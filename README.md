# Projac & Paramol

Projac provides a set of simple abstractions that allow one to write projections targeting relational databases (only support for Microsoft SQL Server at this point). It doesn't shove any ```IEventHandler<T>```, ```IHandle<T>```, or ```IMessageHandler<T>``` down your throat. Use your own or the ones provided by the framework you're integrating with. Paramol provides abstractions to capture the essence of statements to send to a relational database, along with a fluent syntax to author them (only support for Microsoft SQL Server at this point).

It's on NuGet already: [CSharp version](https://www.nuget.org/packages/Projac/) and [FSharp version](https://www.nuget.org/packages/Projac.FSharp/)

## SqlNonQueryStatement & SqlQueryStatement

Abstracts the text and the parameters to be sent to the database. Both non-query (INSERT, UPDATE, DELETE) and query (SELECT) text statements are supported, but as a word of advice, you should generally bias towards non-query statements, since they're the only ones that make sense for writing projections that perform well.

## TSql

Syntactic sugar for writing T-SQL statements in the projection handlers. Parameters can be defined by passing in either an anonymously typed object or a strongly typed one. Properties magically become parameters of the T-SQL statement.

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

Syntactic sugar to allow you to specify projections without the need for a dedicated class. The code should speak for itself, but does require some playing around with. Mind you, only non query SQL statements are supported (pit of success and all that).

```csharp
var projection =
  new SqlProjectionBuilder().
    When<PortfolioAdded>(@event =>
      TSql.NonQuery(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    )).
    When<PortfolioRemoved>(@event =>
      TSql.NonQuery(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) }
    )).
    When<PortfolioRenamed>(@event =>
      TSql.NonQuery(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    )).
    Build();
```

How and when you decide to execute the projection specification is still left as an exercise to you. Typically, you'll turn the handlers into a map where you can lookup the handler based on the type of event to be handled. Familiarity with plain old ADO.NET is assumed. You can take a look at the ```TSqlNonQueryStatementFlusher``` (in the tests under Usage) to get an idea of how to flush the resulting statements.

## Projection Descriptor

Next to the actual projection, you'll want to somehow identify the projection. A name and/or version or a date and time. It's just a piece of string. Data definition statements describe the schema of the current projection as a bunch of ``SqlNonQueryStatements``, e.g. drop any database objects pertaining to previous versions of the projection and create any new database objects pertaining to the *current* version. Note that data definition statements are entirely optional, under your control and omit any form of handholding (unlike traditional database schema migration tooling). What's the motivation behind this? Traditionally one would manage the database object schema *on-the-side* using a database project (or an equivalent there of). Yet this is counter to the idea of treating projections as individual units. As such you should see this as an attempt to bring what is usually separated closer together. 

```csharp
public static class PortfolioProjection
{
  public static readonly SqlProjectionDescriptor Descriptor =
    new SqlProjectionDescriptorBuilder("portfolio-v1")
    {
      DataDefinitionStatements = TSql.Compose(
              TSql.NonQuery(
                  "CREATE TABLE [Portfolio] ( " +
                  "[Id] INT NOT NULL PRIMARY KEY, " +
                  "[Name] NVARCHAR(40) NOT NULL)")),
      Projection = new SqlProjectionBuilder().
          When<PortfolioAdded>(@event =>
              TSql.NonQuery(
                  "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
                  new {P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40)}
                  )).
          When<PortfolioRemoved>(@event =>
              TSql.NonQuery(
                  "DELETE FROM [Portfolio] WHERE Id = @P1",
                  new {P1 = TSql.Int(@event.Id)}
                  )).
          When<PortfolioRenamed>(@event =>
              TSql.NonQuery(
                  "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
                  new {P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40)}
                  )).
          Build()
    }.
    Build();
}
```
## Projection Handler

Your projection handlers should either accept an ```IObserver<SqlNonQueryStatement>``` to push their SQL statements on or the projection handling methods should return ```IEnumerable<SqlNonQueryStatement>```. This is not something that is part of the library. This is your code and optionally the framework you depend upon. The declarative projection syntax above only supports the ```Enumerable``` approach.

```csharp
// Observable approach - void IHandle.Handle(TMessage message)

public class PortfolioListProjectionHandler : 
  IHandle<PortfolioAdded>,
  IHandle<PortfolioRemoved>,
  IHandle<PortfolioRenamed> {
  
  readonly IObserver<TSqlNonQueryStatement> statements;

  public PortfolioListProjectionHandler(IObserver<SqlNonQueryStatement> statements) {
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

// Enumerable approach - IEnumerable<SqlNonQueryStatement> IHandle.Handle(TMessage message)

public class PortfolioListProjectionHandler : 
  IHandle<PortfolioAdded>,
  IHandle<PortfolioRemoved>,
  IHandle<PortfolioRenamed> {

  public IEnumerable<SqlNonQueryStatement> Handle(PortfolioAdded @event) {
    yield return
      TSql.NonQuery(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }

  public IEnumerable<SqlNonQueryStatement> Handle(PortfolioRemoved @event) {
    yield return
      TSql.NonQuery(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) });
  }

  public IEnumerable<SqlNonQueryStatement> Handle(PortfolioRenamed @event) {
    yield return
      TSql.NonQuery(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }
}

```

## FSharp support

F# is a wonderful language that makes some of the Projac bits - like declarative projections - obsolete since you can use the language itself as a DSL to get the same result. Projac has been extended with support for F#'s FSharpOption&lt;T&gt;, such that it blends more with the native language (there are numerous other variations possible). Below an example of a native declarative projection that is leveraging pattern matching.

```fsharp
open System;
open Projac;

type PortfolioEvent =
    | PortfolioCreated of PortfolioCreated
    | PortfolioArchived of PortfolioArchived
    | PhotoAdded of PhotoAddToPortfolio
    | PhotoRemoved of PhotoRemovedFromPortfolio
and PortfolioCreated = { PortfolioId : int; Name : string }
and PhotoAddToPortfolio = { PortfolioId : int; PhotoId : int }
and PhotoRemovedFromPortfolio = { PortfolioId : int; PhotoId : int }
and PortfolioArchived = { PortfolioId : int; Name : string }

[<AutoOpen>] // This could live in a ProjacFSharpBindings.fs in the NuGet
module ProjacArgumentHelpers =
    let TSqlArg converter val name = name, val |> converter
    let TSqlInt = TSqlArg TSql.Int
    let TSqlVarCharMax = TSqlArg TSql.VarCharMax

let projectPortfolioEvent = function
    | PortfolioAdded m -> 
        TSql.NonQuery(
            "INSERT INTO [PortfolioPhotoCount] ([Id], [Name], [PhotoCount]) VALUES (@Id, @Name, @PhotoCount)", 
            [   m.PortfolioId |> TSqlInt "Id"
                m.Name        |> TSqlVarChar "Name""
                0             |> TSqlInt "PhotoCount"); ])
    | PhotoAdded m ->
        TSql.NonQuery(
            "UPDATE [PortfolioPhotoCount] SET [PhotoCount] = [PhotoCount] + 1 WHERE [Id] = @Id", 
            [ m.PortfolioId |> TSqlInt "Id" ])
    | PhotoRemoved m ->
        TSql.NonQuery(
            "UPDATE [PortfolioPhotoCount] SET [PhotoCount] = [PhotoCount] - 1 WHERE [Id] = @Id", 
            [ m.PortfolioId |> TSqlInt "Id" ])
    | PortfolioArchived as m ->
        TSql.NonQueryFormat(
            "DELETE FROM [PortfolioPhotoCount] WHERE [Id] = {0}", 
            m.PortfolioId |> TSqlInt "Id")```