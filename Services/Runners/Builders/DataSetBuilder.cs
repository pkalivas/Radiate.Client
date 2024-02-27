using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Interfaces;
using Radiate.Optimizers.Interfaces;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.Builders;

public abstract class DataSetBuilder<TEpoch, T> : EngineBuilder<TEpoch, T>
    where TEpoch : IEpoch
    where T : IPredictionModel<T>
{
    private readonly ITensorFrameService _tensorFrameService;
    
    protected DataSetBuilder(ITensorFrameService tensorFrameService, IStore<RootState> store) : base(store)
    {
        _tensorFrameService = tensorFrameService;
    }
    
    protected abstract IEngine<TEpoch, T> BuildEngine(RunInputsState inputs, TensorFrame frame);
    protected abstract Task<TensorFrame> BuildFrame(RunInputsState inputs);

    protected override async Task<IEngine<TEpoch, T>> BuildEngine(Guid runId, RunInputsState inputs)
    {
        var frame = await BuildFrame(inputs);
        _tensorFrameService.SetTensorFrame(runId, frame);
        return BuildEngine(inputs, frame);
    }
}
