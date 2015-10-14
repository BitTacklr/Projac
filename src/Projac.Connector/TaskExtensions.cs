using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    internal static class TaskExtensions
    {
        public static Task ExecuteAsync(this IEnumerable<Task> enumerable, CancellationToken cancellationToken)
        {
            var source = new TaskCompletionSource<object>();
            var enumerator = enumerable.GetEnumerator();
            source.Task.
                ContinueWith(
                    next => enumerator.Dispose(),
                    cancellationToken,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current);
            ExecuteAsyncCore(source, enumerator, cancellationToken);
            return source.Task;
        }

        private static void ExecuteAsyncContinuation(Task previous, TaskCompletionSource<object> source, IEnumerator<Task> enumerator, CancellationToken cancellationToken)
        {
            if (!previous.IsCanceled || !previous.IsFaulted)
            {
                ExecuteAsyncCore(source, enumerator, cancellationToken);
            }
        }

        private static void ExecuteAsyncCore(TaskCompletionSource<object> source, IEnumerator<Task> enumerator, CancellationToken cancellationToken)
        {
            try
            {
                if (enumerator.MoveNext())
                {
                    enumerator.Current.
                        ContinueWith(
                            next => ExecuteAsyncContinuation(next, source, enumerator, cancellationToken),
                            cancellationToken,
                            TaskContinuationOptions.ExecuteSynchronously,
                            TaskScheduler.Current);
                }
                else
                {
                    source.SetResult(null);
                }
            }
            catch (Exception exception)
            {
                source.SetException(exception);
            }
        }
    }
}