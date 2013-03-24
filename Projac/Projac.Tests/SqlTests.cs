using System;
using System.Collections.Generic;
using Projac.Tests.Builders;
using Xunit;

namespace Projac.Tests {
  public class SqlTests {
    [Fact]
    public void TextOnlyStatementReturnsInstanceWithExpectedTextAndNoParameters() {
      Assert.Equal(
        New().
          WithText("Text").
          WithParameters(new Tuple<string, object>[0]).
          Build(),
        Sql.Statement("Text"));
    }

    [Fact]
    public void ParameterizedStatementReturnsInstanceWithExtractedPropertiesAsParameters() {
      Assert.Equal(
        New().
          WithText("Text").
          WithParameters(AllSupportedDataTypesAsParameters()).
          Build(),
        Sql.Statement("Text", AllSupportedDataTypesAsAnonymousParameter()));
    }

    [Fact]
    public void ParameterizedStatementReturnsInstanceWithEmptyParameters() {
      Assert.Equal(
        New().
          WithText("Text").
          WithParameters(new Tuple<string, object>[0]).
          Build(),
        Sql.Statement("Text", new {}));
    }

    private static IEnumerable<Tuple<string, object>> AllSupportedDataTypesAsParameters() {
      return new[] {
        new Tuple<string, object>("DateTime", DateTime.Today),
        new Tuple<string, object>("DBNull", DBNull.Value),
        new Tuple<string, object>("Decimal", (Decimal)1),
        new Tuple<string, object>("Double", (Double)1),
        new Tuple<string, object>("Guid", new Guid("A81A2EC3-7A92-4E86-9C95-D4B6ED83A56C")),
        new Tuple<string, object>("Int16", (Int16)1),
        new Tuple<string, object>("Int32", (Int32)1),
        new Tuple<string, object>("Int64", (Int64)1),
        new Tuple<string, object>("Null", (object)null),
        new Tuple<string, object>("Single", (Single)1),
        new Tuple<string, object>("String", "Text"),
        new Tuple<string, object>("TimeSpan", TimeSpan.Zero),
      };
    }

    private static object AllSupportedDataTypesAsAnonymousParameter() {
      return new {
        Int16 = (Int16)1,
        Int32 = (Int32)1,
        Int64 = (Int64)1,
        Double = (Double)1,
        Single = (Single)1,
        Decimal = (Decimal)1,
        String = "Text",
        TimeSpan = TimeSpan.Zero,
        DateTime = DateTime.Today,
        Guid = new Guid("A81A2EC3-7A92-4E86-9C95-D4B6ED83A56C"),
        Null = (object)null,
        DBNull = DBNull.Value
      };
    }

    private static SqlStatementBuilder New() {
      return new SqlStatementBuilder();
    }
  }
}
