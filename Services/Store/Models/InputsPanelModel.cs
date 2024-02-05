namespace Radiate.Client.Services.Store.Models;

public record InputsPanelModel
{
    public Guid RunId { get; set; } = Guid.Empty;
    public RunInputsModel Inputs { get; set; } = new();
}