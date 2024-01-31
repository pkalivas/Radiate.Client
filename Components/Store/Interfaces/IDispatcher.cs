namespace Radiate.Client.Components.Store.Interfaces;

public interface IDispatcher
{
    void Dispatch(IStateAction stateAction);
}