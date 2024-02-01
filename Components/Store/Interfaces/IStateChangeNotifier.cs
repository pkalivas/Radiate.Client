namespace Radiate.Client.Components.Store.Interfaces;

public interface IStateChangeNotifier
{
    event EventHandler StateChanged;
}