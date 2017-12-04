# Projac

Projac is a set of .NET libraries that allow you to author projections targeting various backing stores and is easy to  integrate with existing event stores such as [EventStore](http://www.eventstore.org) and [SQLStreamStore](https://github.com/SQLStreamStore). [![Join the chat at https://gitter.im/yreynhout/Projac](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/yreynhout/Projac?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

- [Projac](https://github.com/BitTacklr/Projac/wiki/projac.md) allows you to author projections that target any store for which you can bring your own connection (e.g [Redis](http://redis.io), [RavenDB](http://ravendb.net/), [Elasticsearch](http://http://www.elasticsearch.org/), [Microsoft Windows Azure Table Storage](http://azure.microsoft.com/en-us/documentation/services/storage/)). 

- [Projac.Sql, Projac.SqlClient and Projac.SQLite](https://github.com/BitTacklr/Projac/wiki/projac.sql.md) allow you to author projections that target relational databases. Projac.Sql contains common abstractions across all database providers that use the ADO.NET model. Projac.SqlClient targets [Microsoft SQL Server](http://www.microsoft.com/en-us/server-cloud/products/sql-server-editions/overview.aspx). Projac.SQLite targets [SQLite](http://sqlite.org). We welcome contributions for other database providers that follow a similar recipe.

It's available on both NuGet & MyGet:

- Projac: [NuGet](https://www.nuget.org/packages/Projac/) - [MyGet](https://www.myget.org/feed/projac/package/nuget/Projac)
- Projac.Sql: [NuGet](https://www.nuget.org/packages/Projac,Sql/) - [MyGet](https://www.myget.org/feed/projac/package/nuget/Projac.Sql)
- Projac.SqlClient: [NuGet](https://www.nuget.org/packages/Projac.SqlClient/) - [MyGet](https://www.myget.org/feed/projac/package/nuget/Projac.SqlClient)
- [WIP] Projac.SQLite: [NuGet](https://www.nuget.org/packages/Projac,SQLite/) - [MyGet](https://www.myget.org/feed/projac/package/nuget/Projac.SQLite)

The custom MyGet feed can be found [here](https://www.myget.org/F/projac/api/v3/index.json).

---

**Important Changes**

If you're using a version prior to 0.1.0, not only has your cheese been moved, it probably has been broken in unexpected places. Please check out the [changes made in 0.1.0](https://github.com/BitTacklr/Projac/wiki/ChangesIn.0.1.0.md) as well as the [how do I upgrade to 0.1.0 guide](https://github.com/BitTacklr/Projac/wiki/UpgradeTo0.1.0.md). If you want to keep your cheese as is, you can always fork this code base and use the `legacy` branch.