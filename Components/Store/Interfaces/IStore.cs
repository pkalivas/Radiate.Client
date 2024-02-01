namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore : IDisposable, IActionSubscriber
{
    void Register<TState>(TState state) where TState : IFeature<TState>;

    IState<TState> Select<TState>();
}