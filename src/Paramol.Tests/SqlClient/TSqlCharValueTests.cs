using System;
using System.Data;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
    public class TSqlCharValueTests
    {
        [Test]
        public void NullIsNotAnAcceptableValue()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory(null, new TSqlCharSize(123)));
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

            var sut = SutFactory(value, new TSqlCharSize(123));

            var result = sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Char, value, false, 123);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";
            const string value = "value";

            var sut = SutFactory(value, new TSqlCharSize(123));

            var result = sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Char, value, false, 123);
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
            var sut = SutFactory("value", new TSqlCharSize(123));
            var other = SutFactory("value", new TSqlCharSize(123));
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueDiffers()
        {
            var sut = SutFactory("value1", new TSqlCharSize(123));
            var other = SutFactory("value2", new TSqlCharSize(123));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirSizeDiffers()
        {
            var sut = SutFactory("value", new TSqlCharSize(123));
            var other = SutFactory("value", new TSqlCharSize(456));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyHaveTheSameValueAndSize()
        {
            var sut = SutFactory("value", new TSqlCharSize(123));
            var other = SutFactory("value", new TSqlCharSize(123));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueDiffers()
        {
            var sut = SutFactory("value1", new TSqlCharSize(123));
            var other = SutFactory("value2", new TSqlCharSize(123));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirSizeDiffers()
        {
            var sut = SutFactory("value", new TSqlCharSize(123));
            var other = SutFactory("value", new TSqlCharSize(456));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        private static TSqlCharValue SutFactory()
        {
            return SutFactory("value", new TSqlCharSize(123));
        }

        private static TSqlCharValue SutFactory(string value, TSqlCharSize size)
        {
            return new TSqlCharValue(value, size);
        }
    }
}