using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store;

public record RootState : IState
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public UiState UiState { get; init; } = new();
    
    public IImmutableDictionary<Guid, RunUiState> RunUis { get; private init; } =
        new Dictionary<Guid, RunUiState>().ToImmutableDictionary();

    public IImmutableDictionary<Guid, RunState> Runs { get; private init; } =
        new Dictionary<Guid, RunState>().ToImmutableDictionary();

    public RootState UpdateRun(Guid runId, Func<RunState, RunState> update)
    {
        var currentRun = Runs.TryGetValue(runId, out var run) ? run : new();
        var newRuns = Runs.ToDictionary(); 
        newRuns[runId] = update(currentRun);
        return this with { Runs = newRuns.ToImmutableDictionary() };
    }
    
    public RootState UpdateRunUi(Guid runId, Func<RunUiState, RunUiState> update)
    {
        var currentRunUi = RunUis.TryGetValue(runId, out var runUi) ? runUi : new();
        var newRunUis = RunUis.ToDictionary(); 
        newRunUis[runId] = update(currentRunUi);
        return this with { RunUis = newRunUis.ToImmutableDictionary() };
    }
    
    public RootState UpdateUi(Func<UiState, UiState> update) => this with { UiState = update(UiState) };
}
