namespace Radiate.Client.Components.Store.Interfaces;

public interface IAction<in TState> where TState : IState<TState>
{
    string StateName { get; }
    Task Reduce(TState feature);
}
