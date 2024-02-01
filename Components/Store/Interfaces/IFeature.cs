namespace Radiate.Client.Components.Store.Interfaces;

public interface IFeature
{
    string Name { get; }
    IState GetState();
    Type GetStateType();
    void SetState(IState state);
}

public interface IFeature<out TFeature> : IFeature
    where TFeature : IState<TFeature>
{
    TFeature State { get; }
}
