using System.Collections.Immutable;
using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record StandardRunUiProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsLoading { get; init; } = true;
    public IRunUITemplate? UiTemplate { get; init; } = default!;
    public IImmutableDictionary<Guid, bool> IsExpanded { get; init; } = ImmutableDictionary<Guid, bool>.Empty;
}