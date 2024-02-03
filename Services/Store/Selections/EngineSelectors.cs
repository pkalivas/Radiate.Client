using MudBlazor;
using Radiate.Client.Services.Store.Models;
using Radiate.Engines.Entities;
using Radiate.Engines.Schema;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class EngineSelectors
{
    public static readonly ISelector<RootState, EngineControlModel> SelectEngineControl = Selectors
        .Create<RootState, RunModel, EngineControlModel>(RunSelectors.SelectRun, run => new EngineControlModel
        {
            RunId = run.RunId,
            IsRunning = run.IsRunning,
            IsPaused = run.IsPaused,
            IsCompleted = run.IsCompleted
        });
    
    public static readonly ISelector<RootState, EngineModel> SelectEngineModel = Selectors
        .Create<RootState, UiModel, RunModel, EngineModel>(UiSelectors.SelectUiState, RunSelectors.SelectRun,
            (ui, run) =>
            {
                if (ui.EngineStateExpanded.TryGetValue(run.RunId, out var engineTree))
                {
                    return new EngineModel
                    {
                        RunId = run.RunId,
                        TreeItems = GetItems(run.Outputs.EngineStates, engineTree),
                        Expanded = engineTree,
                        Inputs = run.Inputs,
                        CurrentEngineState = run?.Outputs?.EngineStates.FirstOrDefault().Value
                    };
                }
                
                return new EngineModel
                {
                    RunId = run.RunId,
                    TreeItems = GetItems(run.Outputs.EngineStates, new Dictionary<string, bool>()),
                    Expanded = new Dictionary<string, bool>(),
                    Inputs = run.Inputs,
                    CurrentEngineState = run?.Outputs?.EngineStates.FirstOrDefault().Value
                };
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
        
        currentTreeItem.IsExpanded = expanded.GetValueOrDefault(current, false);
        
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
}