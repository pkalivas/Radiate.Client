using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Runners.Builders;

namespace Radiate.Client.Services.Runners.Interfaces;

public delegate IEngineBuilder EngineRunnerFactory(string model, string data);

public interface IEngineRunner
{
    Task StartRun(Guid runId, RunInputsState inputs, CancellationTokenSource cts);
}