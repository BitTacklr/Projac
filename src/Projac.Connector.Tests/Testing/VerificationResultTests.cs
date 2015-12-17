using System;
using NUnit.Framework;
using Projac.Connector.Testing;

namespace Projac.Connector.Tests.Testing
{
    [TestFixture]
    public class VerificationResultTests
    {
        [Test]
        public void PassMessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => VerificationResult.Pass(null));
        }

        [TestCase("")]
        [TestCase("message")]
        public void PassReturnsExpectedResult(string message)
        {
            var result = VerificationResult.Pass(message);

            Assert.That(result.Passed, Is.True);
            Assert.That(result.Failed, Is.False);
            Assert.That(result.Message, Is.EqualTo(message));
        }

        [Test]
        public void FailMessageCanNotBeNull()
        {
           Assert.Throws<ArgumentNullException>(() => VerificationResult.Fail(null));
        }

        [TestCase("")]
        [TestCase("message")]
        public void FailReturnsExpectedResult(string message)
        {
            var result = VerificationResult.Fail(message);

            Assert.That(result.Passed, Is.False);
            Assert.That(result.Failed, Is.True);
            Assert.That(result.Message, Is.EqualTo(message));
        }

        [Test]
        public void IsEquatable()
        {
            var sut = SutFactory();

            Assert.That(sut, Is.InstanceOf<IEquatable<VerificationResult>>());
        }

        [Test]
        public void ObjectEqualsItself()
        {
            var sut = SutFactory();

            Assert.That(sut.Equals((object) sut), Is.True);
        }

        [Test]
        public void EqualsItself()
        {
            var sut = SutFactory();

            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void DoesNotEqualObjectOfOtherType()
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
        public void TwoInstancesAreEqualWhenTheirStateAndMessageAreEqual()
        {
            var sut = SutFactory(VerificationResultState.Passed, "message");
            var other = SutFactory(VerificationResultState.Passed, "message");

            Assert.That(sut.Equals(other), Is.True);
        }

        [Test]
        public void TwoInstancesAreNotEqualWhenTheirStateDiffers()
        {
            var sut = SutFactory(VerificationResultState.Failed, "message");
            var other = SutFactory(VerificationResultState.Passed, "message");

            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstancesAreNotEqualWhenTheirMessageDiffers()
        {
            var sut = SutFactory(VerificationResultState.Passed, "message1");
            var other = SutFactory(VerificationResultState.Passed, "message2");

            Assert.That(sut.Equals(other), Is.False);
        }

        [Test]
        public void TwoInstancesAreOperatorEqualWhenTheirStateAndMessageAreEqual()
        {
            var sut = SutFactory(VerificationResultState.Passed, "message");
            var other = SutFactory(VerificationResultState.Passed, "message");

            Assert.That(sut == other, Is.True);
            Assert.That(sut != other, Is.False);
        }

        [Test]
        public void TwoInstancesAreNotOperatorEqualWhenTheirStateDiffers()
        {
            var sut = SutFactory(VerificationResultState.Failed, "message");
            var other = SutFactory(VerificationResultState.Passed, "message");

            Assert.That(sut == other, Is.False);
            Assert.That(sut != other, Is.True);
        }

        [Test]
        public void TwoInstancesAreNotOperatorEqualWhenTheirMessageDiffers()
        {
            var sut = SutFactory(VerificationResultState.Passed, "message1");
            var other = SutFactory(VerificationResultState.Passed, "message2");

            Assert.That(sut == other, Is.False);
            Assert.That(sut != other, Is.True);
        }

        [Test]
        public void TwoInstancesHaveTheSameHashCodeWhenTheirStateAndMessageAreEqual()
        {
            var sut = SutFactory(VerificationResultState.Passed, "message");
            var other = SutFactory(VerificationResultState.Passed, "message");

            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.True);
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirStateDiffers()
        {
            var sut = SutFactory(VerificationResultState.Failed, "message");
            var other = SutFactory(VerificationResultState.Passed, "message");

            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        [Test]
        public void TwoInstancesDoNotHaveTheSameHashCodeWhenTheirMessageDiffers()
        {
            var sut = SutFactory(VerificationResultState.Passed, "message1");
            var other = SutFactory(VerificationResultState.Passed, "message2");

            Assert.That(sut.GetHashCode().Equals(other.GetHashCode()), Is.False);
        }

        private static VerificationResult SutFactory()
        {
            return VerificationResult.Pass();
        }

        private static VerificationResult SutFactory(VerificationResultState state, string message)
        {
            return state == VerificationResultState.Passed ? VerificationResult.Pass(message) : VerificationResult.Fail(message);
        }
    }
}
