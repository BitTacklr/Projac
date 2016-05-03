# Projac

[![Join the chat at https://gitter.im/yreynhout/Projac](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/yreynhout/Projac?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Projac is a set of projection libraries that allow you to write projections targetting various backing stores.

- [Projac & Paramol](./Sql.md): Allows you to write projections that target relational databases (only [Microsoft SQL Server](http://www.microsoft.com/en-us/server-cloud/products/sql-server-editions/overview.aspx) and [SQLite](http://sqlite.org) ATM).
- [Projac.Connector](./Connector.md): Allows you to write projections that target any store for which you can bring your own connection (e.g [Redis](http://redis.io), [RavenDB](http://ravendb.net/), [Elasticsearch](http://http://www.elasticsearch.org/), [Microsoft Windows Azure Table Storage](http://azure.microsoft.com/en-us/documentation/services/storage/) )
