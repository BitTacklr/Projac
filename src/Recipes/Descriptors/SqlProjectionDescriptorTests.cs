using System;
using NUnit.Framework;
using Paramol;
using Projac;

namespace Recipes.Descriptors
{
    [TestFixture]
    public class SqlProjectionDescriptorTests
    {
        private const string Identifier = "id";

        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutIdentifierFactory(null)
                );
        }

        [Test]
        public void ProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutProjectionFactory(null)
                );
        }

        [Test]
        public void PropertiesArePreserved()
        {
            var projection = new SqlProjection(new[] { new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]) });
            var sut = SutFactory("identifier", projection);

            Assert.That(sut.Identifier, Is.EqualTo("identifier"));
            Assert.That(sut.Projection, Is.SameAs(projection));
        }

        [Test]
        public void ToBuilderReturnsExpectedResult()
        {
            var handler = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var projection = new SqlProjection(new[] { handler });
            var sut = SutFactory("identifier", projection);

            var result = sut.ToBuilder().Build();

            Assert.That(result.Identifier, Is.EqualTo("identifier"));
            Assert.That(result.Projection.Handlers, Is.EquivalentTo(new[] { handler }));
        }

        private static SqlProjectionDescriptor SutIdentifierFactory(string identifier)
        {
            return SutFactory(identifier, SqlProjection.Empty);
        }

        private static SqlProjectionDescriptor SutProjectionFactory(SqlProjection projection)
        {
            return SutFactory(Identifier, projection);
        }

        private static SqlProjectionDescriptor SutFactory(string identifier, SqlProjection projection)
        {
            return new SqlProjectionDescriptor(identifier, projection);
        }
    }
}