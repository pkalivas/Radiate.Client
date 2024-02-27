using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Effects;
using Radiate.Client.Services;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Runners;
using Radiate.Client.Services.Runners.Image;
using Radiate.Client.Services.Runners.MultiObjective;
using Radiate.Client.Services.Runners.Regression;
using Radiate.Client.Services.Runners.SinWave;
using Radiate.Client.Services.Runners.XOR;
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
            .AddTransient<InputsService>()
            .AddSingleton<IValidationService, ValidationService>()
            .AddSingleton<ITensorFrameService, TensorFrameService>()
            .AddSingleton<IActorService, ActorService>()
            .AddSingleton<IWorkItemQueue, WorkItemQueue>()
            .AddHostedService<BackgroundWorkerService>()
            .AddHostedService(sp => (ActorService)sp.GetRequiredService<IActorService>());

    private static IServiceCollection AddEngineRunners(this IServiceCollection services) =>
        services
            .AddScoped<XorGraphBuilder>()
            .AddScoped<GraphRegressionBuilder>()
            .AddScoped<TreeRegressionBuilder>()
            .AddScoped<GraphSinWaveBuilder>()
            .AddScoped<MultiObjectiveDTLZBuilder>()
            .AddScoped<PolygonBuilder>()
            .AddScoped<CircleBuilder>()
            .AddScoped<EngineRunnerFactory>(sp => (model, data) => model switch
            {
                ModelTypes.Graph => data switch
                {
                    DataSetTypes.Regression => sp.GetRequiredService<GraphRegressionBuilder>(),
                    DataSetTypes.Xor => sp.GetRequiredService<XorGraphBuilder>(),
                    DataSetTypes.SinWave => sp.GetRequiredService<GraphSinWaveBuilder>(),
                    _ => throw new ArgumentException($"Runner {model} {data}.")
                },
                ModelTypes.Tree => data switch
                {
                    DataSetTypes.Regression => sp.GetRequiredService<TreeRegressionBuilder>(),
                    _ => throw new ArgumentException($"Runner {model} {data}.")
                },
                ModelTypes.MultiObjective => data switch
                {
                    _ => sp.GetRequiredService<MultiObjectiveDTLZBuilder>(),
                },
                ModelTypes.Image => data switch
                {
                    DataSetTypes.Polygon => sp.GetRequiredService<PolygonBuilder>(),
                    DataSetTypes.Circle => sp.GetRequiredService<CircleBuilder>(),
                },
                _ => throw new ArgumentException($"Runner {model} {data}.")
            });
    
    private static IServiceCollection AddReflow(this IServiceCollection services) => 
        services
            .AddTransient<IEffectRegistry<RootState>, RunEffects>()
            .AddTransient<IEffectRegistry<RootState>, RunUiEffects>()
            .AddTransient<IEffectRegistry<RootState>, GlobalEffects>()
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