using System.Reactive.Linq;
using System.Reactive.Subjects;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Actions;
using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class EngineRunner<TEpoch, T> : IEngineRunner
    where TEpoch : IEpoch
{
    private readonly IStore<RootState> _store;
    private readonly BehaviorSubject<bool> _pause = new(false);

    protected EngineRunner(IStore<RootState> store)
    {
        _store = store;
    }
    
    protected abstract Task<EngineOutput<TEpoch, T>> Fit(RunInputsState inputs, CancellationTokenSource cts, Action<EngineOutput<TEpoch, T>> onEngineComplete);
    protected abstract RunOutputsState MapToOutput(EngineOutput<TEpoch, T> handle);
    
    public async Task StartRun(Guid runId, RunInputsState inputs, CancellationTokenSource cts)
    {
        var control = _store.Select(state => state.Runs[runId].IsPaused)
            .Subscribe(isPaused => _pause.OnNext(isPaused));

        var result = await Fit(inputs, cts, handle =>
        {
            _store.Dispatch(new SetRunOutputsAction(MapToOutput(handle)));
            
            if (_pause.Value)
            {
                _pause.Where(val => !val).FirstAsync().Wait();
            }
        });
        
        _store.Dispatch(new SetRunOutputsAction(MapToOutput(result)));
        _store.Dispatch(new EngineStoppedAction());
        _pause.OnCompleted();
        control.Dispose();
    }
}