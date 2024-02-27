using Radiate.Client.Domain.Store.Models.States;

namespace Radiate.Client.Services.Runners.Interfaces;


public interface IEngineRunner
{
    Task StartRun(Guid runId, RunInputsState inputs, CancellationTokenSource cts);
}