using Radiate.Client.Services.Store.Models;

namespace Radiate.Client.Services.Runners.Interfaces;

public delegate IEngineRunner EngineRunnerFactory(string name);

public interface IEngineRunner
{
    Task StartRun(Guid runId, RunInput inputs, CancellationTokenSource cts);

    RunInput GetInputs(RunInputsModel model);
}