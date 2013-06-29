using System;
using NUnit.Framework;
using Projac.Tests.Builders;

namespace Projac.Tests {
  [TestFixture]
  public class SqlStatementCollectorTests {
    [Test]
    public void IsSqlStatementObserver() {
      Assert.That(NewCollector().Build(), Is.InstanceOf<IObserver<SqlStatement>>());
    }

    [Test]
    public void InitialStatementsAreEmpty() {
      Assert.That(
        NewCollector().Build().Statements,
        Is.Empty);
    }

    [Test]
    public void OnNextCollectsStatement() {
      var sut = NewCollector().Build();
      sut.OnNext(NewStatement().Build());
      Assert.That(
        sut.Statements,
        Is.EquivalentTo(new[] {
          NewStatement().Build(), 
        }));
    }

    [Test]
    public void OnNextAppendsToExistingStatements() {
      var sut = NewCollector().
        WithStatements(new[] {
          NewStatement().WithText("Text1").Build(),
          NewStatement().WithText("Text2").Build()
        }).
        Build();
      sut.OnNext(NewStatement().WithText("Text3").Build());
      Assert.That(
        sut.Statements, Is.EquivalentTo(
          new[] {
            NewStatement().WithText("Text1").Build(),
            NewStatement().WithText("Text2").Build(),
            NewStatement().WithText("Text3").Build()
          }));
    }

    [Test]
    public void OnErrorDoesNotThrow() {
      Assert.DoesNotThrow(() => NewCollector().Build().OnError(new Exception()));
    }

    [Test]
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
