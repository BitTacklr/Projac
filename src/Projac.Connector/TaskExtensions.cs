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
            if (!previous.IsCanceled && !cancellationToken.IsCancellationRequested)
            {
                if (previous.IsFaulted && previous.Exception != null)
                {
                    source.SetException(previous.Exception);
                }
                else
                {
                    ExecuteAsyncCore(source, enumerator, cancellationToken);
                }
            }
            else
            {
                source.SetCanceled();
            }
        }

        private static void ExecuteAsyncCore(TaskCompletionSource<object> source, IEnumerator<Task> enumerator, CancellationToken cancellationToken)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    source.SetCanceled();
                }
                else
                {
                    if (enumerator.MoveNext())
                    {
                        if (enumerator.Current.IsCompleted)
                        {
                            if (!enumerator.Current.IsCanceled)
                            {
                                if (enumerator.Current.IsFaulted && enumerator.Current.Exception != null)
                                {
                                    source.SetException(enumerator.Current.Exception);
                                }
                                else
                                {
                                    enumerator.Current.
                                        ContinueWith(
                                            next =>
                                                ExecuteAsyncContinuation(next, source, enumerator, cancellationToken),
                                            cancellationToken,
                                            TaskContinuationOptions.ExecuteSynchronously,
                                            TaskScheduler.Current);
                                }
                            }
                            else
                            {
                                source.SetCanceled();
                            }
                        }
                        else
                        {
                            enumerator.Current.
                                ContinueWith(
                                    next => ExecuteAsyncContinuation(next, source, enumerator, cancellationToken),
                                    cancellationToken,
                                    TaskContinuationOptions.ExecuteSynchronously,
                                    TaskScheduler.Current);
                        }
                    }
                    else
                    {
                        source.SetResult(null);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                source.SetCanceled();
            }
            catch (OperationCanceledException)
            {
                source.SetCanceled();
            }
            catch (Exception exception)
            {
                source.SetException(exception);
            }
        }
    }
}