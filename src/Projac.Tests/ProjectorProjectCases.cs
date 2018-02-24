using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    public class ProjectorProjectCases
    {
        public static IEnumerable<TestCaseData> ProjectMessageWithTokenCases()
        {
            var task = TaskFactory();
            //Match
            var token1 = new CancellationToken();
            var message1 = new object();
            var handler1 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var resolver1 = new ProjectionHandlerResolver<CallRecordingConnection>(message => new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                message1,
                token1,
                new []
                {
                    new RecordedCall(1, message1, token1)
                }).SetDescription("Case match");
            //Mismatch
            var token2 = new CancellationToken();
            var message2 = new object();
            var resolver2 = new ProjectionHandlerResolver<CallRecordingConnection>(message => new ProjectionHandler<CallRecordingConnection>[0]);
            yield return new TestCaseData(
                resolver2,
                message2,
                token2,
                Array.Empty<RecordedCall>()).SetDescription("Case mismatch");
            //Multimatch
            var token3 = new CancellationToken();
            var message3 = new object();
            var handler3 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(3, message, token);
                    return task;
                });
            var handler4 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(4, message, token);
                    return task;
                });
            var resolver3 = new ProjectionHandlerResolver<CallRecordingConnection>(message => new[] { handler3, handler4 });
            yield return new TestCaseData(
                resolver3,
                message3,
                token3,
                new []
                {
                    new RecordedCall(3, message3, token3), 
                    new RecordedCall(4, message3, token3)
                }).SetDescription("Case multimatch");;
        }

        public static IEnumerable<TestCaseData> ProjectMessageWithoutTokenCases()
        {
            var task = TaskFactory();
            //Match
            var message1 = new object();
            var handler1 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var resolver1 = new ProjectionHandlerResolver<CallRecordingConnection>(message => new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                message1,
                new []
                {
                    new RecordedCall(1, message1, CancellationToken.None), 
                });
            //Mismatch
            var message2 = new object();
            var resolver2 = new ProjectionHandlerResolver<CallRecordingConnection>(message => new ProjectionHandler<CallRecordingConnection>[0]);
            yield return new TestCaseData(
                resolver2,
                message2,
                Array.Empty<RecordedCall>());
            //Multimatch
            var message3 = new object();
            var handler3 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(3, message, token);
                    return task;
                });
            var handler4 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(4, message, token);
                    return task;
                });
            var resolver3 = new ProjectionHandlerResolver<CallRecordingConnection>(message => new[] { handler3, handler4 });
            yield return new TestCaseData(
                resolver3,
                message3,
                new []
                {
                    new RecordedCall(3, message3, CancellationToken.None), 
                    new RecordedCall(4, message3, CancellationToken.None)
                });
        }

        public static IEnumerable<TestCaseData> ProjectMessagesWithTokenCases()
        {
            var task = TaskFactory();
            //Partial match
            var token1 = new CancellationToken();
            var handler1 = new ProjectionHandler<CallRecordingConnection>(
                typeof(string),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var resolver1 = Resolve.WhenEqualToHandlerMessageType(new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                new object[] { "123", 123 },
                token1,
                new[]
                {
                    new RecordedCall(1, "123", token1)
                });
            //Mismatch
            var token2 = new CancellationToken();
            var resolver2 = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<CallRecordingConnection>[0]);
            yield return new TestCaseData(
                resolver2,
                new object[] { new object(), 123 },
                token2,
                Array.Empty<RecordedCall>());
            //Multimatch
            var token3 = new CancellationToken();
            var message3 = new object();
            var message4 = new object();
            var handler3 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var handler4 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(2, message, token);
                    return task;
                });
            var resolver3 = Resolve.WhenEqualToHandlerMessageType(new[] { handler3, handler4 });
            yield return new TestCaseData(
                resolver3,
                new object[] { message3, message4},
                token3,
                new []
                {
                    new RecordedCall(1, message3, token3),
                    new RecordedCall(2, message3, token3),
                    new RecordedCall(1, message4, token3),
                    new RecordedCall(2, message4, token3)
                });
            //Multitype Match
            var token4 = new CancellationToken();
            var handler5 = new ProjectionHandler<CallRecordingConnection>(
                typeof(string),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var handler6 = new ProjectionHandler<CallRecordingConnection>(
                typeof(int),
                (connection, message, token) =>
                {
                    connection.RecordCall(2, message, token);
                    return task;
                });
            var resolver4 = Resolve.WhenEqualToHandlerMessageType(new[] { handler5, handler6 });
            yield return new TestCaseData(
                resolver4,
                new object[] { "123", 123 },
                token4,
                new []
                {
                    new RecordedCall(1, "123", token4),
                    new RecordedCall(2, 123, token4)
                });
            //Match
            var token5 = new CancellationToken();
            var message5 = new object();
            var handler7 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var handler8 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(2, message, token);
                    return task;
                });

            var resolver5 = Resolve.WhenEqualToHandlerMessageType(new[] { handler7, handler8 });
            yield return new TestCaseData(
                resolver5,
                new object[] { message5 },
                token5,
                new []
                {
                    new RecordedCall(1, message5, token5),
                    new RecordedCall(2, message5, token5)
                });
        }

        public static IEnumerable<TestCaseData> ProjectMessagesWithoutTokenCases()
        {
            var task = TaskFactory();
            //Partial match
            var handler1 = new ProjectionHandler<CallRecordingConnection>(
                typeof(string),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var resolver1 = Resolve.WhenEqualToHandlerMessageType(new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                new object[] { "123", 123 },
                new []
                {
                    new RecordedCall(1, "123", CancellationToken.None)
                });
            //Mismatch
            var resolver2 = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<CallRecordingConnection>[0]);
            yield return new TestCaseData(
                resolver2,
                new object[] { new object(), 123 },
                Array.Empty<RecordedCall>());
            //Multimatch
            var message3 = new object();
            var message4 = new object();
            var handler3 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var handler4 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(2, message, token);
                    return task;
                });
            var resolver3 = Resolve.WhenEqualToHandlerMessageType(new[] { handler3, handler4 });
            yield return new TestCaseData(
                resolver3,
                new object[] { message3, message4 },
                new []
                {
                    new RecordedCall(1, message3, CancellationToken.None),
                    new RecordedCall(2, message3, CancellationToken.None),
                    new RecordedCall(1, message4, CancellationToken.None),
                    new RecordedCall(2, message4, CancellationToken.None)
                });
            //Multitype Match
            var handler5 = new ProjectionHandler<CallRecordingConnection>(
                typeof(string),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var handler6 = new ProjectionHandler<CallRecordingConnection>(
                typeof(int),
                (connection, message, token) =>
                {
                    connection.RecordCall(2, message, token);
                    return task;
                });
            var resolver4 = Resolve.WhenEqualToHandlerMessageType(new[] { handler5, handler6 });
            yield return new TestCaseData(
                resolver4,
                new object[] { "123", 123 },
                new []
                {
                    new RecordedCall(1, "123", CancellationToken.None),
                    new RecordedCall(2, 123, CancellationToken.None)
                });
            //Match
            var message5 = new object();
            var handler7 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(1, message, token);
                    return task;
                });
            var handler8 = new ProjectionHandler<CallRecordingConnection>(
                typeof(object),
                (connection, message, token) =>
                {
                    connection.RecordCall(2, message, token);
                    return task;
                });

            var resolver5 = Resolve.WhenEqualToHandlerMessageType(new[] { handler7, handler8 });
            yield return new TestCaseData(
                resolver5,
                new object[] { message5 },
                new []
                {
                    new RecordedCall(1, message5, CancellationToken.None),
                    new RecordedCall(2, message5, CancellationToken.None)
                });
        }

        private static Task TaskFactory()
        {
            return Task.CompletedTask;
        }
    }
}
