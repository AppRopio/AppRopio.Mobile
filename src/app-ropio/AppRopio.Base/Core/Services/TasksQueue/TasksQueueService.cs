using System.Threading.Tasks;
using System.Threading;
using System;

namespace AppRopio.Base.Core.Services.TasksQueue
{
    public class TasksQueueService : ITasksQueueService
    {
        #region Fields

        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        #endregion

        #region IBasketTasksQueueService implementation

        public async Task Append(Func<Task> task)
        {
            await _semaphore.WaitAsync();
            try
            {
                await task();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task Append(Func<Task> task, CancellationToken ct)
        {
            await _semaphore.WaitAsync();
            try
            {
                ct.ThrowIfCancellationRequested();
                await task();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> Append<T>(Func<Task<T>> task)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await task();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> Append<T>(Func<Task<T>> task, CancellationToken ct)
        {
            await _semaphore.WaitAsync();
            try
            {
                ct.ThrowIfCancellationRequested();
                return await task();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        #endregion
    }
}
