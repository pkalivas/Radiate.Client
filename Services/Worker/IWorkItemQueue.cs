namespace Radiate.Client.Services.Worker;

public interface IWorkItemQueue
{
    void Enqueue(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>?> Dequeue(CancellationToken cancellationToken);
}