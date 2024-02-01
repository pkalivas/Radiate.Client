namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore : IDisposable, IActionSubscriber
{
    void Selctors<T>(Func<StateStore, IState<T>> selector);
    void Register<TState>(TState state) where TState : IFeature<TState>;
    List<IState> GetStates();

    IState<TState> Select<TState>();
}