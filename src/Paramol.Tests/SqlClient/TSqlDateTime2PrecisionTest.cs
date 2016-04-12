using System;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
    public class TSqlDateTime2PrecisionTest
    {
        [Test]
        public void MinReturnsExpectedInstance()
        {
            Assert.That(TSqlDateTime2Precision.Max, Is.EqualTo(new TSqlDateTime2Precision(7)));
        }

        [Test]
        public void MaxReturnsExpectedInstance()
        {
            Assert.That(TSqlDateTime2Precision.Min, Is.EqualTo(new TSqlDateTime2Precision(0)));
        }

        [Test]
        public void DefaultReturnsExpectedInstance()
        {
            Assert.That(TSqlDateTime2Precision.Default, Is.EqualTo(new TSqlDateTime2Precision(7)));
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        [TestCase(6, true)]
        [TestCase(7, true)]
        [TestCase(8, false)]
        public void PrecisionMustBeWithinRange(byte value, bool withinRange)
        {
            if (!withinRange)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => SutFactory(value));
            }
            else
            {
                Assert.DoesNotThrow(() => SutFactory(value));
            }
        }

        [Test]
        public void DoesEqualItself()
        {
            var sut = SutFactory();
            var instance = sut;
            Assert.That(sut.Equals(instance), Is.True);
        }

        [Test]
        public void DoesNotEqualObjectOfOtherType()
        {
            var sut = SutFactory();
            var instance = new object();
            Assert.That(sut.Equals(instance), Is.False);
        }

        [Test]
        public void DoesNotEqualNull()
        {
            var sut = SutFactory();
            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void IsEquatable()
        {
            var sut = SutFactory();
            Assert.That(sut, Is.InstanceOf<IEquatable<TSqlDateTime2Precision>>());
        }

        [Test]
        public void TwoInstancesAreEqualIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(1);
            var other = SutFactory(1);
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheirValuesDiffer()
        {
            var sut = SutFactory(1);
            var other = SutFactory(2);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstancesHaveTheSameHashCodeIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(1);
            var other = SutFactory(1);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheirValuesDiffer()
        {
            var sut = SutFactory(1);
            var other = SutFactory(2);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstancesAreOperatorEqualIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(1);
            var other = SutFactory(1);
            Assert.That(sut == other, Is.True);
            Assert.That(sut != other, Is.False);
        }

        [Test]
        public void TwoInstancesAreNotOperatorEqualIfTheirValuesDiffer()
        {
            var sut = SutFactory(1);
            var other = SutFactory(2);
            Assert.That(sut == other, Is.False);
            Assert.That(sut != other, Is.True);
        }

        [Test]
        public void CanBeImplicitlyConvertedToByte()
        {
            var sut = SutFactory(3);

            Byte result = sut;

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void CanBeImplicitlyConvertedFromByte()
        {
            TSqlDateTime2Precision sut = 3;

            Assert.That(sut, Is.EqualTo(SutFactory(3)));
        }

        private static TSqlDateTime2Precision SutFactory()
        {
            return SutFactory((byte)new Random().Next(0, 7));
        }

        private static TSqlDateTime2Precision SutFactory(byte value)
        {
            return new TSqlDateTime2Precision(value);
        }
    }
}