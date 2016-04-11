namespace Paramol.Tests.SqlClient
{
    using System;
    using NUnit.Framework;
    using Paramol.SqlClient;

    [TestFixture]
    public class TSqlDecimalScaleTests
    {
        [TestCase(19, 0, true)]
        [TestCase(19, 19, true)]
        [TestCase(19, 20, false)]
        public void SizeMustBeWithinRange(byte precision, byte scale, bool withinRange)
        {
            if (!withinRange)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => SutFactory(precision, scale));
            }
            else
            {
                Assert.DoesNotThrow(() => SutFactory(precision, scale));
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
            Assert.That(sut, Is.InstanceOf<IEquatable<TSqlDecimalScale>>());
        }

        [Test]
        public void TwoInstancesAreEqualIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(19, 4);
            var other = SutFactory(19, 4);
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheirValuesDiffer()
        {
            var sut = SutFactory(19, 4);
            var other = SutFactory(19, 5);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstancesHaveTheSameHashCodeIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(19, 4);
            var other = SutFactory(19, 4);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheirValuesDiffer()
        {
            var sut = SutFactory(19, 4);
            var other = SutFactory(19, 5);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstancesAreOperatorEqualIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(19, 4);
            var other = SutFactory(19, 4);
            Assert.That(sut == other, Is.True);
            Assert.That(sut != other, Is.False);
        }

        [Test]
        public void TwoInstancesAreNotOperatorEqualIfTheirValuesDiffer()
        {
            var sut = SutFactory(19,4);
            var other = SutFactory(19, 5);
            Assert.That(sut == other, Is.False);
            Assert.That(sut != other, Is.True);
        }

        [Test]
        public void CanBeImplicitlyConvertedToByte()
        {
            var sut = SutFactory(19, 12);

            Byte result = sut;

            Assert.That(result, Is.EqualTo(12));
        }

        private static TSqlDecimalScale SutFactory()
        {
            var random = new Random();
            var precision = random.Next(1, 38);
            return SutFactory((byte)precision, (byte)random.Next(0, precision));
        }

        private static TSqlDecimalScale SutFactory(byte precision, byte scale)
        {
            return new TSqlDecimalScale(precision, scale);
        }
    }
}