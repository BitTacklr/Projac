using System;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionDescriptorBuilderTests
    {
        private const string Identifier = "id";
        private const string Version = "v1";

        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutIdentifierFactory(null)
                );
        }

        [Test]
        public void VersionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutVersionFactory(null)
                );
        }

        [Test]
        public void SchemaProjectionIsEmptyByDefault()
        {
            var sut = SutFactory();
            var result = sut.SchemaProjection;
            Assert.That(result, Is.SameAs(SqlProjection.Empty));
        }

        [Test]
        public void SchemaProjectionCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.SchemaProjection = null);
        }

        [Test]
        public void SchemaProjectionCanBeEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.SchemaProjection = SqlProjection.Empty);
        }

        [Test]
        public void SchemaProjectionCanBeNonEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.SchemaProjection = new SqlProjection(
                    new[]
                    {
                        new SqlProjectionHandler(
                            typeof (object),
                            _ => new SqlNonQueryCommand[0])
                    }));
        }

        [Test]
        public void SchemaProjectionEmptyIsPreserved()
        {
            var sut = SutFactory();
            sut.SchemaProjection = SqlProjection.Empty;
            var result = sut.SchemaProjection;
            Assert.That(result, Is.SameAs(SqlProjection.Empty));
        }

        [Test]
        public void SchemaProjectionIsPreserved()
        {
            var sut = SutFactory();
            var projection = new SqlProjection(
                new[]
                {
                    new SqlProjectionHandler(
                        typeof (object),
                        _ => new SqlNonQueryCommand[0])
                });
            sut.SchemaProjection = projection;
            var result = sut.SchemaProjection;
            Assert.That(result, Is.SameAs(projection));
        }

        [Test]
        public void ProjectionIsEmptyByDefault()
        {
            var sut = SutFactory();
            var result = sut.Projection;
            Assert.That(result, Is.SameAs(SqlProjection.Empty));
        }

        [Test]
        public void ProjectionCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.Projection = null);
        }

        [Test]
        public void ProjectionCanBeEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.Projection = SqlProjection.Empty);
        }

        [Test]
        public void ProjectionCanBeNonEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.Projection = new SqlProjection(
                    new[]
                    {
                        new SqlProjectionHandler(
                            typeof (object),
                            _ => new SqlNonQueryCommand[0])
                    }));
        }

        [Test]
        public void ProjectionEmptyIsPreserved()
        {
            var sut = SutFactory();
            sut.Projection = SqlProjection.Empty;
            var result = sut.Projection;
            Assert.That(result, Is.SameAs(SqlProjection.Empty));
        }

        [Test]
        public void ProjectionIsPreserved()
        {
            var sut = SutFactory();
            var projection = new SqlProjection(
                new[]
                {
                    new SqlProjectionHandler(
                        typeof (object),
                        _ => new SqlNonQueryCommand[0])
                });
            sut.Projection = projection;
            var result = sut.Projection;
            Assert.That(result, Is.SameAs(projection));
        }

        [Test]
        public void BuildReturnsExpectedResult()
        {
            var projection = new SqlProjection(
                new[]
                {
                    new SqlProjectionHandler(
                        typeof (object),
                        _ => new SqlNonQueryCommand[0])
                });
            var schemaProjection = new SqlProjection(
                new[]
                {
                    new SqlProjectionHandler(
                        typeof (object),
                        _ => new SqlNonQueryCommand[0])
                });
            var sut = new SqlProjectionDescriptorBuilder("identifier", "v2")
            {
                SchemaProjection = schemaProjection,
                Projection = projection
            };
            var result = sut.Build();
            Assert.That(result, Is.InstanceOf<SqlProjectionDescriptor>());
            Assert.That(result.Identifier, Is.EqualTo("identifier"));
            Assert.That(result.Version, Is.EqualTo("v2"));
            Assert.That(result.SchemaProjection, Is.SameAs(schemaProjection));
            Assert.That(result.Projection, Is.SameAs(projection));
        }

        private static SqlProjectionDescriptorBuilder SutIdentifierFactory(string identifier)
        {
            return SutFactory(identifier, Version);
        }

        private static SqlProjectionDescriptorBuilder SutVersionFactory(string version)
        {
            return SutFactory(Identifier, version);
        }

        private static SqlProjectionDescriptorBuilder SutFactory()
        {
            return new SqlProjectionDescriptorBuilder(Identifier, Version);
        }

        private static SqlProjectionDescriptorBuilder SutFactory(string identifier, string version)
        {
            return new SqlProjectionDescriptorBuilder(identifier, version);
        }
    }
}