using System;
using System.Data;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
    public class TSqlIntNullValueTests
    {
        private TSqlIntNullValue _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = TSqlIntNullValue.Instance;
        }

        [Test]
        public void InstanceIsSqlNullValue()
        {
            Assert.That(TSqlIntNullValue.Instance, Is.InstanceOf<TSqlIntNullValue>());
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

            result.ExpectSqlParameter(parameterName, SqlDbType.Int, DBNull.Value, true, 4);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var result = _sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Int, DBNull.Value, true, 4);
        }

        [Test]
        public void DoesEqualItself()
        {
            Assert.That(_sut.Equals(TSqlIntNullValue.Instance), Is.True);
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