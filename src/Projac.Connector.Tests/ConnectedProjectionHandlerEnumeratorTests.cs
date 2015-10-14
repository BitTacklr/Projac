using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Connector.Tests
{
    [TestFixture]
    public class ConnectedProjectionHandlerEnumeratorTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedProjectionHandlerEnumerator<object>(null));
        }

        [Test]
        public void DisposeDoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                new ConnectedProjectionHandlerEnumerator<object>(new ConnectedProjectionHandler<object>[0]));
        }

        [TestCaseSource("MoveNextCases")]
        public void MoveNextReturnsExpectedResult(
            ConnectedProjectionHandlerEnumerator<object> sut, bool expected)
        {
            var result = sut.MoveNext();
            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> MoveNextCases()
        {
            //No handlers
            var enumerator1 = new ConnectedProjectionHandlerEnumerator<object>(new ConnectedProjectionHandler<object>[0]);
            yield return new TestCaseData(enumerator1, false);
            yield return new TestCaseData(enumerator1, false); //idempotency check

            //1 handler
            var enumerator2 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator2, true);
            yield return new TestCaseData(enumerator2, false);
            yield return new TestCaseData(enumerator2, false); //idempotency check

            //2 handlers
            var enumerator3 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory()),
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator3, true);
            yield return new TestCaseData(enumerator3, true);
            yield return new TestCaseData(enumerator3, false);
            yield return new TestCaseData(enumerator3, false); //idempotency check
        }

        [TestCaseSource("MoveNextAfterResetCases")]
        public void MoveNextAfterResetReturnsExpectedResult(
            ConnectedProjectionHandlerEnumerator<object> sut, bool expected)
        {
            sut.Reset();

            var result = sut.MoveNext();

            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> MoveNextAfterResetCases()
        {
            //No handlers
            var enumerator1 = new ConnectedProjectionHandlerEnumerator<object>(new ConnectedProjectionHandler<object>[0]);
            yield return new TestCaseData(enumerator1, false);
            yield return new TestCaseData(enumerator1, false);

            //1 handler
            var enumerator2 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator2, true);
            yield return new TestCaseData(enumerator2, true);

            //2 handlers
            var enumerator3 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory()),
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator3, true);
            yield return new TestCaseData(enumerator3, true);
        }

        [TestCaseSource("ResetCases")]
        public void ResetDoesNotThrow(ConnectedProjectionHandlerEnumerator<object> sut)
        {
            Assert.DoesNotThrow(sut.Reset);
        }

        private static IEnumerable<TestCaseData> ResetCases()
        {
            //No handlers
            var enumerator1 = new ConnectedProjectionHandlerEnumerator<object>(new ConnectedProjectionHandler<object>[0]);
            yield return new TestCaseData(enumerator1);

            //1 handler
            var enumerator2 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator2);

            //2 handlers
            var enumerator3 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory()),
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator3);
        }

        [TestCaseSource("CurrentNotStartedCases")]
        public void CurrentReturnsExpectedResultWhenNotStarted(
            ConnectedProjectionHandlerEnumerator<object> sut)
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
            var enumerator1 = new ConnectedProjectionHandlerEnumerator<object>(new ConnectedProjectionHandler<object>[0]);
            yield return new TestCaseData(enumerator1);

            //1 handler
            var enumerator2 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator2);

            //2 handlers
            var enumerator3 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory()),
                HandlerFactory(TaskFactory())
            });
            yield return new TestCaseData(enumerator3);
        }

        [TestCaseSource("CurrentCompletedCases")]
        public void CurrentReturnsExpectedResultWhenCompleted(
            ConnectedProjectionHandlerEnumerator<object> sut)
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
            var enumerator1 = new ConnectedProjectionHandlerEnumerator<object>(new ConnectedProjectionHandler<object>[0]);
            while (enumerator1.MoveNext()) { }
            yield return new TestCaseData(enumerator1);

            //1 handler
            var enumerator2 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory())
            });
            while (enumerator2.MoveNext()) { }
            yield return new TestCaseData(enumerator2);

            //2 handlers
            var enumerator3 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(TaskFactory()),
                HandlerFactory(TaskFactory())
            });
            while (enumerator3.MoveNext()) { }
            yield return new TestCaseData(enumerator3);
        }

        [TestCaseSource("CurrentStartedCases")]
        public void CurrentReturnsExpectedResultWhenStarted(
            ConnectedProjectionHandlerEnumerator<object> sut, Task expected)
        {
            sut.MoveNext();

            var result = sut.Current.Handler(null, null, CancellationToken.None);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCaseSource("CurrentStartedCases")]
        public void EnumeratorCurrentReturnsExpectedResultWhenStarted(
            IEnumerator sut, Task expected)
        {
            sut.MoveNext();

            var result = ((ConnectedProjectionHandler<object>)sut.Current).Handler(null, null, CancellationToken.None);

            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> CurrentStartedCases()
        {
            //No handlers - not applicable

            //1 handler
            var task1 = TaskFactory();
            var enumerator2 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(task1)
            });
            yield return new TestCaseData(enumerator2, task1);

            //2 handlers
            var task2 = TaskFactory();
            var task3 = TaskFactory();
            var enumerator3 = new ConnectedProjectionHandlerEnumerator<object>(new[]
            {
                HandlerFactory(task2),
                HandlerFactory(task3)
            });
            yield return new TestCaseData(enumerator3, task2);
            yield return new TestCaseData(enumerator3, task3);
        }

        private static ConnectedProjectionHandler<object> HandlerFactory(Task task)
        {
            return new ConnectedProjectionHandler<object>(
                typeof(object),
                (_, __, ___) => task);
        }

        private static Task TaskFactory()
        {
            return Task.FromResult<object>(null);
        }
    }
}