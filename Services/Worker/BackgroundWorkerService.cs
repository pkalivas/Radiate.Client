namespace Radiate.Client.Services.Worker;

public class BackgroundWorkerService : BackgroundService
{
    private readonly ILogger<BackgroundWorkerService> _logger;

    public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger,
        IWorkItemQueue workItemQueue)
    {
        _logger = logger;
        TaskQueue = workItemQueue;
    }

    public IWorkItemQueue TaskQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await BackgroundProcessing(stoppingToken);

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await TaskQueue.Dequeue(stoppingToken);

            try
            {
                if (workItem != null)
                {
                    await workItem(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queued Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}