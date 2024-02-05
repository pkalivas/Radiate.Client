using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Runners;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Client.Services.Store;
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
            .AddScoped<EngineRunnerFactory>(sp => (model, data) => (model, data) switch
            {
                ("Graph", "Regression") => sp.GetRequiredService<GraphRegressionRunner>(),
                ("Graph", "XOR") => sp.GetRequiredService<XORGraphRunner>(),
                ("Image", "Circle") => sp.GetRequiredService<CircleEngineRunner>(),
                ("Image", "Polygon") => sp.GetRequiredService<PolygonEngineRunner>(),
                _ => throw new ArgumentException($"Runner {model} {data}.")
            });
    
    private static IServiceCollection AddReflow(this IServiceCollection services) => 
        services
            .AddTransient<IEffectRegistry<RootState>, RootEffects>()
            .AddSingleton<IStore<RootState>, Store<RootState>>(sp =>
            {
                var store = new Store<RootState>(RootReducer.CreateReducers(), new RootState());
                var serviceProvidedEffects = sp.GetService<IEffectRegistry<RootState>>();

                if (serviceProvidedEffects != null)
                {
                    store.Register(serviceProvidedEffects.CreateEffects().ToArray());
                }
                
                return store;
            });
}