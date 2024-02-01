namespace Radiate.Client.Components.Store.Interfaces;

public interface IFeatureSelection<out TFeature, TSelected> : IFeature<TSelected>
    where TFeature : IFeature<TFeature>
    where TSelected : IFeatureSelection<TFeature, TSelected>
{
    event EventHandler<TSelected> OnChange; 
    void Select(Func<TFeature, TSelected> selector, Action<TSelected> callback);
}