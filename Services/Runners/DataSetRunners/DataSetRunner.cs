using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Interfaces;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.DataSetRunners;

public abstract class DataSetRunner<TEpoch, T> : EngineRunner<TEpoch, T> where TEpoch : IEpoch
{
    protected DataSetRunner(IStore<RootState> store) : base(store) { }
    
    protected TensorFrame? Frame { get; private set; }
    
    protected abstract Task<TensorFrame> LoadFrame();
    protected abstract TensorFrame FormatFrame(TensorFrame frame);

    protected override async Task OnStartRun(RunInputsState inputs)
    {
        Frame ??= FormatFrame(await LoadFrame());
    }
}
