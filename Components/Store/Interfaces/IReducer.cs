namespace Radiate.Client.Components.Store.Interfaces;

public interface IReducer
{
    IFeature Reduce(IFeature feature, IAction action);
}

public interface IReducer<TState>
{
    TState Reduce(TState state, IAction action);
}