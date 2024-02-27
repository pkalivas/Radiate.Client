using Akka.Actor;
using Radiate.Client.Services.Actors.Commands;
using Radiate.Client.Services.Runners;
using Radiate.Client.Services.Worker;

namespace Radiate.Client.Services.Actors;

public class RunsActor : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    
    private CancellationTokenSource _cancellationTokenSource = new();
    
    public RunsActor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        ReceiveAsync<RunsActorMessage<StopRunCommand>>(Handle);
        ReceiveAsync<RunsActorMessage<StartRunCommand>>(Handle);
    }
    
    private async Task Handle(RunsActorMessage<StopRunCommand> message) => await _cancellationTokenSource.CancelAsync();
    
    private async Task Handle(RunsActorMessage<StartRunCommand> message)
    {
        await using var outerScope = _serviceProvider.CreateAsyncScope();
        var workItemQueue = outerScope.ServiceProvider.GetRequiredService<IWorkItemQueue>();
        
        if (_cancellationTokenSource.Token.IsCancellationRequested)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        var (runId, inputs) = message.Message;
        
        workItemQueue.Enqueue(async token => await Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var builderFactory = scope.ServiceProvider.GetRequiredService<EngineRunnerFactory>();
            
            var builder = builderFactory(inputs.ModelType, inputs.DataSetType);

            await builder.Run(runId, inputs, _cancellationTokenSource);
        }, token));
    }
}