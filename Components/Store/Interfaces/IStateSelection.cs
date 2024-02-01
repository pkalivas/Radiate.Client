namespace Radiate.Client.Components.Store.Interfaces;

public interface IStateSelection<out TFeature, TSelected> : IState<TSelected>, IDisposable
    where TFeature : IState<TFeature>
    where TSelected : IState<TSelected>
{
    event EventHandler<TSelected> SelectedValueChanged;
    T Select<T>(Func<TSelected, T> selector)
        where T : IState<T>;
}