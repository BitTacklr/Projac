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
        public void NonQueryParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQuery("", ParameterCountLimitedExceeded.Instance));
        }

        [Test]
        public void NonQueryIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryIf(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryIf(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryUnless(true, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryUnless(false, "", ParameterCountLimitedExceeded.Instance).ToArray());
        }

        [Test]
        public void NonQueryFormatParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryFormat("", ParameterCountLimitedExceeded.Instance.All));
        }

        [Test]
        public void NonQueryFormatIfParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryFormatIf(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryFormatIfParameterCountNotLimitedTo2098WhenConditionIsNotMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryFormatIf(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryFormatUnlessParameterCountLimitedTo2098WhenConditionIsMet()
        {
            Assert.DoesNotThrow(() => TSql.NonQueryFormatUnless(true, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }

        [Test]
        public void NonQueryFormatUnlessParameterCountNotLimitedTo2098WhenConditionIsMet()
        {
            Assert.Throws<ArgumentException>(() => TSql.NonQueryFormatUnless(false, "", ParameterCountLimitedExceeded.Instance.All).ToArray());
        }
    }
}
