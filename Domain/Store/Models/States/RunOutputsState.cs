using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Engines.Entities;
using Radiate.Engines.Harness;
using Radiate.Extensions.Evolution.Architects.Groups;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunOutputsState
{
    public string EngineState { get; init; } = "";
    public string EngineId { get; init; } = "";
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

    public Graph<T> GetGraph<T>()
    {
        if (Graph is Graph<T> graph)
        {
            return graph;
        }
        
        throw new Exception("Graph is not of the expected type");
    }
}

public record TreeOutput
{
    public string Type { get; init; } = "";
    public IImmutableList<object> Trees { get; init; } = ImmutableList<object>.Empty;

    public List<Tree<T>> GetTrees<T>() => Trees
        .Select(tree =>
        {
            if (tree is Tree<T> t)
            {
                return t;
            }
            
            throw new Exception("Tree is not of the expected type");
        })
        .ToList();
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