namespace Radiate.Client.Domain.Store.Models;

public interface ITreeItem<out TKey>
{
    public TKey Index { get; }
    public IEnumerable<TKey> Children { get; }
    public bool IsCyclic();
}

public class TreeItemData<T, TKey> where T : ITreeItem<TKey>
{
    public T Data { get; init; }
    public HashSet<TreeItemData<T, TKey>> TreeItems { get; init; } = new();
    public bool IsExpanded { get; set; } = false;

    public TreeItemData(T data, bool isExpanded = false)
    {
        Data = data;
        IsExpanded = isExpanded;
    }
    
    public bool HasChild => TreeItems.Count > 0;
}