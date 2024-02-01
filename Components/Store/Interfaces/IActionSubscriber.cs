namespace Radiate.Client.Components.Store.Interfaces;

public interface IActionSubscriber
{
    void Notify(IAction action);
    void Subscribe<TAction>(object subscriber, Action<TAction> callback) where TAction : IAction;
    void Unsubscribe<TAction>(object subscriber) where TAction : IAction;
    void UnsubscribeAll(object subscriber);
}