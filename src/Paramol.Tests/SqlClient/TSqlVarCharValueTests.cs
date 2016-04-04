using System;
using System.Data;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
    public class TSqlVarCharValueTests
    {
        [Test]
        public void NullIsNotAnAcceptableValue()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory(null, new TSqlVarCharSize(123)));
        }

        [Test]
        public void IsSqlParameterValue()
        {
            var sut = SutFactory();

            Assert.That(sut, Is.InstanceOf<IDbParameterValue>());
        }

        [Test]
        public void ToDbParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";
            const string value = "value";

            var sut = SutFactory(value, new TSqlVarCharSize(123));

            var result = sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.VarChar, value, false, 123);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";
            const string value = "value";

            var sut = SutFactory(value, new TSqlVarCharSize(123));

            var result = sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.VarChar, value, false, 123);
        }

        [Test]
        public void DoesEqualItself()
        {
            var sut = SutFactory();
            var instance = sut;
            Assert.That(sut.Equals(instance), Is.True);
        }

        [Test]
        public void DoesNotEqualOtherObjectType()
        {
            var sut = SutFactory();
            Assert.That(sut.Equals(new object()), Is.False);
        }

        [Test]
        public void DoesNotEqualNull()
        {
            var sut = SutFactory();
            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void TwoInstanceAreEqualIfTheyHaveTheSameValueAndSize()
        {
            var sut = SutFactory("value", new TSqlVarCharSize(123));
            var other = SutFactory("value", new TSqlVarCharSize(123));
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueDiffers()
        {
            var sut = SutFactory("value1", new TSqlVarCharSize(123));
            var other = SutFactory("value2", new TSqlVarCharSize(123));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirSizeDiffers()
        {
            var sut = SutFactory("value", new TSqlVarCharSize(123));
            var other = SutFactory("value", new TSqlVarCharSize(456));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyHaveTheSameValueAndSize()
        {
            var sut = SutFactory("value", new TSqlVarCharSize(123));
            var other = SutFactory("value", new TSqlVarCharSize(123));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueDiffers()
        {
            var sut = SutFactory("value1", new TSqlVarCharSize(123));
            var other = SutFactory("value2", new TSqlVarCharSize(123));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirSizeDiffers()
        {
            var sut = SutFactory("value", new TSqlVarCharSize(123));
            var other = SutFactory("value", new TSqlVarCharSize(456));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        private static TSqlVarCharValue SutFactory()
        {
            return SutFactory("value", new TSqlVarCharSize(123));
        }

        private static TSqlVarCharValue SutFactory(string value, TSqlVarCharSize size)
        {
            return new TSqlVarCharValue(value, size);
        }
    }
}