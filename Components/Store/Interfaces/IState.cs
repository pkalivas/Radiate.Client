namespace Radiate.Client.Components.Store.Interfaces;

public interface IState : IStateChangeNotifier
{
    int ChangeCount { get; }
    string Name { get; }
    IState GetState();
    void SetState(IState state);
    public void Reduce(IReducer reducer, IAction action);
}

public interface IState<T> : IState
{
    T GetValue();
    IState<TK> SelectState<TK>(Func<T, TK> selector);
    event EventHandler<T>? SelectedValueChanged;
}
