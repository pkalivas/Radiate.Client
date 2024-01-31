using System.Collections.Concurrent;

namespace Radiate.Client.Services.Worker;

public class WorkItemQueue : IWorkItemQueue
{
    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();
    private readonly SemaphoreSlim _signal = new(0);

    public void Enqueue(Func<CancellationToken, Task> workItem)
    {
        _workItems.Enqueue(workItem);
        _signal.Release();
    }

    async Task<Func<CancellationToken, Task>?> IWorkItemQueue.Dequeue(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        return _workItems.TryDequeue(out var workItem) ? workItem : null;
    }
}