using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Effects;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Runners;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Client.Services.Schema;
using Radiate.Client.Services.Worker;
using Reflow;
using Reflow.Interfaces;

namespace Radiate.Client.Bootstrap;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddReflow()
            .AddEngineRunners()
            .AddSingleton<IActorService, ActorService>()
            .AddSingleton<IWorkItemQueue, WorkItemQueue>()
            .AddHostedService<BackgroundWorkerService>()
            .AddHostedService(sp => (ActorService)sp.GetRequiredService<IActorService>());

    private static IServiceCollection AddEngineRunners(this IServiceCollection services) =>
        services
            .AddScoped<XORGraphRunner>()
            .AddScoped<CircleEngineRunner>()
            .AddScoped<PolygonEngineRunner>()
            .AddScoped<GraphRegressionRunner>()
            .AddScoped<TreeRegressionRunner>()
            .AddScoped<MultiObjectiveRunner>()
            .AddScoped<EngineRunnerFactory>(sp => (model, data) => (model, data) switch
            {
                (ModelTypes.Tree, DataSetTypes.Regression) => sp.GetRequiredService<TreeRegressionRunner>(),
                (ModelTypes.Graph, DataSetTypes.Regression) => sp.GetRequiredService<GraphRegressionRunner>(),
                (ModelTypes.Graph, DataSetTypes.Xor) => sp.GetRequiredService<XORGraphRunner>(),
                (ModelTypes.Image, DataSetTypes.Circle) => sp.GetRequiredService<CircleEngineRunner>(),
                (ModelTypes.Image, DataSetTypes.Polygon) => sp.GetRequiredService<PolygonEngineRunner>(),
                (ModelTypes.MultiObjective, _) => sp.GetRequiredService<MultiObjectiveRunner>(),
                _ => throw new ArgumentException($"Runner {model} {data}.")
            });
    
    private static IServiceCollection AddReflow(this IServiceCollection services) => 
        services
            .AddTransient<IEffectRegistry<RootState>, RunEffects>()
            .AddTransient<IEffectRegistry<RootState>, RunUiEffects>()
            .AddTransient<IEffectRegistry<RootState>, GlobalEffects>()
            .AddTransient<IEffectRegistry<RootState>, UiEffects>()
            .AddSingleton<IStore<RootState>, Store<RootState>>(sp =>
            {
                var store = new Store<RootState>(RootReducer.CreateReducers(), new RootState());
                var serviceProvidedEffects = sp.GetServices<IEffectRegistry<RootState>>()
                    .SelectMany(val => val.CreateEffects())
                    .ToArray();

                if (serviceProvidedEffects.Length != 0)
                {
                    store.Register(serviceProvidedEffects);
                }
                
                return store;
            });
}