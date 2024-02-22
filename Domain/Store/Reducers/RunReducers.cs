using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Engines.Schema;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store.Reducers;

public static class RunReducers
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
    [
        Reducer.On<RunCreatedAction, RootState>(AddRun),
        Reducer.On<SetRunOutputsAction, RootState>(AddOutput),
        Reducer.On<SetRunInputsAction, RootState>(SetRunInputs),
        Reducer.On<CopyRunAction, RootState>(CopyRun),
        Reducer.On<EngineStoppedAction, RootState>(RunCompleted),
        Reducer.On<StartEngineAction, RootState>(StartEngine),
        Reducer.On<PauseEngineRunAction, RootState>(PauseEngine),
        Reducer.On<ResumeEngineRunAction, RootState>(ResumeEngine),
        Reducer.On<CancelEngineRunAction, RootState>(CancelEngine),
        Reducer.On<SetRunScoresAction, RootState>(AddRunScores)
    ];

    private static RootState AddRun(RootState state, RunCreatedAction action) =>
        state.UpdateRun(action.Run.RunId, _ => action.Run);
    
    private static RootState AddOutput(RootState state, SetRunOutputsAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            Outputs = action.EngineOutput,
            // Scores = run.Scores
            //     .Concat(action.EngineOutputs.Select(val => (float) val.Metrics[MetricNames.Score].Value))
            //     .ToImmutableList(),
        });
    
    private static RootState AddRunScores(RootState state, SetRunScoresAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            Scores = run.Scores
                .Concat(action.Scores)
                .ToImmutableList(),
        });
    
    private static RootState SetRunInputs(RootState state, SetRunInputsAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            Inputs = action.Inputs
        });

    private static RootState CopyRun(RootState state, CopyRunAction action) => state
        .UpdateRun(action.NewRunId, run => run with
        {
            RunId = action.NewRunId,
            Index = state.Runs.Count,
            Inputs = state.Runs[action.CopyRunId].Inputs with { }
        });

    private static RootState RunCompleted(RootState state, EngineStoppedAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            IsRunning = false,
            IsPaused = false,
            IsCompleted = true,
            EndTime = DateTime.Now
        });

    private static RootState StartEngine(RootState state, StartEngineAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            IsRunning = true,
            IsPaused = false,
            IsCompleted = false,
            StartTime = DateTime.Now
        });

    private static RootState ResumeEngine(RootState state, ResumeEngineRunAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            IsPaused = false,
            IsRunning = true,
            IsCompleted = false
        });

    private static RootState PauseEngine(RootState state, PauseEngineRunAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            IsPaused = true,
            IsRunning = true,
            IsCompleted = false
        });

    private static RootState CancelEngine(RootState state, CancelEngineRunAction action) =>
        state.UpdateRun(action.RunId, run => run with
        {
            IsPaused = false,
            IsRunning = false,
            IsCompleted = true
        });
}