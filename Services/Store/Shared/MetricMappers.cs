using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.Projections;
using Radiate.Engines.Entities;
using Radiate.Engines.Schema;
using Radiate.Engines.Statistics.Stats;

namespace Radiate.Client.Services.Store.Shared;

public static class MetricMappers
{
    public static List<MetricValueModel> GetMetricValues(MetricSet metricSet) =>
        metricSet.Metrics.Values.SelectMany(FlattenMetric).Select(GetMetricValue).ToList();
    
    public static List<MetricValueModel> GetMetricValues(MetricValue metric)
    {
        var list = new List<MetricValueModel>();
        foreach (var value in FlattenMetric(metric))
        {
            list.Add(GetMetricValue(value));
        }
        
        return list;
    }
    
    public static IEnumerable<MetricValue> FlattenMetric(MetricValue value)
    {
        if (value.Children.Count == 0)
        {
            return new List<MetricValue> { value };
        }
        
        var list = new List<MetricValue>();
        foreach (var child in value.Children)
        {
            list.AddRange(FlattenMetric(child.Value));
        }
        
        return list;
    }    
    
    public static MetricValueModel GetMetricValue(MetricValue metric)
    {
        if (metric.MetricType is not MetricTypes.Description)
        {
            return new MetricValueModel
            {
                Name = metric.Name,
                MetricType = MetricTypes.Distribution,
                Distribution = metric.Statistics.LastSequence.Select(val => (double)val).ToArray(),
                Value = RoundMetric(metric, stat => stat.LastValue),
                Min = RoundMetric(metric, stat => stat.Min),
                Max = RoundMetric(metric, stat => stat.Max),
                Mean = RoundMetric(metric, stat => stat.Mean),
                Variance = RoundMetric(metric, stat => stat.Variance),
                Skewness = RoundMetric(metric, stat => stat.Skewness),
                Kurtosis = RoundMetric(metric, stat => stat.Kurtosis),
                Sum = RoundMetric(metric, stat => stat.Sum),
                Current = RoundTime(metric, val => val.LastValue),
                Total = RoundTime(metric, val => val.Sum),
                MeanTime = RoundTime(metric, val => (float)val.Mean),
                MinTime = RoundTime(metric, val => (float)val.Min),
                MaxTime = RoundTime(metric, val => (float)val.Max),
            };
        }
        
        return new MetricValueModel
        {
            Name = metric.Name,
            MetricType = MetricTypes.Operation,
            Description = metric.Description,
        };
    }
    
    private static double RoundMetric(MetricValue metric, Func<FloatStatistics, double> selector) =>
        Math.Round(selector(metric.Statistics), 4);

    private static TimeSpan RoundTime(MetricValue metric, Func<FloatStatistics, float> selector)
    {
        var value = selector(metric.Time);
        
        return TimeSpan.FromMilliseconds(float.IsNaN(value) || float.IsPositiveInfinity(value) || float.IsNegativeInfinity(value)
            ? 0
            : value);
    }

}