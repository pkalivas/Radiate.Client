using System.Text.Json.Serialization;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunInputsState 
{
    public string ModelType { get; set; } = "Graph";
    public string DataSetType { get; set; } = "XOR";
    public GraphInputs GraphInputs { get; init; } = new();
    public TreeInputs TreeInputs { get; init; } = new();
    public PopulationInputs PopulationInputs { get; init; } = new();
    public LimitInputs LimitInputs { get; init; } = new();
    public ImageInputs ImageInputs { get; init; } = new();
    public MultiObjectiveInputs MultiObjectiveInputs { get; init; } = new();
}

public record PopulationInputs
{
    public int PopulationSize { get; set; } = 50;
    public float MutationRate { get; set; } = 0.02f;
    public float CrossoverRate { get; set; } = 0.7f;
}

public record GraphInputs 
{
    public float AddGateRate { get; set; } = 0.05f;
    public float AddLinkRate { get; set; } = 0.05f;
    public float AddWeightRate { get; set; } = 0.05f;
    public float AddMemoryRate { get; set; } = 0.01f;
}

public record TreeInputs 
{
    public int MaxDepth { get; set; } = 5;
}

public record LimitInputs
{
    public int IterationLimit { get; set; } = 1000;
}

public record ImageInputs
{
    public int Height { get; set; } = 50;
    public int Width { get; set; } = 50;
    public int NumShapes { get; set; } = 150;
    public int NumVertices { get; set; } = 4;
    [JsonIgnore] public ImageEntity TargetImage { get; set; } = new();
}

public record MultiObjectiveInputs
{
    public int FrontMinSize { get; set; } = 1000;
    public int FrontMaxSize { get; set; } = 1100;
}
