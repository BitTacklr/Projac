using System;
using System.Data;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
    public class TSqlDateTime2ValueTests
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
            var sut = SutFactory(value, new TSqlDateTime2Precision(3));

            var result = sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.DateTime2, value, false, 8, 3);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";

            var value = DateTime.UtcNow;
            var sut = SutFactory(value, new TSqlDateTime2Precision(3));

            var result = sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.DateTime2, value, false, 8, 3);
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
        public void TwoInstanceAreEqualIfTheyHaveTheSameValueAndPrecision()
        {
            var value = DateTime.UtcNow;
            var sut = SutFactory(value, new TSqlDateTime2Precision(3));
            var other = SutFactory(value, new TSqlDateTime2Precision(3));
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueDiffers()
        {
            var value1 = new DateTime(0);
            var value2 = new DateTime(1);
            var sut = SutFactory(value1, new TSqlDateTime2Precision(3));
            var other = SutFactory(value2, new TSqlDateTime2Precision(3));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirPrecisionDiffers()
        {
            var value = new DateTime(0);
            var sut = SutFactory(value, new TSqlDateTime2Precision(3));
            var other = SutFactory(value, new TSqlDateTime2Precision(4));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyHaveTheSameValueAndSize()
        {
            var value = new DateTime(0);
            var sut = SutFactory(value, new TSqlDateTime2Precision(3));
            var other = SutFactory(value, new TSqlDateTime2Precision(3));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueDiffers()
        {
            var value1 = new DateTime(0);
            var value2 = new DateTime(1);
            var sut = SutFactory(value1, new TSqlDateTime2Precision(3));
            var other = SutFactory(value2, new TSqlDateTime2Precision(3));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirSizeDiffers()
        {
            var value = new DateTime(0);
            var sut = SutFactory(value, new TSqlDateTime2Precision(3));
            var other = SutFactory(value, new TSqlDateTime2Precision(4));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        private static TSqlDateTime2Value SutFactory()
        {
            return SutFactory(DateTime.UtcNow, new TSqlDateTime2Precision(3));
        }

        private static TSqlDateTime2Value SutFactory(DateTime value, TSqlDateTime2Precision precision)
        {
            return new TSqlDateTime2Value(value, precision);
        }
    }
}