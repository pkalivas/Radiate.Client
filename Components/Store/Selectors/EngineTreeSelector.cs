using MudBlazor;
using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.Models;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Engines.Entities;
using Radiate.Engines.Schema;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Selectors;

public record EngineTreeState : ICopy<EngineTreeState>
{
    public HashSet<TreeItemData<EngineState>> TreeItems { get; init; } = new();
    
    public EngineTreeState Copy() => new()
    {
        TreeItems = TreeItems.ToHashSet()
    };
}

public static class EngineTreeSelector
{
    public static IState<EngineTreeState> Select(StateStore store) => 
        store.Select<RootFeature>()
            .SelectState(state =>
            {
                if (state.UiState.EngineStateExpanded.TryGetValue(state.CurrentRunId, out var engineTree))
                {
                    return new EngineTreeState
                    {
                        TreeItems = GetItems(state.Runs[state.CurrentRunId].Outputs.EngineStates, engineTree)
                    };
                }
                
                if (state.Runs.TryGetValue(state.CurrentRunId, out var run))
                {
                    return new EngineTreeState
                    {
                        TreeItems = GetItems(run.Outputs.EngineStates, new Dictionary<string, bool>())
                    };
                }

                return new EngineTreeState();
            });
    
    private static HashSet<TreeItemData<EngineState>> GetItems(Dictionary<string, EngineState> states, Dictionary<string, bool> expanded)
    {
        var seen = new HashSet<string>();
        var result = new HashSet<TreeItemData<EngineState>>();

        foreach (var state in states.Values)
        {
            if (seen.Contains(state.EngineId))
            {
                continue;
            }
            
            foreach (var item in GetTreeItems(state.EngineId, seen, states, expanded))
            {
                result.Add(item);
            }
        }
        
        return result;
    }
    
    private static HashSet<TreeItemData<EngineState>> GetTreeItems(string current, 
        HashSet<string> seen, 
        Dictionary<string, EngineState> states,
        Dictionary<string, bool> expanded)
    {
        seen.Add(current);
        
        var currentEngineState = states[current];
        var currentTreeItem = new TreeItemData<EngineState>(GetIcon(currentEngineState), GetColor(currentEngineState), currentEngineState);
        
        if (expanded.TryGetValue(current, out var isExpanded))
        {
            currentTreeItem.IsExpanded = isExpanded;
        }
        else
        {
            currentTreeItem.IsExpanded = false;
        }
        
        foreach (var sub in states[current].SubEngines)
        {
            foreach (var item in GetTreeItems(sub, seen, states, expanded))
            {
                currentTreeItem.TreeItems.Add(item);
            }
        }
        
        return new List<TreeItemData<EngineState>> { currentTreeItem }.ToHashSet();
    }
    
    private static string GetIcon(EngineState state) => state.State switch
    {
        EngineStateTypes.Pending => Icons.Material.Filled.Pending,
        EngineStateTypes.Started => Icons.Material.Filled.Start,
        EngineStateTypes.Running => Icons.Material.Filled.RunCircle,
        EngineStateTypes.Stopped => Icons.Material.Filled.Stop,
        _ => Icons.Custom.FileFormats.FileCode
    };

    private static Color GetColor(EngineState state) => state.State switch
    {
        EngineStateTypes.Pending => Color.Default,
        EngineStateTypes.Started => Color.Primary,
        EngineStateTypes.Running => Color.Success,
        EngineStateTypes.Stopped => Color.Secondary,
        _ => Color.Default
    };
    
    private static bool HasSwitched(EngineState state) => state.Metrics.Get(MetricNames.EngineChange)?.Statistics?.Sum > 0;

}