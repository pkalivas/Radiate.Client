using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Queue<(object Action, Type ActionType)> _actionQueue = new();
    
    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void Dispatch<TAction, TState>(TAction action)
        where TAction : IAction<TState>
        where TState : IState<TState>
    {
        using var scope = _serviceProvider.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IStore>();
        
        var state = store.GetFeature<TState>();
        
        
        
        throw new NotImplementedException();
    }
}