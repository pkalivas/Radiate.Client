using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore : IDisposable, IActionSubscriber
{
    event Action ActionsProcessed;
    void Selctors<T>(Func<StateStore, IState<T>> selector) where T : ICopy<T>;
    void Register<TState>(TState state) where TState : IFeature<TState>;
    List<IState> GetStates();

    IState<TState> Select<TState>() where TState : ICopy<TState>;
}