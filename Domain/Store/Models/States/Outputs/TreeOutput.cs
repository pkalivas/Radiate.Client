using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Domain.Store.Models.States.Outputs;

public record TreeOutput
{
    public string Type { get; init; } = "";
    public object Tree { get; init; } = new();

    public ExpressionTree<T>? GetTrees<T>() 
    {
        if (Tree is ExpressionTree<T> tree)
        {
            return tree;
        }

        return null;
    }
}