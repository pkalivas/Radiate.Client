using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Mappers;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Engines.Schema;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class EngineRunner<TEpoch, T> : IEngineRunner where TEpoch : IEpoch
{
    private static TimeSpan BufferTime => TimeSpan.FromMilliseconds(100);
    
    private readonly IStore<RootState> _store;
    private readonly IDisposable _outputSubscription;
    private readonly BehaviorSubject<bool> _pause = new(false);
    private readonly Subject<(Guid, RunOutputsState)> _outputs = new();

    protected EngineRunner(IStore<RootState> store)
    {
        _store = store;
        _outputSubscription = _outputs
            .Buffer(BufferTime)
            .Where(set => set.Any())
            .Subscribe(HandleOutputs);
    }
    
    protected abstract Task OnStartRun(RunInputsState inputs);
    protected abstract Task<EngineOutput<TEpoch, T>> Fit(RunInputsState inputs, CancellationTokenSource cts, Action<EngineOutput<TEpoch, T>> onEngineComplete);
    protected abstract RunOutputsState MapToOutput(EngineOutput<TEpoch, T> handle, RunInputsState inputs, bool isLast = false);
    
    public async Task StartRun(Guid runId, RunInputsState inputs, CancellationTokenSource cts)
    {
        await OnStartRun(inputs);
        
        var control = _store.Select(state => state.Runs[runId].IsPaused)
            .Subscribe(isPaused => _pause.OnNext(isPaused));

        var result = await Fit(inputs, cts, handle =>
        {
            _outputs.OnNext((runId, MapToOutput(handle, inputs)));
            
            if (_pause.Value)
            {
                _pause.Where(val => !val).FirstAsync().Wait();
            }
        });
        
        _outputs.OnNext((runId, MapToOutput(result, inputs)));
        _store.Dispatch(new EngineStoppedAction(runId));
        
        Thread.Sleep(BufferTime);
        
        _pause.OnCompleted();
        _outputs.OnCompleted();
        _outputSubscription.Dispose();
        control.Dispose();
    }
    
    private RunOutputsState Map(EngineOutput<TEpoch, T> output, RunInputsState inputs, bool isLast = false) => new()
    {
        EngineState = output.GetState(output.EngineId),
        EngineId = output.EngineId,
        EngineStates = output.EngineStates.ToImmutableDictionary(),
        Metrics = MetricMappers.GetMetricValues(output.Metrics).ToImmutableDictionary(key => key.Name),
    };

    private void HandleOutputs(IList<(Guid, RunOutputsState)> outputs) =>
        _store.Dispatch(new SetRunOutputsAction(outputs
            .Select(val => val.Item1)
            .Distinct()
            .Single(), outputs
            .Select(pair => pair.Item2)
            .OrderBy(val => val.Metrics[MetricNames.Index].Value)
            .ToList()));
}