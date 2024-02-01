using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Subscribers;

public class Subscription
{
    public Type? SubsciberType { get; set; }
    public Type? ActionType { get; set; }
    public Action<IAction> Callback { get; set; }
}