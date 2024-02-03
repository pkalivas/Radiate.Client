using MudBlazor;

namespace Radiate.Client.Services.Store.Models;

public class TreeItemData<T>
{
    public string Icon { get; set; }
    public Color Color { get; set; }
    public T Data { get; set; }

    public bool IsExpanded { get; set; } = false;
    
    public HashSet<TreeItemData<T>> TreeItems { get; set; } = new();

    public TreeItemData(string icon, Color color, T data)
    {
        Icon = icon;
        Color = color;
        Data = data;
    }
    
    public bool HasChild => TreeItems.Count > 0;
}