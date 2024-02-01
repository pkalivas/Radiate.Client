namespace Radiate.Client.Components.Store.Interfaces;

public interface IFeature : IState
{
    void SetState(IState state);
}

public interface IFeature<out TFeature> : IFeature, IState<TFeature>
    where TFeature : IFeature<TFeature>
{
}
