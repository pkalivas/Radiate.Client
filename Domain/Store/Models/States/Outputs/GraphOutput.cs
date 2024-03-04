using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Domain.Store.Models.States.Outputs;

public record GraphOutput
{
    public string Type { get; set; } = "";
    public object Graph { get; set; } = new();

    public PerceptronGraph<T>? GetGraph<T>()
    {
        if (Graph is PerceptronGraph<T> graph)
        {
            return graph;
        }

        return null;
    }
}
