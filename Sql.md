# Projac & Paramol

Projac provides a set of simple abstractions that allow one to write projections targeting relational databases. It doesn't shove any ```IEventHandler<T>```, ```IHandle<T>```, or ```IMessageHandler<T>``` down your throat. Use your own or the ones provided by the framework you're integrating with, or use the declarative style. Paramol provides abstractions to capture the essence of statements to send to a relational database, along with a fluent syntax to author them. At this point in time only Microsoft SQL Server is supported. You're free to contribute a typed syntax for other relational databases.

It's available on NuGet: [Projac](https://www.nuget.org/packages/Projac/) - [Paramol](https://www.nuget.org/packages/Paramol/)

# Basics

## SqlNonQueryCommand & SqlQueryCommand

Abstracts the text and the parameters to be sent to the database. Both non-query (INSERT, UPDATE, DELETE) and query (SELECT) text statements/procedures are supported, but as a word of advice, you should generally bias towards the non-query ones, since they're the only ones that make sense for writing projections that perform well. They use the ``System.Data.Common`` types such that the command execution code is - in theory - reusable across various ``ADO.NET providers``.

## TSql

Syntactic sugar for writing T-SQL statements in the projection handlers. Parameters can be defined by passing in either an anonymously typed object or a strongly typed one. Properties magically become parameters of the T-SQL statement.

```csharp
TSql.NonQueryStatement(
  "INSERT INTO [Item] (Id, Name) VALUES (@P1, @P2)",
  new { P1 = TSql.Int(message.Id), P2 = TSql.VarChar(message.Value, 40) });
```

* The parameters prefixed with the ```@``` defined in the text refer - by convention - to the properties defined in the parameter type. Properties are automatically prefixed with ```@``` during conversion.
* Use the ```TSql.<DataTypeName>(...)``` methods to specify parameters. This allows for passing in just enough meta-data next to the actual value. The data types have a deliberate focus on the TSQL data types and not the .NET type system.

Alternatively you can use a positional syntax, reminiscent of ODBC parameters, where parameter names will be auto assigned and formatted into the text.

```csharp
TSql.NonQueryStatementFormat(
  "INSERT INTO [Item] (Id, Name) VALUES ({0}, {1})",
  TSql.Int(message.Id), TSql.VarChar(message.Value, 40));
```

Composition plays a big role and can be accessed using the ``TSql.Compose`` method.

```csharp
TSql.Compose(DropSchema()).Compose(CreateSchema());

SqlNonQueryCommand[] DropSchema() 
{
  return TSql.
	Compose(
		TSql.NonQueryStatement("DROP TABLE [Room]")).
	Compose(
		TSql.NonQueryStatement("DROP TABLE [RoomWardCache]"));
}

SqlNonQueryCommand[] CreateSchema() 
{
  return TSql.
	Compose(
		TSql.NonQueryStatement(
@"CREATE TABLE [Room] (
  [RoomId] INT NOT NULL CONSTRAINT PK_Room PRIMARY KEY,
  [Name] NVARCHAR(MAX) NOT NULL,
  [WardId] INT NOT NULL,
  [WardName] NVARCHAR(MAX) NOT NULL
)")).
	Compose(
		TSql.NonQueryStatement(
@"CREATE TABLE [RoomWardCache] (
  [WardId] INT NOT NULL CONSTRAINT PK_RoomWardCache PRIMARY KEY,
  [Name] NVARCHAR(MAX) NOT NULL
)"));
}
```

There are also methods that allow you to conditionally emit commands. Look for methods with ``-If`` and ``-Unless`` suffix.

# Authoring projections

## The Handler Style

With this approach, you're implementing an IHandle (or similar - not part of this library in any case) on a projection class for each message that projection is interested in. There's a number of ways this can work.

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
      TSql.NonQueryStatement(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }));
  }

  public void Handle(PortfolioRemoved @event) {
    statements.OnNext(
      TSql.NonQueryStatement(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) }));
  }

  public void Handle(PortfolioRenamed @event) {
    statements.OnNext(
      TSql.NonQueryStatement(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }));
  }
}
```

Here, your projection handlers should accept an ```IObserver<SqlNonQueryStatement>``` to push their SQL statements on. It's up to you to decide when it's appropriate to flush the observed statements.

```csharp
// Enumerable approach - IEnumerable<SqlNonQueryStatement> IHandle.Handle(TMessage message)

public class PortfolioListProjectionHandler : 
  IHandle<PortfolioAdded>,
  IHandle<PortfolioRemoved>,
  IHandle<PortfolioRenamed> {

  public IEnumerable<SqlNonQueryStatement> Handle(PortfolioAdded @event) {
    yield return
      TSql.NonQueryStatement(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }

  public IEnumerable<SqlNonQueryStatement> Handle(PortfolioRemoved @event) {
    yield return
      TSql.NonQueryStatement(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) });
  }

  public IEnumerable<SqlNonQueryStatement> Handle(PortfolioRenamed @event) {
    yield return
      TSql.NonQueryStatement(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) });
  }
}

```

Here, the projection handling methods return ```IEnumerable<SqlNonQueryStatement>```. Again, how you collect these statements and flush them to the underlying store is up to you.

## The Declarative Style

This approach sports syntactic sugar to allow you to specify projections without the need for an IHandle interface. The code should speak for itself, but does require some playing around with, especially if multiple statements need to be emitted. Mind you, only non query SQL statements are supported.

```csharp
public class PortfolioProjection : SqlProjection
{
  public PortfolioProjection()
  {
    When<PortfolioAdded>(@event =>
      TSql.NonQueryStatement(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    ));
    
    When<PortfolioRemoved>(@event =>
      TSql.NonQueryStatement(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) }
    ));
    
    When<PortfolioRenamed>(@event =>
      TSql.NonQueryStatement(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    ));
  }
}

public static class PortfolioProjectionUsingBuilder
{
  public static readonly AnonymousSqlProjection Instance = new AnonymousSqlProjectionBuilder().
    When<PortfolioAdded>(@event =>
      TSql.NonQueryStatement(
        "INSERT INTO [Portfolio] (Id, Name) VALUES (@P1, @P2)",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    )).
    When<PortfolioRemoved>(@event =>
      TSql.NonQueryStatement(
        "DELETE FROM [Portfolio] WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id) }
    )).
    When<PortfolioRenamed>(@event =>
      TSql.NonQueryStatement(
        "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
        new { P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
    )).
    Build();
}
```

# Executing projections

How and when you decide to execute the projections is still left as an exercise to you. Typically they will sit behind a message subscription that pushes the appropriate messages into them, causing sql commands to be emitted as a side effect. Once these sql commands have been captured you can use one of the built-in executors to execute them (Paramol). If on the other hand, you've authored your projections using the declarative style, then Projac offers a higher level of abstraction called the Async-/SqlProjector.

You'll notice that projections don't know anything about the execution. This is deliberate, allowing you to decide when and how to flush commands to the relational database.

Projac has the ability to perform custom resolution of handlers for a particular message. This capability is provided by the ``SqlProjectionHandlerResolver`` delegate. Out of the box, two implementations are provided. ``Resolve.WhenEqualToHandlerMessageType`` where the message type needs to be an exact match with the message type of the handler, and ``Resolve.WhenAssignableToHandlerMessageType`` where the message type needs to be assignable to the message type of the handler. The latter resolver allows you to dispatch to handlers that are a base type of the message or if you want to use a contravariant envelope (e.g. ``Envelope<out TMessage>``). In this case, handler execution order becomes important. The reasoning is simple: the order in which the handlers are passed into the resolver is the order in which the handlers will be returned from the resolver and consequently also be invoked in that order. Obviously, you're free to bring your own resolver. There's also a concurrent variation of the above two, provided by ``ConcurrentResolve``, **IF** you're calling the projector from different threads concurrently. However, the general recommendation is to call the projector either from a single thread or non-concurrent.

```
var projector = new SqlProjector(
    Resolve.WhenEqualToHandlerMessageType(projection),
    new TransactionalSqlCommandExecutor(
        new ConnectionStringSettings(
            "projac",
            @"Data Source=(localdb)\ProjectsV12;Initial Catalog=ProjacUsage;Integrated Security=SSPI;",
            "System.Data.SqlClient"),
        IsolationLevel.ReadCommitted));
```

# FAQ

## Do I need Projac or Paramol?

It's safe to say that Projac is all about the declarative style while Paramol is all about sql syntax and execution of sql commands. So, if you're not using the declarative style, Paramol should be enough.

# Contributions

* Date, DateTime, DateTime2, Money data types in TSql by [@xt0rted](https://github.com/xt0rted)
* The ``positional syntax`` suggestion by [@tojans](https://github.com/tojans).
* Decimal data type in SqlClientSyntax by [@ritasker](https://github.com/ritasker)
