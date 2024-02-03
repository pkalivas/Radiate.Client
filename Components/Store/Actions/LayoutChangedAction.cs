namespace Radiate.Client.Components.Store.Actions;

public record LayoutChangedAction 
{
    public bool IsSidebarOpen { get; init; }
}
