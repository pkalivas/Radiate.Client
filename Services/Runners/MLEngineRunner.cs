using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Interfaces;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class MLEngineRunner<TEpoch, T> : EngineRunner<TEpoch, T> where TEpoch : IEpoch
{
    protected MLEngineRunner(IStore<RootState> store) : base(store) { }
    
    protected TensorFrame Frame { get; set; }
    
    protected abstract Task<TensorFrame> LoadDataSet();
    
    protected override async Task OnStartRun(RunInputsState inputs)
    {
        Frame = await LoadDataSet();
    }
}