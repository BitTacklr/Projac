using System;
using NUnit.Framework;
using Projac.Messages;

namespace Projac.Tests.Messages
{
    [TestFixture]
    public class BuildProjectionTests
    {
        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new BuildProjection(null, ""));
        }

        [Test]
        public void CreateVersionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new BuildProjection("", null));
        }

        [Test]
        public void PropertiesReturnExpectedResult()
        {
            var sut = new BuildProjection("id", "v2");
            Assert.That(sut.Identifier, Is.EqualTo("id"));
            Assert.That(sut.CreateVersion, Is.EqualTo("v2"));
        }
    }
}