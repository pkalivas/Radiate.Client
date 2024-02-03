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
            .AddScoped<EngineRunnerFactory>(sp => name => name switch
            {
                "Graph_XOR" => sp.GetRequiredService<XORGraphRunner>(),
                "Image_Circle" => sp.GetRequiredService<CircleEngineRunner>(),
                "Image_Polygon" => sp.GetRequiredService<PolygonEngineRunner>(),
                _ => throw new ArgumentException($"Runner with name {name} not found.")
            });
    
    private static IServiceCollection AddReflow(this IServiceCollection services) => 
        services
            .AddTransient<IEffectRegistry<RootState>, RootEffects>()
            .AddSingleton<Store<RootState>>(sp =>
            {
                var store = new Store<RootState>(RootReducer.CreateReducers(), new RootState());
                var serviceProvidedEffects = sp.GetService<IEffectRegistry<RootState>>();

                if (serviceProvidedEffects != null)
                {
                    store.RegisterEffects(serviceProvidedEffects.CreateEffects().ToArray());
                }
                
                return store;
            });
}