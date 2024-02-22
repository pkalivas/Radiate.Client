using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Extensions;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.DataSetRunners;

public abstract class DataSetRunner<TEpoch, T> : EngineRunner<TEpoch, T> where TEpoch : IEpoch
{
    private readonly ITensorFrameService _tensorFrameService;

    protected DataSetRunner(ITensorFrameService tensorFrameService, IStore<RootState> store) : base(store)
    {
        _tensorFrameService = tensorFrameService;
    }

    protected abstract Task<IEngine<TEpoch, T>> BuildEngine(RunInputsState inputs, TensorFrame frame);

    protected abstract Task<TensorFrame> BuildFrame(RunInputsState inputs);

    protected override async Task<EngineOutput<TEpoch, T>> Fit(Guid runId,
        RunInputsState inputs,
        CancellationTokenSource cts,
        Action<EngineOutput<TEpoch, T>> onEngineComplete)
    {
        var frame = await BuildFrame(inputs);
        
        _tensorFrameService.SetTensorFrame(runId, frame);
        
        return (await BuildEngine(inputs, frame)).Fit()
            .Peek(onEngineComplete)
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
    }
}