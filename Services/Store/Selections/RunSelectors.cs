using Radiate.Client.Services.Store.Models.Projections;
using Radiate.Client.Services.Store.Models.States;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

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
        .Create<RootState, RunState, MetricSummaryChartPanelProjection>(SelectRun, run => new MetricSummaryChartPanelProjection
        {
            RunId = run.RunId,
            MetricName = metricName,
            Value = run.Outputs.Metrics.GetValueOrDefault(metricName, new MetricValueModel())
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
        .Create<RootState, RunState, ValidationPanelProjection>(SelectRun, state => new ValidationPanelProjection
        {
            RunId = state.RunId,
            LossFunction = state.Outputs.ValidationOutput.LossFunction,
            Validations = new()
            {
                new ValidationSet
                {
                    Name = "Train",
                    ClassificationAccuracy = state.Outputs.ValidationOutput.TrainValidation.ClassificationAccuracy,
                    RegressionAccuracy = state.Outputs.ValidationOutput.TrainValidation.RegressionAccuracy,
                    CategoricalAccuracy = state.Outputs.ValidationOutput.TrainValidation.CategoricalAccuracy,
                    TotalLoss = state.Outputs.ValidationOutput.TrainValidation.TotalLoss,
                    Duration = state.Outputs.ValidationOutput.TrainValidation.Duration,
                    DataPoints = state.Outputs.ValidationOutput.TrainValidation.DataPoints
                },
                new ValidationSet
                {
                    Name = "Test",
                    ClassificationAccuracy = state.Outputs.ValidationOutput.TestValidation.ClassificationAccuracy,
                    RegressionAccuracy = state.Outputs.ValidationOutput.TestValidation.RegressionAccuracy,
                    CategoricalAccuracy = state.Outputs.ValidationOutput.TestValidation.CategoricalAccuracy,
                    TotalLoss = state.Outputs.ValidationOutput.TestValidation.TotalLoss,
                    Duration = state.Outputs.ValidationOutput.TestValidation.Duration,
                    DataPoints = state.Outputs.ValidationOutput.TestValidation.DataPoints
                }
            }
        });
}