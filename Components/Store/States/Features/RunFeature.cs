using Radiate.Schema;

namespace Radiate.Client.Components.Store.States.Features;

public record RunFeature
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public int Index { get; init; }
    public bool IsRunning { get; init; }
    public string Status { get; init; } = "";
    public RunInputsFeature Inputs { get; init; } = new();
    public RunOutputsFeature Outputs { get; init; } = new();
    public List<float> Scores { get; init; } = new();
    
    public override int GetHashCode()
    {
        return Hash.Of(typeof(RunFeature))
            .And(RunId)
            .And(IsRunning)
            .And(Status)
            .And(Inputs)
            .And(Outputs)
            .And(Scores)
            .Value;
    }
}