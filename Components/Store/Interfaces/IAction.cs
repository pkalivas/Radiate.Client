namespace Radiate.Client.Components.Store.Interfaces;

public interface IAction<in TState> where TState : IState<TState>
{
    string StateName { get; }
    Task Reduce(TState feature);
}

public interface IStateAction { }

public record TestAction : IStateAction
{
    public string StateName => "Test";
}