using System.Reactive.Linq;
using System.Reactive.Subjects;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Actions;
using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Selections;
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
    
    protected abstract Task<EngineOutput<TEpoch, T>> Fit(RunInputsModel inputs, CancellationTokenSource cts, Action<EngineOutput<TEpoch, T>> onEngineComplete);
    protected abstract RunOutputsModel MapToOutput(EngineOutput<TEpoch, T> handle);
    
    public async Task StartRun(Guid runId, RunInputsModel inputs, CancellationTokenSource cts)
    {
        var control = _store.Select(EngineSelectors.SelectEngineControl).Subscribe(OnControl);

        var result = await Fit(inputs, cts, handle =>
        {
            _store.Dispatch(new AddEngineOutputAction(MapToOutput(handle)));
            
            if (_pause.Value)
            {
                _pause.Where(val => !val).FirstAsync().Wait();
            }
        });
        
        _store.Dispatch(new AddEngineOutputAction(MapToOutput(result)));
        _store.Dispatch(new EngineStoppedAction());
        _pause.OnCompleted();
        control.Dispose();
    }
    
    private void OnControl(EngineControlModel control) => _pause.OnNext(control.IsPaused);
}