using System;
using System.Data;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
    public class TSqlVarBinaryValueTests
    {
        [Test]
        public void NullIsNotAnAcceptableValue()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory(null, new TSqlVarBinarySize(123)));
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
            var value = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sut = SutFactory(value, new TSqlVarBinarySize(50));

            var result = sut.ToDbParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.VarBinary, value, false, 50);
        }

        [Test]
        public void ToSqlParameterReturnsExpectedInstance()
        {
            const string parameterName = "name";
            var value = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sut = SutFactory(value, new TSqlVarBinarySize(50));

            var result = sut.ToSqlParameter(parameterName);

            result.ExpectSqlParameter(parameterName, SqlDbType.VarBinary, value, false, 50);
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
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueContentDiffers()
        {
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 4, 5, 6 }, new TSqlVarBinarySize(123));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirValueLengthDiffers()
        {
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 1, 2 }, new TSqlVarBinarySize(123));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceAreNotEqualIfTheirSizeDiffers()
        {
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(456));
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstanceHaveTheSameHashCodeIfTheyHaveTheSameValueAndSize()
        {
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueContentDiffers()
        {
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 4, 5, 6 }, new TSqlVarBinarySize(123));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirValueLengthDiffers()
        {
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 1, 2 }, new TSqlVarBinarySize(123));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstanceDoNotHaveTheSameHashCodeIfTheirSizeDiffers()
        {
            var sut = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123));
            var other = SutFactory(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(456));
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        private static TSqlVarBinaryValue SutFactory()
        {
            return SutFactory(new byte[] { 1, 2, 3, 4, 5 }, new TSqlVarBinarySize(123));
        }

        private static TSqlVarBinaryValue SutFactory(byte[] value, TSqlVarBinarySize size)
        {
            return new TSqlVarBinaryValue(value, size);
        }
    }
}