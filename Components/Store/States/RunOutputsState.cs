using Radiate.Client.Services.Runners;
using Radiate.Engines.Entities;

namespace Radiate.Client.Components.Store.States;

public class RunOutputsState
{
    public string EngineState { get; set; } = "";
    public string EngineId { get; set; } = "";
    public MetricSet Metrics { get; init; } = new();
    public Dictionary<string, EngineState> EngineStates { get; init; } = new();
    public List<RunOutputValue> Outputs { get; init; } = new();
    
    public T GetOutputValue<T>(string name)
    {
        if (Outputs.Count == 0)
        {
            return default!;
        }
        var output = Outputs.FirstOrDefault(x => x.Name == name);
        if (output == null)
        {
            throw new ArgumentException($"Output with name {name} not found.");
        }

        return (T)Convert.ChangeType(output.Value, typeof(T));
    }

    public override bool Equals(object? obj)
    {
        return obj is RunOutputsState state &&
               EngineState == state.EngineState &&
               EngineId == state.EngineId &&
               EqualityComparer<MetricSet>.Default.Equals(Metrics, state.Metrics) &&
               EqualityComparer<Dictionary<string, EngineState>>.Default.Equals(EngineStates, state.EngineStates) &&
               EqualityComparer<List<RunOutputValue>>.Default.Equals(Outputs, state.Outputs);
    }
}
