using Radiate.Engines.Entities;
using Radiate.Extensions.Evolution.Architects.Groups;

namespace Radiate.Client.Services.Store.Models.States;

public record RunOutputsState
{
    public string EngineState { get; set; } = "";
    public string EngineId { get; set; } = "";
    public MetricSet Metrics { get; init; } = new();
    public Dictionary<string, EngineState> EngineStates { get; init; } = new();
    public ImageOutput ImageOutput { get; set; } = new();
    public GraphOutput GraphOutput { get; set; } = new();
    public TreeOutput TreeOutput { get; set; } = new();
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
    public string Type { get; set; } = "";
    public List<object> Trees { get; set; } = new();

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
    public ImageEntity Image { get; set; } = new();
}
