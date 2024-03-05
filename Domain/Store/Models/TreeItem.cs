namespace Radiate.Client.Domain.Store.Models;

public interface ITreeItem<out TKey>
{
    public TKey Key { get; }
    public IEnumerable<TKey> Children { get; }
    public bool IsCyclic();
}

public class TreeItem<T, TKey> where T : ITreeItem<TKey>
{
    public T Data { get; init; }
    public HashSet<TreeItem<T, TKey>> ChildItems { get; init; } = new();
    public bool IsExpanded { get; set; }

    public TreeItem(T data, bool isExpanded = false)
    {
        Data = data;
        IsExpanded = isExpanded;
    }
}