using Radiate.Client.Domain.Store.Models.States;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record InputsPanelModelProjection
{
    public Guid RunId { get; set; } = Guid.Empty;
    public bool IsReadonly { get; set; } = false;
    public RunInputsState Inputs { get; set; } = new();
}