using System.Collections.Immutable;
using Radiate.Client.Domain.Templates;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunUiState
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public IRunTemplate? RunTemplate { get; init; }
    public string TrainTest { get; init; } = TrainTestTypes.Train;
    public ImmutableDictionary<Guid, IPanel> Panels { get; init; } = ImmutableDictionary<Guid, IPanel>.Empty;
}
