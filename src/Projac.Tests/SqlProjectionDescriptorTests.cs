using System;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionDescriptorTests
    {
        private const string Identifier = "identifier:v1";

        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((string)null)
                );
        }

        [Test]
        public void SchemaProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutSchemaProjectionFactory(null)
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
            var schemaProjection = new SqlProjection(new[] { new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]) });
            var sut = SutFactory("identifier-v2", schemaProjection, projection);

            Assert.That(sut.Identifier, Is.EqualTo("identifier-v2"));
            Assert.That(sut.SchemaProjection, Is.SameAs(schemaProjection));
            Assert.That(sut.Projection, Is.SameAs(projection));
        }

        private static SqlProjectionDescriptor SutFactory(string identifier)
        {
            return SutFactory(identifier, SqlProjection.Empty, SqlProjection.Empty);
        }

        private static SqlProjectionDescriptor SutSchemaProjectionFactory(SqlProjection projection)
        {
            return SutFactory(Identifier, projection, SqlProjection.Empty);
        }

        private static SqlProjectionDescriptor SutProjectionFactory(SqlProjection projection)
        {
            return SutFactory(Identifier, SqlProjection.Empty, projection);
        }

        private static SqlProjectionDescriptor SutFactory(
            string identifier,
            SqlProjection schemaProjection,
            SqlProjection projection)
        {
            return new SqlProjectionDescriptor(identifier, schemaProjection, projection);
        }
    }
}