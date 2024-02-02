using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.States;


public record RunInputState : ICopy<RunInputState>
{
    public string ModelType { get; set; } = "Graph";
    public string DataSetType { get; set; } = "XOR";
    public int PopulationSize { get; set; } = 50;
    public float MutationRate { get; set; } = 0.02f;
    public int IterationLimit { get; set; } = 1000;
    public int NumShapes { get; set; } = 150;
    public int NumVertices { get; set; } = 4;
    
    public RunInputState Copy() => new()
    {
        ModelType = ModelType,
        DataSetType = DataSetType,
        PopulationSize = PopulationSize,
        MutationRate = MutationRate,
        IterationLimit = IterationLimit,
        NumShapes = NumShapes,
        NumVertices = NumVertices
    };
}