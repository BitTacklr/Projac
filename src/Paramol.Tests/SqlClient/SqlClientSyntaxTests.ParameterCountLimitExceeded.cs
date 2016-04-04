using System;
using System.Linq;
using NUnit.Framework;

namespace Paramol.Tests.SqlClient
{
    public partial class SqlClientSyntaxTests
    {
        [Test]
        public void QueryParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => Sql.QueryStatement("", ParameterCountLimitedExceeded.Instance));
        }

        [Test]
        public void QueryIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.QueryStatementIf(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => Sql.QueryStatementIf(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => Sql.QueryStatementUnless(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.QueryStatementUnless(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryFormatParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => Sql.QueryStatementFormat("", ParameterCountLimitedExceeded.Instance.All));
        }

        [Test]
        public void QueryFormatIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.QueryStatementFormatIf(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void QueryFormatIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => Sql.QueryStatementFormatIf(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void QueryFormatUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => Sql.QueryStatementFormatUnless(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void QueryFormatUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.QueryStatementFormatUnless(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryStatement("", ParameterCountLimitedExceeded.Instance));
        }

        [Test]
        public void NonQueryStatementIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryStatementIf(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryStatementIf(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryStatementUnless(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryStatementUnless(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryStatementFormat("", ParameterCountLimitedExceeded.Instance.All));
        }

        [Test]
        public void NonQueryStatementFormatIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryStatementFormatIf(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryStatementFormatIf(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryStatementFormatUnless(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryStatementFormatUnless(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryProcedure("", ParameterCountLimitedExceeded.Instance));
        }

        [Test]
        public void NonQueryProcedureIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryProcedureIf(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryProcedureIf(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryProcedureUnless(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryProcedureUnless(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryProcedureFormat("", ParameterCountLimitedExceeded.Instance.All));
        }

        [Test]
        public void NonQueryProcedureFormatIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryProcedureFormatIf(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryProcedureFormatIf(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => Sql.NonQueryProcedureFormatUnless(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => Sql.NonQueryProcedureFormatUnless(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }
    }
}
