using System;
using System.Data;
using NUnit.Framework;
using Projac.Tests.Framework;

namespace Projac.Tests
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
            Assert.That(_sut, Is.InstanceOf<ITSqlParameterValue>());
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var result = _sut.ToSqlParameter(parameterName);

            result.Expect(parameterName, SqlDbType.Int, DBNull.Value, true, 4);
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