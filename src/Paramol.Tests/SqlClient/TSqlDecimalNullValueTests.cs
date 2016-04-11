namespace Paramol.Tests.SqlClient
{
    using System;
    using System.Data;
    using NUnit.Framework;
    using Paramol.SqlClient;

    [TestFixture]
    public class TSqlDecimalNullValueTests
    {
        private TSqlDecimalNullValue _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new TSqlDecimalNullValue();
        }

        [Test]
        public void InstanceIsSqlNullValue()
        {
            Assert.That(new TSqlDecimalNullValue(), Is.InstanceOf<TSqlDecimalNullValue>());
        }

        [Test]
        public void IsSqlParameterValue()
        {
            Assert.That(_sut, Is.InstanceOf<IDbParameterValue>());
        }

        [Test]
        public void ToDbParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var result = _sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Decimal, DBNull.Value, true, 0, 18, 0);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var result = _sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Decimal, DBNull.Value, true, 0, 18, 0);
        }

        [Test]
        public void DoesNotEqualOtherObjectType()
        {
            Assert.That(_sut.Equals(new object()), Is.False);
        }

        [Test]
        public void DoesNotEqualNull()
        {
            Assert.That(_sut.Equals(null), Is.False);
        }

        [Test]
        public void HasExpectedHashCode()
        {
            var result = _sut.GetHashCode();

            Assert.That(result, Is.EqualTo(0));
        }
    }
}