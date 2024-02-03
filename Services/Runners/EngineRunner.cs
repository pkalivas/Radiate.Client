using System.Reactive.Linq;
using System.Reactive.Subjects;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Actions;
using Radiate.Client.Services.Store.Models;
using Radiate.Engines.Entities;

namespace Radiate.Client.Services.Runners;

public abstract class EngineRunner<T> : IEngineRunner where T : IRunInput<T>
{
    private readonly Reflow.Store<RootState> _store;
    private readonly BehaviorSubject<bool> _pause = new(false);

    protected EngineRunner(Reflow.Store<RootState> store)
    {
        _store = store;
    }
    
    protected abstract RunOutputsModel MapToOutput(EngineHandle handle);
    protected abstract Task<EngineHandle> Fit(RunInput inputs, CancellationTokenSource cts, Action<EngineHandle> onEngineComplete);
    
    public abstract RunInput GetInputs(RunInputsModel model);
    
    public async Task StartRun(Guid runId, RunInput inputs, CancellationTokenSource cts)
    {
        var pause = _store.OnAction<PauseEngineRunAction>()
            .Select(pair => pair.Action)
            .Where(action => action.RunId == runId)
            .Subscribe(_ => _pause.OnNext(true));
        
        var resume = _store.OnAction<ResumeEngineRunAction>()
            .Select(pair => pair.Action)
            .Where(action => action.RunId == runId)
            .Subscribe(_ => _pause.OnNext(false));
        
        var cancel = _store.OnAction<CancelEngineRunAction>()
            .Select(pair => pair.Action)
            .Where(action => action.RunId == runId)
            .Subscribe(_ => _pause.OnNext(false));
        
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
        
        pause.Dispose();
        resume.Dispose();
        cancel.Dispose();
    }
}