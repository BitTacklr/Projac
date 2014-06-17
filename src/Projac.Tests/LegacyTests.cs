#pragma warning disable 618
using System;
using System.Data;
using NUnit.Framework;
using Projac.Tests.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlNullValueTests
    {
        private TSqlNullValue _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = TSqlNullValue.Instance;
        }

        [Test]
        public void InstanceIsSqlNullValue()
        {
            Assert.That(TSqlNullValue.Instance, Is.InstanceOf<TSqlNullValue>());
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

            result.Expect(parameterName, SqlDbType.Variant, DBNull.Value, true);
        }

        [Test]
        public void DoesEqualItself()
        {
            Assert.That(_sut.Equals(TSqlNullValue.Instance), Is.True);
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

    [TestFixture]
    public partial class TSqlTests
    {
        [Test]
        public void NullReturnsSqlNullValueInstance()
        {
            Assert.That(TSql.Null(), Is.SameAs(TSqlNullValue.Instance));
        }
    }
}
#pragma warning restore 618