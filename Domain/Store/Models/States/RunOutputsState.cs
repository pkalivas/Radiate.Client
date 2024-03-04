using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States.Outputs;
using Radiate.Engines.Entities;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunOutputsState
{
    public string EngineState { get; init; } = "";
    public string EngineId { get; init; } = "";
    public string ModelType { get; init; } = "";
    public IImmutableDictionary<string, MetricValueModel> Metrics { get; init; } = ImmutableDictionary<string, MetricValueModel>.Empty;
    public IImmutableDictionary<string, EngineState> EngineStates { get; init; } = ImmutableDictionary<string, EngineState>.Empty;
    public EngineStateOutputs EngineStateOutputs { get; init; } = new();
    public ImageOutput ImageOutput { get; init; } = new();
    public GraphOutput GraphOutput { get; init; } = new();
    public TreeOutput TreeOutput { get; init; } = new();
    public ValidationOutput ValidationOutput { get; init; } = new();
    public ParetoFrontOutput ParetoFrontOutput { get; init; } = new();
}