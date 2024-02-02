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
        
        ReceiveAsync<RunsActorMessage<StartRunCommand>>(Handle);
        ReceiveAsync<RunsActorMessage<StopRunCommand>>(Handle);
    }
    
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
        
        var runner = runFactory($"{message.Message.Inputs.ModelType}_{message.Message.Inputs.DataSetType}");
        var inputs = runner.GetInputs(message.Message.Inputs);
        
        workItemQueue.Enqueue(runner.Run(inputs, _cancellationTokenSource));
    }
    
    private async Task Handle(RunsActorMessage<StopRunCommand> message)
    {
        await _cancellationTokenSource.CancelAsync();
    }
}