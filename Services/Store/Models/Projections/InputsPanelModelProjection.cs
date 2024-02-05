namespace Radiate.Client.Services.Store.Models.Projections;

public record InputsPanelModelProjection
{
    public Guid RunId { get; set; } = Guid.Empty;
    public bool IsReadonly { get; set; } = false;
    public RunInputsModel Inputs { get; set; } = new();
}