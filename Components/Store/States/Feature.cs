using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.States;

public abstract record Feature<TState> : IFeature<TState> 
    where TState : IFeature<TState>
{
    
}
