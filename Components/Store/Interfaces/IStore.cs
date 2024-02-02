using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore : IDisposable, IActionSubscriber
{
    event Action ActionsProcessed;
    void RegisterSelector<T>(Func<StateStore, IState<T>> selector) where T : ICopy<T>;
    void RegisterFeature<TState>(TState state) where TState : IFeature<TState>;
    List<IState> GetStates();

    IState<TState> GetState<TState>() where TState : ICopy<TState>;
}