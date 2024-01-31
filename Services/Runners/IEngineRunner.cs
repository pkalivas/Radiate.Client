using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Services.Runners;

public delegate IEngineRunner EngineRunnerFactory(string name);

public interface IEngineRunner
{
    Func<CancellationToken, Task> Run(RunInput inputs, CancellationTokenSource cts, Action<EngineOutputState> resultCallback);

    RunInput GetInputs(AppState state);
}