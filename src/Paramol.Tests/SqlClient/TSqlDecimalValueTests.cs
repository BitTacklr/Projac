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

            var sut = SutFactory(123.0m, 19, 2);

            var result = sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Decimal, 123.0m, false, 0, 19, 2);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var sut = SutFactory(123.0m, 19, 2);

            var result = sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.Decimal, 123, false, 0, 19, 2);
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
        public void TwoInstanceAreEqualIfTheyHaveTheSameValueAndPrecisionAndScale()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(123.0m, 5, 4);
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirPrecisionDiffers()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(123.0m, 6, 4);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirScaleDiffers()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(123.0m, 5, 3);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueDiffers()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(456.0m, 5, 4);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyHaveTheSameValueAndPrecisionAndScale()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(123.0m, 5, 4);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirPrecisionDiffers()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(123.0m, 6, 4);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirScaleDiffers()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(123.0m, 5, 3);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueDiffers()
        {
            var sut = SutFactory(123.0m, 5, 4);
            var other = SutFactory(456.0m, 5, 4);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(38, true)]
        [TestCase(39, false)]
        public void PrecisionMustBeWithinRange(byte precision, bool withinRange)
        {
            if (!withinRange)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => SutFactory(123.0m, precision));
            }
            else
            {
                Assert.DoesNotThrow(() => SutFactory(123.0m, precision));
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