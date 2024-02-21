using System.Reactive.Linq;
using System.Reactive.Subjects;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Actions;
using Radiate.Client.Services.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Engines.Schema;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class EngineRunner<TEpoch, T> : IEngineRunner where TEpoch : IEpoch
{
    private static TimeSpan BufferTime => TimeSpan.FromMilliseconds(500);
    
    private readonly IStore<RootState> _store;
    private readonly IDisposable _outputSubscription;
    private readonly BehaviorSubject<bool> _pause = new(false);
    private readonly Subject<RunOutputsState> _outputs = new();

    protected EngineRunner(IStore<RootState> store)
    {
        _store = store;
        _outputSubscription = _outputs
            .Buffer(BufferTime)
            .Where(set => set.Any())
            .Subscribe(HandleOutputs);
    }
    
    protected abstract Task<EngineOutput<TEpoch, T>> Fit(RunInputsState inputs, CancellationTokenSource cts, Action<EngineOutput<TEpoch, T>> onEngineComplete);
    protected abstract RunOutputsState MapToOutput(EngineOutput<TEpoch, T> handle, RunInputsState inputs, bool isLast = false);
    
    public async Task StartRun(Guid runId, RunInputsState inputs, CancellationTokenSource cts)
    {
        var control = _store.Select(state => state.Runs[runId].IsPaused)
            .Subscribe(isPaused => _pause.OnNext(isPaused));

        var result = await Fit(inputs, cts, handle =>
        {
            _outputs.OnNext(MapToOutput(handle, inputs));
            
            if (_pause.Value)
            {
                _pause.Where(val => !val).FirstAsync().Wait();
            }
        });
        
        _outputs.OnNext(MapToOutput(result, inputs, true));
        _store.Dispatch(new EngineStoppedAction(runId));
        
        Thread.Sleep(BufferTime);
        
        _pause.OnCompleted();
        _outputs.OnCompleted();
        _outputSubscription.Dispose();
        control.Dispose();
    }

    private void HandleOutputs(IList<RunOutputsState> outputs) =>
        _store.Dispatch(new SetRunOutputsAction(outputs
            .OrderBy(val => val.Metrics[MetricNames.Index].Value)
            .ToList()));
}