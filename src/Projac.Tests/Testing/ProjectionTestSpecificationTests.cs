using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Testing;

namespace Projac.Tests.Testing
{
    [TestFixture]
    public class ProjectionTestSpecificationTests
    {
        [Test]
        public void ResolverCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ProjectionTestSpecification<object>(
                    null,
                    new object[0],
                    (session, token) => Task.FromResult(VerificationResult.Pass()))
                );
        }

        [Test]
        public void ResolverReturnsExpectedResult()
        {
            var resolver = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]);
            var sut = new ProjectionTestSpecification<object>(
                resolver,
                new object[0],
                (session, token) => Task.FromResult(VerificationResult.Pass()));

            var result = sut.Resolver;
               
            Assert.That(result, Is.SameAs(resolver));
        }

        [Test]
        public void MessagesCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ProjectionTestSpecification<object>(
                    Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]),
                    null,
                    (session, token) => Task.FromResult(VerificationResult.Pass()))
                );
        }

        [Test]
        public void MessagesReturnsExpectedResult()
        {
            var messages = new [] {new object(), new object() };
            var sut = new ProjectionTestSpecification<object>(
                Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]),
                messages,
                (session, token) => Task.FromResult(VerificationResult.Pass()));

            var result = sut.Messages;

            Assert.That(result, Is.SameAs(messages));
        }

        [Test]
        public void VerificationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ProjectionTestSpecification<object>(
                    Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]),
                    new object[0],
                    null)
                );
        }

        [Test]
        public void VerificationReturnsExpectedResult()
        {
            Func<object, CancellationToken, Task<VerificationResult>> verification = 
                (session, token) => Task.FromResult(VerificationResult.Pass());
            var sut = new ProjectionTestSpecification<object>(
                Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]),
                new object[0],
                verification);

            var result = sut.Verification;

            Assert.That(result, Is.SameAs(verification));
        }
    }
}
