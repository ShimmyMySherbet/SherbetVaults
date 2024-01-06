using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Core.Logging;
using SherbetVaults.Models;

namespace SherbetVaults.Database
{
    public class DatabaseQueue<T> : IDisposable
    {
        private readonly ConcurrentQueue<AsyncDatabaseAction<T>> m_DatabaseQueue = new();
        private readonly SemaphoreSlim m_QueueSemaphore = new(0);

        private CancellationTokenSource m_TokenSource = new();
        public CancellationToken Token => m_TokenSource.Token;

        public T Table { get; }

        public DatabaseQueue(T table)
        {
            Table = table;
        }

        public void Enqueue(AsyncDatabaseAction<T> action)
        {
            m_DatabaseQueue.Enqueue(action);
            m_QueueSemaphore.Release();
        }

        public void FlushQueue()
        {
            SpinWait.SpinUntil(() => m_DatabaseQueue.IsEmpty);
        }

        public void StopWorker()
        {
            m_TokenSource.Cancel();
        }

        public void StartWorker()
        {
            if (Token.IsCancellationRequested)
            {
                m_TokenSource.Dispose();
                m_TokenSource = new CancellationTokenSource();
            }
            Task.Run(RunDatabaseQueue);
        }

        private async Task RunDatabaseQueue()
        {
            const int maxRetries = 3;
            const int retryDelayMilliseconds = 1000; // You can adjust the delay time

            while (!Token.IsCancellationRequested)
            {
                await m_QueueSemaphore.WaitAsync(Token);
                if (m_DatabaseQueue.TryDequeue(out var action))
                {
                    int remainingRetries = maxRetries;

                RetryLabel:

                    try
                    {
                        await action(Table);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning($"Failed to write database action: {ex.Message}");
                        Logger.LogWarning($"Table Type: {typeof(T).FullName}");
                        Logger.LogWarning(ex.StackTrace);

                        if (remainingRetries > 0)
                        {
                            remainingRetries--;
                            await Task.Delay(retryDelayMilliseconds);
                            goto RetryLabel;
                        }
                        else
                        {
                            Logger.LogError($"Failed after all attempts. Skipping action.");
                        }
                    }
                }
            }
        }


        public void Dispose()
        {
            StopWorker();
            m_QueueSemaphore.Dispose();
            m_TokenSource.Dispose();
        }
    }
}