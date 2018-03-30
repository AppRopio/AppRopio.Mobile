using System;
using System.Threading.Tasks;
using System.Threading;

namespace AppRopio.Base.Core.Services.TasksQueue
{
    public interface ITasksQueueService
    {
        Task Append(Func<Task> task);

        Task Append(Func<Task> task, CancellationToken ct);

        Task<T> Append<T>(Func<Task<T>> task);

        Task<T> Append<T>(Func<Task<T>> task, CancellationToken ct);
    }
}
