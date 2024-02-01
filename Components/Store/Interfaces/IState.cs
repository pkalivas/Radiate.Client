using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Interfaces;

public interface IState : IStateChangeNotifier
{
    int ChangeCount { get; }
    string Name { get; }
    IState GetState();
    void SetState(IState state);
    void Reduce(IReducer reducer, IAction action);
}

public interface IState<T> : IState where T : ICopy<T>
{
    T GetValue();
    IState<TK> SelectState<TK>(Func<T, TK> selector) where TK : ICopy<TK>;
    event EventHandler<T>? SelectedValueChanged;
}
