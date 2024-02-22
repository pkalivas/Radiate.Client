using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Engines.Entities;
using Radiate.Engines.Harness;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunOutputsState
{
    public string EngineState { get; init; } = "";
    public string EngineId { get; init; } = "";
    public string ModelType { get; init; } = "";
    public IImmutableDictionary<string, MetricValueModel> Metrics { get; init; } = ImmutableDictionary<string, MetricValueModel>.Empty;
    public IImmutableDictionary<string, EngineState> EngineStates { get; init; } = ImmutableDictionary<string, EngineState>.Empty;
    public ImageOutput ImageOutput { get; init; } = new();
    public GraphOutput GraphOutput { get; init; } = new();
    public TreeOutput TreeOutput { get; init; } = new();
    public ValidationOutput ValidationOutput { get; init; } = new();
    public ParetoFrontOutput ParetoFrontOutput { get; init; } = new();
}

public record GraphOutput
{
    public string Type { get; set; } = "";
    public object Graph { get; set; } = new();

    public PerceptronGraph<T> GetGraph<T>()
    {
        if (Graph is PerceptronGraph<T> graph)
        {
            return graph;
        }
        
        throw new Exception("Graph is not of the expected type");
    }
}

public record TreeOutput
{
    public string Type { get; init; } = "";
    public object Tree { get; init; } = new();

    public ExpressionTree<T> GetTrees<T>() 
    {
        if (Tree is ExpressionTree<T> tree)
        {
            return tree;
        }
        
        throw new Exception("Graph is not of the expected type");
    }
}

public record ImageOutput
{
    public ImageEntity Image { get; init; } = new();
}

public record ValidationOutput
{
    public string LossFunction { get; init; } = "";
    public BatchSetValidation TrainValidation { get; init; } = new();
    public BatchSetValidation TestValidation { get; init; } = new();
}

public record ParetoFrontOutput
{
    public IImmutableList<float[]> Front { get; init; } = ImmutableList<float[]>.Empty;
}