namespace Radiate.Client.Components.Store.Interfaces;

public interface IState : IStateChangeNotifier
{
    string Name { get; }
    IState GetState();
    void SetState(IState state);
    public void Reduce(IReducer reducer, IAction action);
}

public interface IState<T> : IState
{
    T GetValue();
    event EventHandler<T>? SelectedValueChanged;
}
