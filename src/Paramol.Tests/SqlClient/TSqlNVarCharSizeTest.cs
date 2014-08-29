using System;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
    public class TSqlNVarCharSizeTest
    {
        [Test]
        public void MaxReturnsExpectedResult()
        {
            Assert.That(TSqlNVarCharSize.Max, Is.EqualTo(new TSqlNVarCharSize(-1)));
        }

        [TestCase(Int32.MinValue, false)]
        [TestCase(-2, false)]
        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(4000, true)]
        [TestCase(4001, false)]
        [TestCase(Int32.MaxValue, false)]
        public void SizeMustBeWithinRange(int value, bool withinRange)
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
            Assert.That(sut, Is.InstanceOf<IEquatable<TSqlNVarCharSize>>());
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
        public void CanBeImplicitlyConvertedToInt32()
        {
            var sut = SutFactory(123);

            Int32 result = sut;

            Assert.That(result, Is.EqualTo(123));
        }

        [Test]
        public void CanBeImplicitlyConvertedFromInt32()
        {
            TSqlNVarCharSize sut = 123;

            Assert.That(sut, Is.EqualTo(SutFactory(123)));
        }

        private static TSqlNVarCharSize SutFactory()
        {
            return SutFactory(new Random().Next(-1, 4000));
        }

        private static TSqlNVarCharSize SutFactory(int value)
        {
            return new TSqlNVarCharSize(value);
        }
    }
}