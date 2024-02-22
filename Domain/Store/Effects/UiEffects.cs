using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Effects;

public class UiEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
        };
}