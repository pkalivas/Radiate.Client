using Plotly.Blazor;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Entities;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record ValidationPanelProjection
{
    public Guid RunId { get; init; }
    public string LossFunction { get; init; } = "";
    public string TrainTest { get; init; } = "";
    public ValidationSet CurrentValidation { get; init; } = new();
}

public record ValidationSet
{
    public string Name { get; init; } = "";
    public float ClassificationAccuracy { get; init; }
    public float RegressionAccuracy { get; init; }
    public float CategoricalAccuracy { get; init; }
    public float TotalLoss { get; init; }
    public TimeSpan Duration { get; init; }
    public int DataPoints { get; init; }
    public List<ValidationPrediction> PredictionValidations { get; init; } = new();
}

public record ValidationPrediction
{
    public float Loss { get; init; }
    public float ActualValue { get; init; }
    public float PredictedValue { get; init; }
    public float Difference => ActualValue - PredictedValue;
}