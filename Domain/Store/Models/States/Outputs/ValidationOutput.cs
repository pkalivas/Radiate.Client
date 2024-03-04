using Radiate.Engines.Entities;

namespace Radiate.Client.Domain.Store.Models.States.Outputs;

public record ValidationOutput
{
    public string LossFunction { get; init; } = "";
    public BatchSetValidation TrainValidation { get; init; } = new();
    public BatchSetValidation TestValidation { get; init; } = new();
}
