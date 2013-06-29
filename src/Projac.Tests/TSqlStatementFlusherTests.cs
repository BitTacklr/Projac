using System;
using NUnit.Framework;
using Projac.Tests.Builders;

namespace Projac.Tests {
  [TestFixture]
  public class TSqlStatementFlusherTests {
    [Test]
    public void IsSqlStatementFlusher() {
      Assert.That(NewFlusher().Build(), Is.InstanceOf<ISqlStatementFlusher>());
    }

    [Test]
    public void SqlConnectionStringBuilderCanNotBeNull() {
      Assert.Throws<ArgumentNullException>(() => NewFlusher().WithConnectionStringBuilder(null).Build());
    }

    [Test]
    public void FlushSqlStatementsCanNotBeNull() {
      Assert.Throws<ArgumentNullException>(() => NewFlusher().Build().Flush(null));
    }

    [Test]
    public void FlushingNoSqlStatementsDoesNothing() {
      Assert.DoesNotThrow(() => NewFlusher().Build().Flush(new SqlStatement[0]));
    }

    //The rest of the code requires an integration test
    //Need to figure out how I'm going to organize that

#if WORKSONMYMACHINE
    [Test]
    public void IntegrationTest() {
      var builder = new SqlConnectionStringBuilder {
        DataSource = "(localdb)\\Projects",
        InitialCatalog = "Projac",
        IntegratedSecurity = true
      };
      var sut = NewFlusher().WithConnectionStringBuilder(builder).Build();
      sut.Flush(
        new [] {
          Sql.Statement("CREATE TABLE [Test] (Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED, Value NVARCHAR(20) NOT NULL)"),
          Sql.Statement("INSERT INTO [Test] (Value) VALUES (@P1)", new { P1 = "Test" }),
          Sql.Statement("DROP TABLE [Test]")
        });
    }
#endif

    public TSqlStatementFlusherBuilder NewFlusher() {
      return new TSqlStatementFlusherBuilder();
    }
  }
}
