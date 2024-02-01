using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Interfaces;

public interface IStateSelection<out TFeature, TSelected> : IDisposable
{
    event EventHandler<TSelected> SelectedValueChanged;
    IState<T> Select<T>(Func<TSelected, T> selector) where T : ICopy<T>;
}