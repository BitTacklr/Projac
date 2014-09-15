using System;
using NUnit.Framework;
using Projac.Messages;

namespace Projac.Tests.Messages
{
    [TestFixture]
    public class RebuildProjectionTests
    {
        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RebuildProjection(null, "", ""));
        }

        [Test]
        public void DropVersionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RebuildProjection("", null, ""));
        }

        [Test]
        public void CreateVersionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RebuildProjection("", "", null));
        }

        [Test]
        public void PropertiesReturnExpectedResult()
        {
            var sut = new RebuildProjection("id", "v1", "v2");
            Assert.That(sut.Identifier, Is.EqualTo("id"));
            Assert.That(sut.DropVersion, Is.EqualTo("v1"));
            Assert.That(sut.CreateVersion, Is.EqualTo("v2"));
        }
    }
}
