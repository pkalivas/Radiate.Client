using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;

namespace Radiate.Client.Services.Runners.OutputTransforms;

public interface IRunOutputTransform<TEpoch, T> where TEpoch : IEpoch
{
    RunOutputsState Transform(Guid runId, EngineOutput<TEpoch, T> handle, RunOutputsState output, RunInputsState input, bool isLast);
}