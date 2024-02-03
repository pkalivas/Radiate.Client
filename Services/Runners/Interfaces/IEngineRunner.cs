using Radiate.Client.Services.Store.Models;

namespace Radiate.Client.Services.Runners.Interfaces;

public delegate IEngineRunner EngineRunnerFactory(string model, string data);

public interface IEngineRunner
{
    Task StartRun(Guid runId, RunInputsModel inputs, CancellationTokenSource cts);
}