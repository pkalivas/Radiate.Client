using Akka.Actor;
using Radiate.Client.Services.Actors.Commands;
using Radiate.Client.Services.Runners.Builders.XOR;
using Radiate.Client.Services.Runners.Interfaces;
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
        await using var scope = _serviceProvider.CreateAsyncScope();
        var workItemQueue = scope.ServiceProvider.GetRequiredService<IWorkItemQueue>();
        var runFactory = scope.ServiceProvider.GetRequiredService<EngineRunnerFactory>();
        
        if (_cancellationTokenSource.Token.IsCancellationRequested)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
        
        var inputs = message.Message.Inputs;
        var runner = runFactory(inputs.ModelType, inputs.DataSetType);
        
        workItemQueue.Enqueue(async token =>
        {
            await Task.Run(async () =>
            {
                await using var scope = _serviceProvider.CreateAsyncScope();

                var builder = scope.ServiceProvider.GetRequiredService<XorGraphBuilder>();

                // await runner.StartRun(message.RunId, message.Message.Inputs, _cancellationTokenSource);
                await builder.Run(message.RunId, message.Message.Inputs, _cancellationTokenSource);
            }, token);
        });
    }
}