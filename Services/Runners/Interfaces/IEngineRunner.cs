using Radiate.Client.Domain.Store.Models.States;

namespace Radiate.Client.Services.Runners.Interfaces;

public delegate IEngineRunner EngineRunnerFactory(string model, string data);

public interface IEngineRunner
{
    Task Run(Guid runId, RunInputsState inputs, CancellationTokenSource cts);
}