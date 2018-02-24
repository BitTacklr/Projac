using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    public class ProjectorWithMetadataProjectCases
    {
        public static IEnumerable<TestCaseData> ProjectMessageWithTokenCases()
        {
            var task = TaskFactory();
            //Match
            var token1 = new CancellationToken();
            var message1 = new object();
            var metadata1 = new object();
            var handler1 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var resolver1 = new ProjectionHandlerResolver<CallRecordingConnection, object>(message => new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                message1,
                metadata1,
                token1,
                new[]
                {
                    new RecordedCall(1, message1, metadata1, token1)
                });
            //Mismatch
            var token2 = new CancellationToken();
            var message2 = new object();
            var metadata2 = new object();
            var resolver2 = new ProjectionHandlerResolver<CallRecordingConnection, object>(message => new ProjectionHandler<CallRecordingConnection, object>[0]);
            yield return new TestCaseData(
                resolver2,
                message2,
                metadata2,
                token2,
                Array.Empty<RecordedCall>());
            //Multimatch
            var token3 = new CancellationToken();
            var message3 = new object();
            var metadata3 = new object();
            var handler3 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(3, message, metadata, token);
                    return task;
                });
            var handler4 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(4, message, metadata, token);
                    return task;
                });
            var resolver3 = new ProjectionHandlerResolver<CallRecordingConnection, object>(message => new[] { handler3, handler4 });
            yield return new TestCaseData(
                resolver3,
                message3,
                metadata3,
                token3,
                new[]
                {
                    new RecordedCall(3, message3, metadata3, token3), 
                    new RecordedCall(4, message3, metadata3, token3)
                });
        }

        public static IEnumerable<TestCaseData> ProjectMessageWithoutTokenCases()
        {
            var task = TaskFactory();
            //Match
            var message1 = new object();
            var metadata1 = new object();
            var handler1 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var resolver1 = new ProjectionHandlerResolver<CallRecordingConnection, object>(message => new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                message1,
                metadata1,
                new[]
                {
                    new RecordedCall(1, message1, metadata1, CancellationToken.None), 
                });
            //Mismatch
            var message2 = new object();
            var metadata2 = new object();
            var resolver2 = new ProjectionHandlerResolver<CallRecordingConnection, object>(message => new ProjectionHandler<CallRecordingConnection, object>[0]);
            yield return new TestCaseData(
                resolver2,
                message2,
                metadata2,
                Array.Empty<RecordedCall>());
            //Multimatch
            var message3 = new object();
            var metadata3 = new object();
            var handler3 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(3, message, metadata, token);
                    return task;
                });
            var handler4 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(4, message, metadata, token);
                    return task;
                });
            var resolver3 = new ProjectionHandlerResolver<CallRecordingConnection, object>(message => new[] { handler3, handler4 });
            yield return new TestCaseData(
                resolver3,
                message3,
                metadata3,
                new[]
                {
                    new RecordedCall(3, message3, metadata3, CancellationToken.None), 
                    new RecordedCall(4, message3, metadata3, CancellationToken.None)
                });
        }

        public static IEnumerable<TestCaseData> ProjectMessagesWithTokenCases()
        {
            var task = TaskFactory();
            //Partial match
            var token1 = new CancellationToken();
            var metadata1 = new object();
            var metadata2 = new object();
            var handler1 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(string),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var resolver1 = Resolve.WhenEqualToHandlerMessageType(new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                new ValueTuple<object, object>[] 
                { 
                    ValueTuple.Create<object, object>("123", metadata1),
                    ValueTuple.Create<object, object>(123, metadata2)
                },
                token1,
                new[]
                {
                    new RecordedCall(1, "123", metadata1, token1)
                });
            //Mismatch
            var token3 = new CancellationToken();
            var metadata3 = new object();
            var metadata4 = new object();
            var resolver3 = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<CallRecordingConnection, object>[0]);
            yield return new TestCaseData(
                resolver3,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>(new object(), metadata3),
                    ValueTuple.Create<object, object>(123, metadata4)
                },
                token3,
                Array.Empty<RecordedCall>());
            //Multimatch
            var token5 = new CancellationToken();
            var message5 = new object();
            var message6 = new object();
            var metadata5 = new object();
            var metadata6 = new object();
            var handler5 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var handler6 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(2, message, metadata, token);
                    return task;
                });
            var resolver5 = Resolve.WhenEqualToHandlerMessageType(new[] { handler5, handler6 });
            yield return new TestCaseData(
                resolver5,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>(message5, metadata5),
                    ValueTuple.Create<object, object>(message6, metadata6)
                },
                token5,
                new[]
                {
                    new RecordedCall(1, message5, metadata5, token5),
                    new RecordedCall(2, message5, metadata5, token5),
                    new RecordedCall(1, message6, metadata6, token5),
                    new RecordedCall(2, message6, metadata6, token5)
                });
            //Multitype Match
            var token7 = new CancellationToken();
            var metadata7 = new object();
            var metadata8 = new object();
            var handler7 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(string),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var handler8 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(int),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(2, message, metadata, token);
                    return task;
                });
            var resolver7 = Resolve.WhenEqualToHandlerMessageType(new[] { handler7, handler8 });
            yield return new TestCaseData(
                resolver7,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>("123", metadata7),
                    ValueTuple.Create<object, object>(123, metadata8)
                },
                token7,
                new[]
                {
                    new RecordedCall(1, "123", metadata7, token7),
                    new RecordedCall(2, 123, metadata8, token7)
                });
            //Match
            var token9 = new CancellationToken();
            var message9 = new object();
            var metadata9 = new object();
            var handler9 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var handler10 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(2, message, metadata, token);
                    return task;
                });

            var resolver9 = Resolve.WhenEqualToHandlerMessageType(new[] { handler9, handler10 });
            yield return new TestCaseData(
                resolver9,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>(message9, metadata9)
                },
                token9,
                new[]
                {
                    new RecordedCall(1, message9, metadata9, token9),
                    new RecordedCall(2, message9, metadata9, token9)
                });
        }

        public static IEnumerable<TestCaseData> ProjectMessagesWithoutTokenCases()
        {
            var task = TaskFactory();
            //Partial match
            var metadata1 = new object();
            var metadata2 = new object();
            var handler1 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(string),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var resolver1 = Resolve.WhenEqualToHandlerMessageType(new[] { handler1 });
            yield return new TestCaseData(
                resolver1,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>("123", metadata1),
                    ValueTuple.Create<object, object>(123, metadata2)
                },
                new[]
                {
                    new RecordedCall(1, "123", metadata1, CancellationToken.None)
                });
            //Mismatch
            var metadata3 = new object();
            var metadata4 = new object();
            var resolver3 = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<CallRecordingConnection, object>[0]);
            yield return new TestCaseData(
                resolver3,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>(new object(), metadata3),
                    ValueTuple.Create<object, object>(123, metadata4)
                },
                Array.Empty<RecordedCall>());
            //Multimatch
            var message5 = new object();
            var message6 = new object();
            var metadata5 = new object();
            var metadata6 = new object();
            var handler5 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var handler6 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(2, message, metadata, token);
                    return task;
                });
            var resolver5 = Resolve.WhenEqualToHandlerMessageType(new[] { handler5, handler6 });
            yield return new TestCaseData(
                resolver5,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>(message5, metadata3),
                    ValueTuple.Create<object, object>(message6, metadata5)
                },
                new[]
                {
                    new RecordedCall(1, message5, metadata3, CancellationToken.None),
                    new RecordedCall(2, message5, metadata3, CancellationToken.None),
                    new RecordedCall(1, message6, metadata5, CancellationToken.None),
                    new RecordedCall(2, message6, metadata5, CancellationToken.None)
                });
            //Multitype Match
            var metadata7 = new object();
            var metadata8 = new object();
            var handler7 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(string),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var handler8 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(int),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(2, message, metadata, token);
                    return task;
                });
            var resolver7 = Resolve.WhenEqualToHandlerMessageType(new[] { handler7, handler8 });
            yield return new TestCaseData(
                resolver7,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>("123", metadata7),
                    ValueTuple.Create<object, object>(123, metadata8)
                },
                new[]
                {
                    new RecordedCall(1, "123", metadata7, CancellationToken.None),
                    new RecordedCall(2, 123, metadata8, CancellationToken.None)
                });
            //Match
            var message9 = new object();
            var metadata9 = new object();
            var handler9 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(1, message, metadata, token);
                    return task;
                });
            var handler10 = new ProjectionHandler<CallRecordingConnection, object>(
                typeof(object),
                (connection, message, metadata, token) =>
                {
                    connection.RecordCall(2, message, metadata, token);
                    return task;
                });

            var resolver9 = Resolve.WhenEqualToHandlerMessageType(new[] { handler9, handler10 });
            yield return new TestCaseData(
                resolver9,
                new ValueTuple<object, object>[] 
                {
                    ValueTuple.Create<object, object>(message9, metadata9)
                },
                new[]
                {
                    new RecordedCall(1, message9, metadata9, CancellationToken.None),
                    new RecordedCall(2, message9, metadata9, CancellationToken.None)
                });
        }

        private static Task TaskFactory()
        {
            return Task.CompletedTask;
        }
    }
}
