namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore : IDisposable, IActionSubscriber
{
    Task Dispatch<TAction, TState>(TAction action)
        where TAction : IAction<TState>
        where TState : IFeature<TState>, IState<TState>;
    
    void Register<TState>(TState state) where TState : IFeature<TState>;

    State<TState> Select<TState>() where TState : IState<TState>;
}