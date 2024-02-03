using Radiate.Client.Components.Store.Effects;
using Radiate.Client.Components.Store.Reducers;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Runners;
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
                $"Graph_XOR" => sp.GetRequiredService<XORGraphRunner>(),
                "Image_Circle" => sp.GetRequiredService<CircleEngineRunner>(),
                "Image_Polygon" => sp.GetRequiredService<PolygonEngineRunner>(),
                _ => throw new ArgumentException($"Runner with name {name} not found.")
            });

    // private static IServiceCollection AddStore(this IServiceCollection services) =>
    //     services
    //         .AddSingleton<IDispatcher, Dispatcher>()
    //         .AddSingleton<IStore, StateStore>();
    
    private static IServiceCollection AddReflow(this IServiceCollection services) => 
        services
            .AddTransient<IEffect<RootState>, StartEngineEffect>()
            // .AddTransient<StartEngineEffect>()
            .AddTransient<IEffect<RootState>, RunOutputEffect>()
            .AddTransient<IEffect<RootState>, CancelEngineEffect>()
            .AddSingleton<Store<RootState>>(sp =>
            {
                var store = new Store<RootState>(RootReducer.CreateReducers(), new RootState());
                
                var serviceProvidedEffects = sp.GetServices<IEffect<RootState>>();
                
                store.RegisterEffects(serviceProvidedEffects.ToArray());
                
                return store;
            });
}