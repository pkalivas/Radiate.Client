using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Interfaces;

public interface IFeature<out TFeature> : ICopy<TFeature>
{
}
