using System;
using NUnit.Framework;
using Projac.Messages;

namespace Projac.Tests.Messages
{
    [TestFixture]
    public class ReplayProjectionTests
    {
        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ReplayProjection(null, ""));
        }

        [Test]
        public void ReplayVersionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ReplayProjection("", null));
        }

        [Test]
        public void PropertiesReturnExpectedResult()
        {
            var sut = new ReplayProjection("id", "v2");
            Assert.That(sut.Identifier, Is.EqualTo("id"));
            Assert.That(sut.ReplayVersion, Is.EqualTo("v2"));
        }
    }
}