namespace Paramol.Tests.SqlClient
{
    using System;
    using System.Data;

    using NUnit.Framework;

    using Paramol.SqlClient;

    [TestFixture]
    public class TSqlDateValueTests
    {
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

            var value = DateTime.UtcNow;
            var sut = SutFactory(value);

            var result = sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Date, value);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var value = DateTime.UtcNow;
            var sut = SutFactory(value);

            var result = sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Date, value);
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
        public void TwoInstanceAreEqualIfTheyHaveTheSameValue()
        {
            var value = DateTime.UtcNow;
            var sut = SutFactory(value);
            var other = SutFactory(value);
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueDiffers()
        {
            var value1 = new DateTime(0);
            var value2 = new DateTime(1);
            var sut = SutFactory(value1);
            var other = SutFactory(value2);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyHaveTheSameValue()
        {
            var value = new DateTime(0);
            var sut = SutFactory(value);
            var other = SutFactory(value);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueDiffers()
        {
            var value1 = new DateTime(0);
            var value2 = new DateTime(1);
            var sut = SutFactory(value1);
            var other = SutFactory(value2);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        private static TSqlDateValue SutFactory()
        {
            return SutFactory(DateTime.UtcNow);
        }

        private static TSqlDateValue SutFactory(DateTime value)
        {
            return new TSqlDateValue(value);
        }
    }
}