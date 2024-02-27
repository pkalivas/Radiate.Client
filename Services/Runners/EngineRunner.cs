using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Mappers;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Engines.Entities;
using Radiate.Engines.Harness;
using Radiate.Engines.Interfaces;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class EngineRunner<TEpoch, T> : IEngineRunner, IDisposable where TEpoch : IEpoch
{
    private static TimeSpan BufferTime => TimeSpan.FromMilliseconds(100);

    private readonly IStore<RootState> _store;
    private readonly IDisposable _outputSubscription;
    private readonly Subject<(Guid, RunOutputsState)> _outputs = new();
    
    protected EngineRunner(IStore<RootState> store)
    {
        _store = store;
        _outputSubscription = _outputs
            .Buffer(BufferTime)
            .Where(set => set.Any())
            .Subscribe(HandleOutputs);
    }
    
    protected abstract Task<IEngine<TEpoch, T>> BuildEngine(Guid runId, RunInputsState inputs);
    protected abstract RunOutputsState MapOnOutput(RunInputsState runInputs, RunOutputsState runOutputs, EngineOutput<TEpoch, T> output);
    protected abstract RunOutputsState MapOnStop(RunInputsState runInputs, RunOutputsState runOutputs, EngineOutput<TEpoch, T> output);
    
    public async Task Run(Guid runId, RunInputsState inputs, CancellationTokenSource cts)
    {
        var harness = new EngineEventHarness<TEpoch, T>(await BuildEngine(runId, inputs));
        var pauseControl = _store
            .Select(state => state.Runs[runId].IsPaused)
            .Subscribe(isPaused =>
            {
                if (isPaused && harness.CanPause)
                {
                    harness.Pause();
                }
                else if (!isPaused && harness.IsPaused)
                {
                    harness.Resume();
                }
            });
        
        harness.OnOutput += output => _outputs.OnNext((runId, MapOutput(output, inputs)));
        harness.OnStop += output =>
        {
            _store.Dispatch(new BatchRunOutputsAction(runId, [MapStop(output, inputs)]));
            _store.Dispatch(new EngineStoppedAction(runId));
        };
        
        await harness.Run();
        
        pauseControl.Dispose();
    }
    
    private RunOutputsState MapOutput(EngineOutput<TEpoch, T> output, RunInputsState inputs) =>
        MapOnOutput(inputs, Map(inputs, output), output);
    
    private RunOutputsState MapStop(EngineOutput<TEpoch, T> output, RunInputsState inputs) =>
        MapOnStop(inputs, Map(inputs, output), output);
    
    private void HandleOutputs(IList<(Guid, RunOutputsState)> outputs)
    {
        var runId = outputs.First().Item1;
        var runOutputs = outputs.Select(output => output.Item2).ToList();
        
        _store.Dispatch(new BatchRunOutputsAction(runId, runOutputs));
    }

    public void Dispose()
    {
        _outputSubscription.Dispose();
        _outputs.Dispose();
    }
    
    private static RunOutputsState Map(RunInputsState inputs, EngineOutput<TEpoch, T> output) => new()
    {
        EngineState = output.GetState(output.EngineId),
        EngineId = output.EngineId,
        ModelType = inputs.ModelType,
        EngineStates = output.EngineStates.ToImmutableDictionary(),
        Metrics = MetricMappers.GetMetricValues(output.Metrics).ToImmutableDictionary(key => key.Name),
    };
}