using System;
using Projac.Tests.Builders;
using Xunit;

namespace Projac.Tests {
  public class SqlStatementCollectorTests {
    [Fact]
    public void IsSqlStatementObserver() {
      Assert.
        IsAssignableFrom<IObserver<SqlStatement>>(NewCollector().Build());
    }

    [Fact]
    public void InitialStatementsAreEmpty() {
      Assert.Equal(
        new SqlStatement[0],
        NewCollector().Build().Statements);
    }

    [Fact]
    public void OnNextCollectsStatement() {
      var sut = NewCollector().Build();
      sut.OnNext(NewStatement().Build());
      Assert.Equal(
        new [] {
          NewStatement().Build(), 
        },
        sut.Statements);
    }

    [Fact]
    public void OnNextAppendsToExistingStatements() {
      var sut = NewCollector().
        WithStatements(new[] {
          NewStatement().WithText("Text1").Build(),
          NewStatement().WithText("Text2").Build()
        }).
        Build();
      sut.OnNext(NewStatement().WithText("Text3").Build());
      Assert.Equal(
        new[] {
          NewStatement().WithText("Text1").Build(), 
          NewStatement().WithText("Text2").Build(),
          NewStatement().WithText("Text3").Build() 
        },
        sut.Statements);
    }

    [Fact]
    public void OnErrorDoesNotThrow() {
      Assert.DoesNotThrow(() => NewCollector().Build().OnError(new Exception()));
    }

    [Fact]
    public void OnCompletedDoesNotThrow() {
      Assert.DoesNotThrow(() => NewCollector().Build().OnCompleted());
    }

    private static SqlStatementCollectorBuilder NewCollector() {
      return new SqlStatementCollectorBuilder();
    }

    private static SqlStatementBuilder NewStatement() {
      return new SqlStatementBuilder();
    }
  }
}
