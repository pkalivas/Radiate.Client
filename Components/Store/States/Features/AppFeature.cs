using Radiate.Client.Components.Store.Models;
using Radiate.Client.Services.Runners;
using Radiate.Engines.Entities;
using Radiate.Engines.Statistics.Stats;

namespace Radiate.Client.Components.Store.States.Features;

public record AppFeature : Feature<AppFeature>
{
    public CancellationTokenSource CancellationTokenSource { get; set; } = new();
    public bool Running { get; set; } = false;
    public string ModelType { get; set; } = "Graph";
    public string DataSetType { get; set; } = "XOR";
    public List<float> Scores { get; set; } = new();
    public List<RunsState> Runs { get; set; } = new();
    public RunOutputsState RunOutputses { get; set; } = new();
    public RunInputState RunInput { get; set; } = new();
    public ImageState ImageState { get; set; } = new();

    public FloatStatistics? GetStats(string name)
    {
        var stats = RunOutputses.Metrics.Get(name);
        return stats?.Statistics;
    }

    public override AppFeature Copy()
    {
        return new()
        {
            CancellationTokenSource = CancellationTokenSource,
            Running = Running,
            ModelType = ModelType,
            DataSetType = DataSetType,
            Scores = Scores.ToList(),
            Runs = Runs.ToList(),
            RunOutputses = RunOutputses,
            RunInput = RunInput,
            ImageState = ImageState
        };
    }
}

public record RunsState
{
    public Guid RunId { get; set; } = Guid.NewGuid();
    public string Status { get; set; } = "";
    public RunInputState Inputs { get; set; } = new();
}

// public record EngineInputsState
// {
//     public string ModelType { get; set; } = "Graph";
//     public string DataSetType { get; set; } = "XOR";
//     public int PopulationSize { get; set; } = 50;
//     public float MutationRate { get; set; } = 0.02f;
//     public int IterationLimit { get; set; } = 1000;
//     public int NumShapes { get; set; } = 150;
//     public int NumVertices { get; set; } = 4;
// }

// public record ImageState
// {
//     public int ImageWidth { get; set; } = 50;
//     public int ImageHeight { get; set; } = 50;
//     public ImageEntity Target { get; set; } = new();
//     public ImageEntity Current { get; set; } = new();
// }

// public class EngineOutputState
// {
//     public string EngineState { get; set; } = "";
//     public string EngineId { get; set; } = "";
//     public MetricSet Metrics { get; init; } = new();
//     public Dictionary<string, EngineState> EngineStates { get; init; } = new();
//     public List<RunOutputValue> Outputs { get; init; } = new();
//     
//     public T GetOutputValue<T>(string name)
//     {
//         if (Outputs.Count == 0)
//         {
//             return default!;
//         }
//         var output = Outputs.FirstOrDefault(x => x.Name == name);
//         if (output == null)
//         {
//             throw new ArgumentException($"Output with name {name} not found.");
//         }
//
//         return (T)Convert.ChangeType(output.Value, typeof(T));
//     }
//
//     public override bool Equals(object? obj)
//     {
//         return obj is EngineOutputState state &&
//                EngineState == state.EngineState &&
//                EngineId == state.EngineId &&
//                EqualityComparer<MetricSet>.Default.Equals(Metrics, state.Metrics) &&
//                EqualityComparer<Dictionary<string, EngineState>>.Default.Equals(EngineStates, state.EngineStates) &&
//                EqualityComparer<List<RunOutputValue>>.Default.Equals(Outputs, state.Outputs);
//     }
// }

