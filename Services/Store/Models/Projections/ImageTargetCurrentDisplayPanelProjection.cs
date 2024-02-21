namespace Radiate.Client.Services.Store.Models.Projections;

public record ImageTargetCurrentDisplayPanelProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsReadonly { get; set; } = true;
    public ImageEntity TargetImage { get; set; } = new();
    public ImageEntity CurrentImage { get; set; } = new();
}