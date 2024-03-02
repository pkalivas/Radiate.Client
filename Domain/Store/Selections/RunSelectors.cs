using Plotly.Blazor;
using Plotly.Blazor.Traces;
using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Mappers;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Entities;
using Radiate.Engines.Schema;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class RunSelectors
{
    public static ISelector<RootState, RunState> SelectRun => Selectors
        .Create<RootState, RunState>(state => state.Runs.TryGetValue(state.CurrentRunId, out var run) ? run : new RunState());
    
    public static ISelector<RootState, InputsPanelModelProjection> SelectInputsModel => Selectors
        .Create<RootState, RunState, InputsPanelModelProjection>(SelectRun, run => new InputsPanelModelProjection
        {
            RunId = run.RunId,
            Inputs = run.Inputs,
            IsReadonly = run.IsRunning || run.IsPaused || run.IsCompleted
        });
    
    public static readonly ISelector<RootState, MetricDataGridPanelProjection> SelectMetricDataGridPanelModel = Selectors
        .Create<RootState, RunState, MetricDataGridPanelProjection>(SelectRun, run => new MetricDataGridPanelProjection
        {
            RunId = run.RunId,
            Values = run.Outputs.Metrics.Values.ToList()
        });
    
    public static ISelector<RootState, MetricSummaryChartPanelProjection> SelectMetricSummaryChartPanelModel(string metricName) => Selectors
        .Create<RootState, RunState, MetricSummaryChartPanelProjection>(SelectRun, run =>
        {
            var metric = run.Outputs.Metrics.GetValueOrDefault(metricName, new MetricValueModel());
            return new MetricSummaryChartPanelProjection
            {
                RunId = run.RunId,
                MetricName = metricName,
                Value = metric,
                Traces = new List<ITrace>
                {
                    metric.Name switch
                    {
                        MetricNames.FitnessDistribution => TraceMappers.GetScatter(metric.Distribution),
                        MetricNames.GenomeSizeDistribution => TraceMappers.GetPieTrace(metric.Distribution),
                        MetricNames.AgeDistribution => TraceMappers.GetBarTrace(metric.Distribution),
                        _ => new Scatter()
                    }
                }
            };
        });

    public static ISelector<RootState, RunListPanelProjection> SelectRunListPanelProjection => Selectors
        .Create<RootState, RunListPanelProjection>(state => new RunListPanelProjection
        {
            RunId = state.CurrentRunId,
            Runs = state.Runs.Values
                .OrderByDescending(val => val.Index)
                .ToList()
        });
    
    public static ISelector<RootState, ValidationPanelProjection> SelectValidationPanelProjection => Selectors
        .Create<RootState, RunState, RunUiState, ValidationPanelProjection>(SelectRun, RunUiSelectors.SelectRunUiState, 
        (state, uiState) => new ValidationPanelProjection
        {
            RunId = state.RunId,
            LossFunction = state.Outputs.ValidationOutput.LossFunction,
            TrainTest = uiState.TrainTest,
            CurrentValidation = uiState.TrainTest switch
            {
                TrainTestTypes.Train => new ValidationSet
                {
                    Name = TrainTestTypes.Train,
                    ClassificationAccuracy = state.Outputs.ValidationOutput.TrainValidation.ClassificationAccuracy,
                    RegressionAccuracy = state.Outputs.ValidationOutput.TrainValidation.RegressionAccuracy,
                    CategoricalAccuracy = state.Outputs.ValidationOutput.TrainValidation.CategoricalAccuracy,
                    TotalLoss = state.Outputs.ValidationOutput.TrainValidation.TotalLoss,
                    Duration = state.Outputs.ValidationOutput.TrainValidation.Duration,
                    DataPoints = state.Outputs.ValidationOutput.TrainValidation.DataPoints,
                    PredictionValidations = state.Outputs.ValidationOutput.TrainValidation.PredictionValidations
                        .Select(Map)
                        .ToList()
                },
                TrainTestTypes.Test => new ValidationSet
                {
                    Name = TrainTestTypes.Test,
                    ClassificationAccuracy = state.Outputs.ValidationOutput.TestValidation.ClassificationAccuracy,
                    RegressionAccuracy = state.Outputs.ValidationOutput.TestValidation.RegressionAccuracy,
                    CategoricalAccuracy = state.Outputs.ValidationOutput.TestValidation.CategoricalAccuracy,
                    TotalLoss = state.Outputs.ValidationOutput.TestValidation.TotalLoss,
                    Duration = state.Outputs.ValidationOutput.TestValidation.Duration,
                    DataPoints = state.Outputs.ValidationOutput.TestValidation.DataPoints,
                    PredictionValidations = state.Outputs.ValidationOutput.TestValidation.PredictionValidations
                        .Select(Map)
                        .ToList()
                },
            },
        });
    
    public static ISelector<RootState, AccuracyChartPanelProjection> SelectAccuracyChartPanelModel => Selectors
        .Create<RootState, RunState, RunUiState, AccuracyChartPanelProjection>(SelectRun, RunUiSelectors.SelectRunUiState,
            (state, uiState) => new AccuracyChartPanelProjection
            { 
                RunId = state.RunId,
                TrainTestType = uiState.TrainTest,
                Traces = uiState.TrainTest switch
                {
                    TrainTestTypes.Train => new List<ITrace>
                    {
                        TraceMappers.GetScatter(state.Outputs.ValidationOutput.TrainValidation.PredictionValidations
                            .Select(pred => (double)pred.Confidence).ToArray(), "Predicted"),
                        TraceMappers.GetScatter(state.Outputs.ValidationOutput.TrainValidation.PredictionValidations
                            .Select(pred => (double)pred.Label.First()).ToArray(), "Actual")
                    },
                    TrainTestTypes.Test => new List<ITrace>
                    {
                        TraceMappers.GetScatter(state.Outputs.ValidationOutput.TestValidation.PredictionValidations
                            .Select(pred => (double)pred.Confidence).ToArray(), "Predicted"),
                        TraceMappers.GetScatter(state.Outputs.ValidationOutput.TestValidation.PredictionValidations
                            .Select(pred => (double)pred.Label.First()).ToArray(), "Actual")
                    },
                    _ => new List<ITrace>()
                }
            });
    
    private static ValidationPrediction Map(PredictionValidation val) => new()
    {
        Loss = val.Loss,
        ActualValue = val.Label.First(),
        PredictedValue = val.FeatureOutput.Count() > 1 ? val.Classification : val.Confidence
    };
}