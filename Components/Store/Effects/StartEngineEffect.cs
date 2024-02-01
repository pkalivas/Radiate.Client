using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Client.Services.Runners;
using Radiate.Client.Services.Worker;

namespace Radiate.Client.Components.Store.Effects;

public class StartEngineEffect : Effect<RootFeature, StartEngineAction>
{
    public StartEngineEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }
    
    public override async Task HandleAsync(RootFeature feature, StartEngineAction action, IDispatcher dispatcher)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var engineRunnerFactory = scope.ServiceProvider.GetRequiredService<EngineRunnerFactory>();
        var workItemQueue = scope.ServiceProvider.GetRequiredService<IWorkItemQueue>();

        var currentRun = feature.Runs[feature.CurrentRunId];
        
        var runner = engineRunnerFactory($"{currentRun.Inputs.ModelType}_{currentRun.Inputs.DataSetType}");
        var inputs = runner.GetInputs(currentRun.Inputs);
        
        workItemQueue.Enqueue(runner.Run(inputs, new CancellationTokenSource()));
    }
}