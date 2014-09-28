using System;
using NUnit.Framework;
using Paramol;
using Projac;

namespace Recipes.Descriptors
{
    [TestFixture]
    public class SqlProjectionDescriptorBuilderTests
    {
        private const string Identifier = "id";

        private static readonly SqlProjectionDescriptor Descriptor = new SqlProjectionDescriptorBuilder().Build();

        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory(null)
                );
        }

        [Test]
        public void IdentifierAssumesCallerMemberName()
        {
            Assert.That(Descriptor.Identifier, Is.EqualTo("Descriptor"));
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
            var sut =  new SqlProjectionDescriptorBuilder("identifier")
            {
                Projection = projection
            };
            var result = sut.Build();
            Assert.That(result, Is.InstanceOf<SqlProjectionDescriptor>());
            Assert.That(result.Identifier, Is.EqualTo("identifier"));
            Assert.That(result.Projection, Is.SameAs(projection));
        }

        private static SqlProjectionDescriptorBuilder SutFactory()
        {
            return new SqlProjectionDescriptorBuilder(Identifier);
        }

        private static SqlProjectionDescriptorBuilder SutFactory(string identifier)
        {
            return new SqlProjectionDescriptorBuilder(identifier);
        }
    }
}