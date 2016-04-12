using System.Collections;
using System.Collections.Generic;

namespace Paramol.Tests.SqlClient
{
    using System;
    using NUnit.Framework;
    using Paramol.SqlClient;

    [TestFixture]
    public class TSqlDecimalScaleTests
    {
        [Test]
        public void MaxReturnsExpectedInstance()
        {
            Assert.That(TSqlDecimalScale.Max, Is.EqualTo(new TSqlDecimalScale(38)));
        }

        [Test]
        public void MinReturnsExpectedInstance()
        {
            Assert.That(TSqlDecimalScale.Min, Is.EqualTo(new TSqlDecimalScale(0)));
        }

        [Test]
        public void DefaultReturnsExpectedInstance()
        {
            Assert.That(TSqlDecimalScale.Default, Is.EqualTo(new TSqlDecimalScale(0)));
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(37, true)]
        [TestCase(38, true)]
        [TestCase(39, false)]
        public void ScaleMustBeWithinRange(byte value, bool withinRange)
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
            Assert.That(sut, Is.InstanceOf<IEquatable<TSqlDecimalScale>>());
        }

        [Test]
        public void TwoInstancesAreEqualIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(4);
            var other = SutFactory(4);
            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstancesAreNotEqualIfTheirValuesDiffer()
        {
            var sut = SutFactory(4);
            var other = SutFactory(5);
            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstancesHaveTheSameHashCodeIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(4);
            var other = SutFactory(4);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeIfTheirValuesDiffer()
        {
            var sut = SutFactory(4);
            var other = SutFactory(5);
            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstancesAreOperatorEqualIfTheyHaveTheSameValue()
        {
            var sut = SutFactory(4);
            var other = SutFactory(4);
            Assert.That(sut == other, Is.True);
            Assert.That(sut != other, Is.False);
        }

        [Test]
        public void TwoInstancesAreNotOperatorEqualIfTheirValuesDiffer()
        {
            var sut = SutFactory(4);
            var other = SutFactory(5);
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
            TSqlDecimalScale result = 12;

            Assert.That(result, Is.EqualTo(new TSqlDecimalScale(12)));
        }

        [Test]
        public void CanBeExplicitlyConvertedFromByte()
        {
            var result = (TSqlDecimalScale)12;

            Assert.That(result, Is.EqualTo(new TSqlDecimalScale(12)));
        }

        [Test]
        public void IsComparableToTSqlDecimalScale()
        {
            Assert.That(SutFactory(), Is.InstanceOf<IComparable<TSqlDecimalScale>>());
        }

        [Test]
        public void IsComparableToTSqlDecimalPrecision()
        {
            Assert.That(SutFactory(), Is.InstanceOf<IComparable<TSqlDecimalPrecision>>());
        }

        [TestCaseSource("CompareToScaleCases")]
        public void CompareToScaleReturnsExpectedResult(TSqlDecimalScale sut, TSqlDecimalScale other, int expected)
        {
            var result = sut.CompareTo(other);

            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> CompareToScaleCases()
        {
            yield return new TestCaseData(
                new TSqlDecimalScale(1), new TSqlDecimalScale(2), -1);
            yield return new TestCaseData(
                new TSqlDecimalScale(2), new TSqlDecimalScale(1), 1);
            yield return new TestCaseData(
                new TSqlDecimalScale(1), new TSqlDecimalScale(1), 0);
        }

        [TestCaseSource("CompareToPrecisionCases")]
        public void CompareToPrecisionReturnsExpectedResult(TSqlDecimalScale sut, TSqlDecimalPrecision other, int expected)
        {
            var result = sut.CompareTo(other);

            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> CompareToPrecisionCases()
        {
            yield return new TestCaseData(
                new TSqlDecimalScale(1), new TSqlDecimalPrecision(2), -1);
            yield return new TestCaseData(
                new TSqlDecimalScale(2), new TSqlDecimalPrecision(1), 1);
            yield return new TestCaseData(
                new TSqlDecimalScale(1), new TSqlDecimalPrecision(1), 0);
        }

        [TestCase(1, 1, true)]
        [TestCase(2, 1, false)]
        [TestCase(0, 1, true)]
        public void LessThanOrEqualToScaleOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) <= new TSqlDecimalScale(right);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1, 1, false)]
        [TestCase(2, 1, false)]
        [TestCase(0, 1, true)]
        public void LessThanScaleOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) < new TSqlDecimalScale(right);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1, 1, true)]
        [TestCase(2, 1, true)]
        [TestCase(0, 1, false)]
        public void GreaterThanOrEqualToScaleOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) >= new TSqlDecimalScale(right);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1, 1, false)]
        [TestCase(2, 1, true)]
        [TestCase(0, 1, false)]
        public void GreaterThanScaleOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) > new TSqlDecimalScale(right);

            Assert.That(result, Is.EqualTo(expected));
        }



        [TestCase(1, 1, true)]
        [TestCase(2, 1, false)]
        [TestCase(0, 1, true)]
        public void LessThanOrEqualToPrecisionOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) <= new TSqlDecimalPrecision(right);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1, 1, false)]
        [TestCase(2, 1, false)]
        [TestCase(0, 1, true)]
        public void LessThanPrecisionOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) < new TSqlDecimalPrecision(right);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1, 1, true)]
        [TestCase(2, 1, true)]
        [TestCase(0, 1, false)]
        public void GreaterThanOrEqualToPrecisionOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) >= new TSqlDecimalPrecision(right);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1, 1, false)]
        [TestCase(2, 1, true)]
        [TestCase(0, 1, false)]
        public void GreaterThanPrecisionOperatorReturnsExpectedResult(byte left, byte right, bool expected)
        {
            var result = new TSqlDecimalScale(left) > new TSqlDecimalPrecision(right);

            Assert.That(result, Is.EqualTo(expected));
        }

        private static TSqlDecimalScale SutFactory()
        {
            var random = new Random();
            return SutFactory((byte)random.Next(0, 38));
        }

        private static TSqlDecimalScale SutFactory(byte value)
        {
            return new TSqlDecimalScale(value);
        }
    }
}