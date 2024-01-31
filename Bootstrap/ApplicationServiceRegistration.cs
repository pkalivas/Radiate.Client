using Radiate.Client.Components.Store;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Services.Runners;
using Radiate.Client.Services.Worker;
using AppState = Radiate.Client.Components.Store.States.AppState;

namespace Radiate.Client.Bootstrap;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddStore()
            .AddEngineRunners()
            .AddSingleton<IWorkItemQueue, WorkItemQueue>()
            .AddHostedService<BackgroundWorkerService>();


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

    private static IServiceCollection AddStore(this IServiceCollection services) =>
        services
            .AddSingleton<IStore, StateStore>(sp =>
            {
                var store = new StateStore();
                store.Register(new AppState());
                return store;
            })
            .AddScoped<AppState>(sp => sp.GetRequiredService<IStore>().GetFeature<AppState>());
}