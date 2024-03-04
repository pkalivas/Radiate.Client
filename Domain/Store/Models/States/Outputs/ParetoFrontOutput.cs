using System.Collections.Immutable;

namespace Radiate.Client.Domain.Store.Models.States.Outputs;

public record ParetoFrontOutput
{
    public IImmutableList<float[]> Front { get; init; } = ImmutableList<float[]>.Empty;
}