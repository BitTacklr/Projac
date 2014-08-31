using System;
using System.Data.Common;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionDescriptorBuilderTests
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
        public void DataDefinitionStatementsAreEmptyByDefault()
        {
            var sut = SutFactory();
            var result = sut.DataDefinitionStatements;
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void DataDefinitionStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.DataDefinitionStatements = null);
        }

        [Test]
        public void DataDefinitionStatementsCanBeEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.DataDefinitionStatements = new SqlNonQueryStatement[0]);
        }

        [Test]
        public void DataDefinitionStatementsCanBeNonEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.DataDefinitionStatements = new[] { StatementFactory(), StatementFactory() });
        }

        [Test]
        public void DataDefinitionStatementsArePreserved()
        {
            var sut = SutFactory();
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();
            sut.DataDefinitionStatements = new[] { statement1, statement2 };
            var result = sut.DataDefinitionStatements;
            Assert.That(result, Is.EquivalentTo(new[] { statement1, statement2 }));
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
                            _ => new SqlNonQueryStatement[0])
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
                        _ => new SqlNonQueryStatement[0])
                });
            sut.Projection = projection;
            var result = sut.Projection;
            Assert.That(result, Is.SameAs(projection));
        }

        [Test]
        public void BuildReturnsExpectedResult()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();
            var projection = new SqlProjection(
                new[]
                {
                    new SqlProjectionHandler(
                        typeof (object),
                        _ => new SqlNonQueryStatement[0])
                });
            var sut = new SqlProjectionDescriptorBuilder("identifier")
            {
                DataDefinitionStatements = new[] {statement1, statement2},
                Projection = projection
            };
            var result = sut.Build();
            Assert.That(result, Is.InstanceOf<SqlProjectionDescriptor>());
            Assert.That(result.Identifier, Is.EqualTo("identifier"));
            Assert.That(result.DataDefinitionStatements, Is.EquivalentTo(new[] { statement1, statement2 }));
            Assert.That(result.Projection, Is.SameAs(projection));
        }

        private static SqlProjectionDescriptorBuilder SutFactory(string identifier = Identifier)
        {
            return new SqlProjectionDescriptorBuilder(identifier);
        }

        private static SqlNonQueryStatement StatementFactory()
        {
            return new SqlNonQueryStatement("text", new DbParameter[0]);
        }
    }
}