namespace Radiate.Client.Domain.Store.Models.States.Outputs;

public record ImageOutput
{
    public ImageEntity Image { get; init; } = new();
}
