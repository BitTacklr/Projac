using System;
using Projac.Tests.Builders;
using Xunit;

namespace Projac.Tests {
  public class SqlStatementTests {
    [Fact]
    public void TextCannotBeNull() {
      Assert.Throws<ArgumentNullException>(() => New().WithText(null).Build());
    }

    [Fact]
    public void ParametersCanNotBeNull() {
      Assert.Throws<ArgumentNullException>(() => New().WithParameters(null).Build());
    }

    [Fact]
    public void TextReturnsExpectedInitialValue() {
      Assert.Equal("Text", New().WithText("Text").Build().Text);
    }

    [Fact]
    public void ParametersReturnsExpectedInitialValue() {
      Assert.Equal(new[] {
        new Tuple<string, object>("P1", DBNull.Value),
        new Tuple<string, object>("P2", "Test"),
      }, New().WithParameters(new[] {
        new Tuple<string, object>("P1", DBNull.Value),
        new Tuple<string, object>("P2", "Test"),
      }).Build().Parameters);
    }

    [Fact]
    public void EmptyParametersReturnsExpectedInitialValue() {
      Assert.Equal(
        new Tuple<string, object>[0],
        New().WithParameters(new Tuple<string, object>[0]).Build().Parameters);
    }

    [Fact]
    public void TwoInstancesAreEqualWhenTheyHaveTheSameTextAndProperties() {
      Assert.Equal(
        New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build(),
        New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build());
    }

    [Fact]
    public void TwoInstancesAreNotEqualWhenTheirTextDiffers() {
      Assert.NotEqual(
        New().WithText("Text1").Build(),
        New().WithText("Text2").Build());
    }

    [Fact]
    public void TwoInstancesAreNotEqualWhenTheirParameterNamesDiffer() {
      Assert.NotEqual(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build(),
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P2", DBNull.Value)
          }).
          Build());
    }

    [Fact]
    public void TwoInstancesAreNotEqualWhenTheirParameterValuesDiffer() {
      Assert.NotEqual(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build(),
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build());
    }

    [Fact]
    public void TwoInstancesAreNotEqualWhenTheirParameterCountsDiffer() {
      Assert.NotEqual(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0),
            new Tuple<string, object>("P2", 0)
          }).
          Build(),
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build());
    }


    [Fact]
    public void TwoInstancesHaveTheSameHashCodeWhenTheyHaveTheSameTextAndProperties() {
      Assert.Equal(
        New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build().GetHashCode(),
        New().
          WithText("Text").
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value),
            new Tuple<string, object>("P2", "Test"),
          }).
          Build().GetHashCode());
    }

    [Fact]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirTextDiffers() {
      Assert.NotEqual(
        New().WithText("Text1").Build().GetHashCode(),
        New().WithText("Text2").Build().GetHashCode());
    }

    [Fact]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirParameterNamesDiffer() {
      Assert.NotEqual(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build().GetHashCode(),
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P2", DBNull.Value)
          }).
          Build().GetHashCode());
    }

    [Fact]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirParameterValuesDiffer() {
      Assert.NotEqual(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build().GetHashCode(),
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", DBNull.Value)
          }).
          Build().GetHashCode());
    }

    [Fact]
    public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirParameterCountsDiffer() {
      Assert.NotEqual(
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0),
            new Tuple<string, object>("P2", 0)
          }).
          Build().GetHashCode(),
        New().
          WithParameters(new[] {
            new Tuple<string, object>("P1", 0)
          }).
          Build().GetHashCode());
    }

    private static SqlStatementBuilder New() {
      return new SqlStatementBuilder();
    }
  }
}