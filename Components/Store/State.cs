using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class State<TState> : StateSelection<TState, TState>
    where TState : IState<TState>
{
    public State(IState<TState> state) : base(state, val => val)
    {
    }
    
    public override int GetHashCode() => State.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is State<TState> state)
        {
            return ReferenceEquals(state, State);
        }

        return false;
    }
}