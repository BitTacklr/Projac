using System;
using System.Collections.Generic;
using NUnit.Framework;
using Projac.Tests.Builders;

namespace Projac.Tests {
  [TestFixture]
  public class SqlTests {
    [Test]
    public void TextOnlyStatementReturnsInstanceWithExpectedTextAndNoParameters() {
      Assert.That(
        Sql.Statement("Text"),
        Is.EqualTo(New().
          WithText("Text").
          WithParameters(new Tuple<string, object>[0]).
          Build()));
    }

    [Test]
    public void ParameterizedStatementReturnsInstanceWithExtractedPropertiesAsParameters() {
      Assert.That(
        Sql.Statement("Text", AllSupportedDataTypesAsAnonymousParameter()),
        Is.EqualTo(New().
          WithText("Text").
          WithParameters(AllSupportedDataTypesAsParameters()).
          Build()));
    }

    [Test]
    public void ParameterizedStatementReturnsInstanceWithEmptyParameters() {
      Assert.That(
        Sql.Statement("Text", new {}),
        Is.EqualTo(New().
          WithText("Text").
          WithParameters(new Tuple<string, object>[0]).
          Build()));
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
