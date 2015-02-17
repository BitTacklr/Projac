using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Projac.WindowsAzure.Storage
{
    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class AsyncCloudTableProjector
    {
        private readonly Dictionary<Type, HandlerNode> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCloudTableProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> is <c>null</c>.</exception>
        public AsyncCloudTableProjector(CloudTableProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, ToLinkedList);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(CloudTableClient client, object message)
        {
            return ProjectAsync(client, message, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(CloudTableClient client, object message, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (message == null) throw new ArgumentNullException("message");

            HandlerNode handlerNode;
            if (_handlers.TryGetValue(message.GetType(), out handlerNode) && handlerNode != null)
            {
                return handlerNode.
                    Handler.Handler(client, message, cancellationToken).
                    ContinueWith(
                        _ => ProjectMessageAsyncContinuation(handlerNode.Next, client, message, cancellationToken), 
                        cancellationToken,
                        TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted,
                        TaskScheduler.Current).
                    Unwrap();
            }
            return Task.FromResult(false);
        }

        private static Task ProjectMessageAsyncContinuation(HandlerNode handlerNode, CloudTableClient client, object message, CancellationToken cancellationToken)
        {
            if (handlerNode != null)
            {
                return handlerNode.
                    Handler.Handler(client, message, cancellationToken).
                    ContinueWith(
                        _ => ProjectMessageAsyncContinuation(handlerNode.Next, client, message, cancellationToken),
                        cancellationToken,
                        TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted,
                        TaskScheduler.Current);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Projects the specified messages to project.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public Task ProjectAsync(CloudTableClient client, IEnumerable<object> messages)
        {
            return ProjectAsync(client, messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public Task ProjectAsync(CloudTableClient client, IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (messages == null) throw new ArgumentNullException("messages");

            var sequence = new TaskContinuationSequence(
                new TaskCompletionSource<object>(),
                new HandleMessageTaskEnumerator(
                    _handlers,
                    messages,
                    client,
                    cancellationToken),
                cancellationToken);

            if (sequence.Tasks.MoveNext())
            {
                sequence.
                    Tasks.
                    Current.
                    ContinueWith(
                        parentTask => ContinueProjectAsync(
                            parentTask,
                            sequence),
                        cancellationToken);
                        //,
                        //TaskContinuationOptions.ExecuteSynchronously, 
                        //TaskScheduler.Current);
                return sequence.TaskCompletionSource.Task;
            }
            sequence.Tasks.Dispose();
            return sequence.TaskCompletionSource.Task;
        }

        private static void ContinueProjectAsync(
            Task antecedentTask,
            TaskContinuationSequence sequence)
        {
            if (antecedentTask.IsCanceled)
            {
                sequence.Tasks.Dispose();
                sequence.TaskCompletionSource.SetCanceled();
            }
            else if (antecedentTask.IsFaulted)
            {
                sequence.Tasks.Dispose();
                sequence.TaskCompletionSource.SetException(antecedentTask.Exception);
            }
            else
            {
                if (sequence.Tasks.MoveNext())
                {
                    sequence.
                        Tasks.
                        Current.
                        ContinueWith(
                            parentTask => ContinueProjectAsync(
                                parentTask,
                                sequence),
                            sequence.CancellationToken);
                            //,
                            //TaskContinuationOptions.ExecuteSynchronously, 
                            //TaskScheduler.Current);
                }
                else
                {
                    sequence.Tasks.Dispose();
                    sequence.TaskCompletionSource.SetResult(null);
                }
            }
        }

        private static HandlerNode ToLinkedList(IEnumerable<CloudTableProjectionHandler> handlers)
        {
            using (var enumerator = handlers.GetEnumerator())
            {
                HandlerNode handlerNode = null;
                while (enumerator.MoveNext())
                {
                    handlerNode = new HandlerNode(enumerator.Current, handlerNode);
                }
                return handlerNode;
            }
        }

        private class HandlerNode
        {
            public readonly CloudTableProjectionHandler Handler;
            public readonly HandlerNode Next;

            public HandlerNode(CloudTableProjectionHandler handler, HandlerNode next)
            {
                Handler = handler;
                Next = next;
            }
        }

        private class TaskContinuationSequence
        {
            private readonly TaskCompletionSource<object> _taskCompletionSource;
            private readonly IEnumerator<Task> _tasks;
            private readonly CancellationToken _cancellationToken;

            public TaskContinuationSequence(
                TaskCompletionSource<object> taskCompletionSource,
                IEnumerator<Task> tasks,
                CancellationToken cancellationToken)
            {
                if (taskCompletionSource == null) throw new ArgumentNullException("taskCompletionSource");
                if (tasks == null) throw new ArgumentNullException("tasks");
                _taskCompletionSource = taskCompletionSource;
                _tasks = tasks;
                _cancellationToken = cancellationToken;
            }

            public TaskCompletionSource<object> TaskCompletionSource
            {
                get { return _taskCompletionSource; }
            }

            public IEnumerator<Task> Tasks
            {
                get { return _tasks; }
            }

            public CancellationToken CancellationToken
            {
                get { return _cancellationToken; }
            }
        }

        private class HandleMessageTaskEnumerator : IEnumerator<Task>
        {
            private readonly Dictionary<Type, HandlerNode> _handlers;
            private readonly IEnumerator<object> _enumerator;
            private readonly CloudTableClient _client;
            private readonly CancellationToken _cancellationToken;
            private bool _moved;
            private HandlerNode _node;
            private Task _task;

            public HandleMessageTaskEnumerator(
                Dictionary<Type, HandlerNode> handlers, 
                IEnumerable<object> messages,
                CloudTableClient client,
                CancellationToken cancellationToken)
            {
                _handlers = handlers;
                _enumerator = messages.GetEnumerator();
                _client = client;
                _cancellationToken = cancellationToken;
                _moved = false;
                _node = null;
                _task = null;
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                _task = null;
                if (_node != null)
                {
                    _node = _node.Next;
                    if (_node != null) return true;
                }
                _moved = _enumerator.MoveNext();
                while (_moved && (!_handlers.TryGetValue(_enumerator.Current.GetType(), out _node) || _node == null))
                {
                    _moved = _enumerator.MoveNext();
                }
                return _moved;
            }

            public void Reset()
            {
                _enumerator.Reset();
                _moved = false;
                _node = null;
                _task = null;
            }

            public Task Current
            {
                get
                {
                    return _task ?? (_task = _node.
                        Handler.
                        Handler(_client, _enumerator.Current, _cancellationToken));
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
