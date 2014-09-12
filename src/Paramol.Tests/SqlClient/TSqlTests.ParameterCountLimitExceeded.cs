using System;
using System.Linq;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    public partial class TSqlTests
    {
        [Test]
        public void QueryParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.Query("", ParameterCountLimitedExceeded.Instance));
        }

        [Test]
        public void QueryIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.QueryIf(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.QueryIf(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.QueryUnless(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.QueryUnless(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void QueryFormatParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.QueryFormat("", ParameterCountLimitedExceeded.Instance.All));
        }

        [Test]
        public void QueryFormatIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.QueryFormatIf(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void QueryFormatIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.QueryFormatIf(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void QueryFormatUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.QueryFormatUnless(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void QueryFormatUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.QueryFormatUnless(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryStatement("", ParameterCountLimitedExceeded.Instance));
        }

        [Test]
        public void NonQueryStatementIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryStatementIf(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryStatementIf(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryStatementUnless(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryStatementUnless(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryStatementFormat("", ParameterCountLimitedExceeded.Instance.All));
        }

        [Test]
        public void NonQueryStatementFormatIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryStatementFormatIf(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryStatementFormatIf(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryStatementFormatUnless(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryStatementFormatUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryStatementFormatUnless(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryProcedure("", ParameterCountLimitedExceeded.Instance));
        }

        [Test]
        public void NonQueryProcedureIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryProcedureIf(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryProcedureIf(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryProcedureUnless(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryProcedureUnless(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryProcedureFormat("", ParameterCountLimitedExceeded.Instance.All));
        }

        [Test]
        public void NonQueryProcedureFormatIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryProcedureFormatIf(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryProcedureFormatIf(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryProcedureFormatUnless(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryProcedureFormatUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryProcedureFormatUnless(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }
    }
}
