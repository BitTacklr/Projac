namespace Paramol.Tests.SqlClient
{
    using System;
    using System.Data;
    using NUnit.Framework;
    using Paramol.SqlClient;

    [TestFixture]
    public class TSqlDecimalValueTests
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

            var sut = SutFactory(123.0m);

            var result = sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Decimal, 123.0m, false, 0, 18, 1);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var sut = SutFactory(123.0m);

            var result = sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Decimal, 123, false, 0, 18, 1);
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
            var sut = SutFactory(123.0m);
            var other = SutFactory(123.0m);
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueDiffers()
        {
            var sut = SutFactory(123.0m);
            var other = SutFactory(456.0m);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(123.0m);
            var other = SutFactory(123.0m);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueDiffers()
        {
            var sut = SutFactory(123.0m);
            var other = SutFactory(456.0m);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(38, true)]
        [TestCase(39, false)]
        public void PrecisionMustBeWithinRange(byte scale, bool withinRange)
        {
            if (!withinRange)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => SutFactory(123.0m, scale));
            }
            else
            {
                Assert.DoesNotThrow(() => SutFactory(123.0m, scale));
            }
        }

        [TestCase(0, true)]
        [TestCase(8, true)]
        [TestCase(9, false)]
        public void ScaleMustBeWithinRange(byte scale, bool withinRange)
        {
            if (!withinRange)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => SutFactory(123.0m, 8, scale));
            }
            else
            {
                Assert.DoesNotThrow(() => SutFactory(123.0m, 8, scale));
            }
        }

        private static TSqlDecimalValue SutFactory()
        {
            return SutFactory(123.0m);
        }

        private static TSqlDecimalValue SutFactory(decimal value, byte precision = 18, byte scale = 0)
        {
            return new TSqlDecimalValue(value, precision, scale);
        }
    }
}