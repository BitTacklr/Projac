using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using Paramol;
using Projac.Tests.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionHandlerEnumeratorTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SqlProjectionHandlerEnumerator(null));
        }

        [Test]
        public void DisposeDoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                new SqlProjectionHandlerEnumerator(new SqlProjectionHandler[0]));
        }

        [TestCaseSource("MoveNextCases")]
        public void MoveNextReturnsExpectedResult(
            SqlProjectionHandlerEnumerator sut, bool expected)
        {
            var result = sut.MoveNext();
            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> MoveNextCases()
        {
            //No handlers
            var enumerator1 = new SqlProjectionHandlerEnumerator(new SqlProjectionHandler[0]);
            yield return new TestCaseData(enumerator1, false);
            yield return new TestCaseData(enumerator1, false); //idempotency check

            //1 handler
            var enumerator2 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator2, true);
            yield return new TestCaseData(enumerator2, false);
            yield return new TestCaseData(enumerator2, false); //idempotency check

            //2 handlers
            var enumerator3 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator3, true);
            yield return new TestCaseData(enumerator3, true);
            yield return new TestCaseData(enumerator3, false);
            yield return new TestCaseData(enumerator3, false); //idempotency check
        }

        [TestCaseSource("MoveNextAfterResetCases")]
        public void MoveNextAfterResetReturnsExpectedResult(
            SqlProjectionHandlerEnumerator sut, bool expected)
        {
            sut.Reset();

            var result = sut.MoveNext();

            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> MoveNextAfterResetCases()
        {
            //No handlers
            var enumerator1 = new SqlProjectionHandlerEnumerator(new SqlProjectionHandler[0]);
            yield return new TestCaseData(enumerator1, false);
            yield return new TestCaseData(enumerator1, false);

            //1 handler
            var enumerator2 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator2, true);
            yield return new TestCaseData(enumerator2, true);

            //2 handlers
            var enumerator3 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator3, true);
            yield return new TestCaseData(enumerator3, true);
        }

        [TestCaseSource("ResetCases")]
        public void ResetDoesNotThrow(SqlProjectionHandlerEnumerator sut)
        {
            Assert.DoesNotThrow(sut.Reset);
        }

        private static IEnumerable<TestCaseData> ResetCases()
        {
            //No handlers
            var enumerator1 = new SqlProjectionHandlerEnumerator(new SqlProjectionHandler[0]);
            yield return new TestCaseData(enumerator1);

            //1 handler
            var enumerator2 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator2);

            //2 handlers
            var enumerator3 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator3);
        }

        [TestCaseSource("CurrentNotStartedCases")]
        public void CurrentReturnsExpectedResultWhenNotStarted(
            SqlProjectionHandlerEnumerator sut)
        {
            Assert.Throws<InvalidOperationException>(
                () => { var _ = sut.Current; });
        }

        [TestCaseSource("CurrentNotStartedCases")]
        public void EnumeratorCurrentReturnsExpectedResultWhenNotStarted(
            IEnumerator sut)
        {
            Assert.Throws<InvalidOperationException>(
                () => { var _ = sut.Current; });
        }

        private static IEnumerable<TestCaseData> CurrentNotStartedCases()
        {
            //No handlers
            var enumerator1 = new SqlProjectionHandlerEnumerator(new SqlProjectionHandler[0]);
            yield return new TestCaseData(enumerator1);

            //1 handler
            var enumerator2 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator2);

            //2 handlers
            var enumerator3 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
            yield return new TestCaseData(enumerator3);
        }

        [TestCaseSource("CurrentCompletedCases")]
        public void CurrentReturnsExpectedResultWhenCompleted(
            SqlProjectionHandlerEnumerator sut)
        {
            Assert.Throws<InvalidOperationException>(
                () => { var _ = sut.Current; });
        }

        [TestCaseSource("CurrentCompletedCases")]
        public void EnumeratorCurrentReturnsExpectedResultWhenCompleted(
            IEnumerator sut)
        {
            Assert.Throws<InvalidOperationException>(
                () => { var _ = sut.Current; });
        }

        private static IEnumerable<TestCaseData> CurrentCompletedCases()
        {
            //No handlers
            var enumerator1 = new SqlProjectionHandlerEnumerator(new SqlProjectionHandler[0]);
            while (enumerator1.MoveNext()) { }
            yield return new TestCaseData(enumerator1);

            //1 handler
            var enumerator2 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
            while (enumerator2.MoveNext()) { }
            yield return new TestCaseData(enumerator2);

            //2 handlers
            var enumerator3 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
            while (enumerator3.MoveNext()) { }
            yield return new TestCaseData(enumerator3);
        }

        [TestCaseSource("CurrentStartedCases")]
        public void CurrentReturnsExpectedResultWhenStarted(
            SqlProjectionHandlerEnumerator sut, SqlNonQueryCommand[] expected)
        {
            sut.MoveNext();

            var result = sut.Current.Handler(null);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCaseSource("CurrentStartedCases")]
        public void EnumeratorCurrentReturnsExpectedResultWhenStarted(
            IEnumerator sut, SqlNonQueryCommand[] expected)
        {
            sut.MoveNext();

            var result = ((SqlProjectionHandler)sut.Current).Handler(null);

            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> CurrentStartedCases()
        {
            //No handlers - not applicable

            //1 handler
            var command1 = CommandFactory();
            var enumerator2 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(command1)
                });
            yield return new TestCaseData(enumerator2, new[]
                {
                    command1
                });

            //2 handlers
            var command2 = CommandFactory();
            var command3 = CommandFactory();
            var enumerator3 = new SqlProjectionHandlerEnumerator(new[]
                {
                    HandlerFactory(command2),
                    HandlerFactory(command3)
                });
            yield return new TestCaseData(enumerator3, new[]
                {
                    command2
                });
            yield return new TestCaseData(enumerator3, new[]
                {
                    command3
                });
        }

        private static SqlProjectionHandler HandlerFactory(SqlNonQueryCommand command)
        {
            return new SqlProjectionHandler(
                typeof(object),
                o => new[] { command });
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
        }
    }
}
