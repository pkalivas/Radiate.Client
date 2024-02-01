using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Services.Runners;
using Radiate.Client.Services.Worker;

namespace Radiate.Client.Components.Store.Effects;

public class StartEngineEffect : RootEffect<AppFeature, StartEngineAction>
{
    public StartEngineEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }
    
    public override async Task HandleAsync(AppFeature feature, StartEngineAction action, IDispatcher dispatcher)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var engineRunnerFactory = scope.ServiceProvider.GetRequiredService<EngineRunnerFactory>();
        var workItemQueue = scope.ServiceProvider.GetRequiredService<IWorkItemQueue>();
        
        var runner = engineRunnerFactory($"{feature.ModelType}_{feature.DataSetType}");
        var inputs = runner.GetInputs(feature);
        
        workItemQueue.Enqueue(runner.Run(inputs, feature.CancellationTokenSource));
    }
}