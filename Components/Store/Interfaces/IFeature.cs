namespace Radiate.Client.Components.Store.Interfaces;

public interface IFeature { }

public interface IFeature<TFeature> : IFeature
    where TFeature : IFeature<TFeature>, IFeature
{
    
}
