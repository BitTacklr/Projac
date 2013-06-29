using System;
using NUnit.Framework;
using Projac.Tests.Builders;

namespace Projac.Tests {
  [TestFixture]
  public class SqlStatementTests {
    [Test]
    public void TextCannotBeNull() {
      Assert.Throws<ArgumentNullException>(() => New().WithText(null).Build());
    }

    [Test]
    public void ParametersCanNotBeNull() {
      Assert.Throws<ArgumentNullException>(() => New().WithParameters(null).Build());
    }

    [Test]
    public void TextReturnsExpectedInitialValue() {
      Assert.That(New().WithText("Text").Build().Text, Is.EqualTo("Text"));
    }

    [Test]
    public void ParametersReturnsExpectedInitialValue() {
      Assert.That(
        New().WithParameters(new[] {
          new Tuple<string, object>("P1", DBNull.Value),
          new Tuple<string, object>("P2", "Test"),
        }).Build().Parameters,
        Is.EqualTo(new[] {
          new Tuple<string, object>("P1", DBNull.Value),
          new Tuple<string, object>("P2", "Test"),
        }));
    }

    [Test]
    public void EmptyParametersReturnsExpectedInitialValue() {
      Assert.That(
        New().WithParameters(new Tuple<string, object>[0]).Build().Parameters,
        Is.EqualTo(new Tuple<string, object>[0]));
    }

    [Test]
    public void TwoInstancesAreEqualWhenTheyHaveTheSameTextAndProperties() {
      Assert.That(
        New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build(),
        Is.EqualTo(New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build()));
    }

    [Test]
    public void TwoInstancesAreNotEqualWhenTheirTextDiffers() {
      Assert.That(
        New().WithText("Text1").Build(),
        Is.Not.EqualTo(New().WithText("Text2").Build()));
    }

    [Test]
    public void TwoInstancesAreNotEqualWhenTheirParameterNamesDiffer() {
      Assert.That(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build(),
        Is.Not.EqualTo(New().
          WithParameters(new[] {
            new Tuple<string, object>("P2", DBNull.Value)
          }).
          Build()));
    }

    [Test]
    public void TwoInstancesAreNotEqualWhenTheirParameterValuesDiffer() {
      Assert.That(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build(),
        Is.Not.EqualTo(New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build()));
    }

    [Test]
    public void TwoInstancesAreNotEqualWhenTheirParameterCountsDiffer() {
      Assert.That(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0),
            new Tuple<string, object>("P2", 0)
          }).
          Build(),
        Is.Not.EqualTo(New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build()));
    }


    [Test]
    public void TwoInstancesHaveTheSameHashCodeWhenTheyHaveTheSameTextAndProperties() {
      Assert.That(
        New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build().GetHashCode(),
        Is.EqualTo(New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build().GetHashCode()));
    }

    [Test]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirTextDiffers() {
      Assert.That(
        New().WithText("Text1").Build().GetHashCode(),
        Is.Not.EqualTo(New().WithText("Text2").Build().GetHashCode()));
    }

    [Test]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirParameterNamesDiffer() {
      Assert.That(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build().GetHashCode(),
        Is.Not.EqualTo(New().
          WithParameters(new[] {
            new Tuple<string, object>("P2", DBNull.Value)
          }).
          Build().GetHashCode()));
    }

    [Test]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirParameterValuesDiffer() {
      Assert.That(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build().GetHashCode(),
        Is.Not.EqualTo(New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build().GetHashCode()));
    }

    [Test]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirParameterCountsDiffer() {
      Assert.That(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0),
            new Tuple<string, object>("P2", 0)
          }).
          Build().GetHashCode(),
        Is.Not.EqualTo(New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build().GetHashCode()));
    }

    private static SqlStatementBuilder New() {
      return new SqlStatementBuilder();
    }
  }
}