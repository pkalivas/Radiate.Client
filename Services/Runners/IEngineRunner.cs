using Radiate.Client.Services.Store.Models;

namespace Radiate.Client.Services.Runners;

public delegate IEngineRunner EngineRunnerFactory(string name);

public interface IEngineRunner
{
    Func<CancellationToken, Task> Run(RunInput inputs, CancellationTokenSource cts);

    RunInput GetInputs(RunInputsModel model);
}