using System;
using System.Data.SqlClient;
using Projac.Tests.Builders;
using Xunit;

namespace Projac.Tests {
  public class TSqlStatementFlusherTests {
    [Fact]
    public void IsSqlStatementFlusher() {
      Assert.IsAssignableFrom<ISqlStatementFlusher>(NewFlusher().Build());
    }

    [Fact]
    public void SqlConnectionStringBuilderCanNotBeNull() {
      Assert.Throws<ArgumentNullException>(() => NewFlusher().WithConnectionStringBuilder(null).Build());
    }

    [Fact]
    public void FlushSqlStatementsCanNotBeNull() {
      Assert.Throws<ArgumentNullException>(() => NewFlusher().Build().Flush(null));
    }

    [Fact]
    public void FlushingNoSqlStatementsDoesNothing() {
      Assert.DoesNotThrow(() => NewFlusher().Build().Flush(new SqlStatement[0]));
    }

    //The rest of the code requires an integration test
    //Need to figure out how I'm going to organize that

#if WORKSONMYMACHINE
    [Fact]
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
