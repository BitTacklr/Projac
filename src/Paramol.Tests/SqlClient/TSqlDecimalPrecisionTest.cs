namespace Paramol.Tests.SqlClient
{
    using System;
    using NUnit.Framework;
    using Paramol.SqlClient;

    [TestFixture]
    public class TSqlDecimalPrecisionTests
    {
        [Test]
        public void MaxReturnsExpectedInstance()
        {
            Assert.That(TSqlDecimalPrecision.Max, Is.EqualTo(new TSqlDecimalPrecision(38)));
        }

        [Test]
        public void MinReturnsExpectedInstance()
        {
            Assert.That(TSqlDecimalPrecision.Min, Is.EqualTo(new TSqlDecimalPrecision(1)));
        }

        [Test]
        public void DefaultReturnsExpectedInstance()
        {
            Assert.That(TSqlDecimalPrecision.Default, Is.EqualTo(new TSqlDecimalPrecision(18)));
        }

        [TestCase(byte.MinValue, false)]
        [TestCase(1, true)]
        [TestCase(38, true)]
        [TestCase(byte.MaxValue, false)]
        public void SizeMustBeWithinRange(byte value, bool withinRange)
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
            Assert.That(sut, Is.InstanceOf<IEquatable<TSqlDecimalPrecision>>());
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
            var sut = SutFactory(12);

            byte result = sut;

            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void CanBeExplicitlyConvertedToByte()
        {
            var sut = SutFactory(12);

            var result = (byte)sut;

            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void CanBeImplicitlyConvertedFromByte()
        {
            TSqlDecimalPrecision sut = 12;

            Assert.That(sut, Is.EqualTo(SutFactory(12)));
        }

        [Test]
        public void CanBeExplicitlyConvertedFromByte()
        {
            var sut = (TSqlDecimalPrecision)12;

            Assert.That(sut, Is.EqualTo(SutFactory(12)));
        }

        private static TSqlDecimalPrecision SutFactory()
        {
            return SutFactory((byte)new Random().Next(1, 38));
        }

        private static TSqlDecimalPrecision SutFactory(byte value)
        {
            return new TSqlDecimalPrecision(value);
        }
    }
}