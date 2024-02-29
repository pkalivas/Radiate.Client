using Plotly.Blazor.Traces;
using Plotly.Blazor.Traces.ScatterLib;

namespace Radiate.Client.Services.Mappers;

public static class TraceMappers
{
    public static Scatter GetScatter(double[] values, string name = "") => new()
    {
        Name = name,
        Mode = ModeFlag.Lines,
        Y = values.Select(val => (object) val).ToList(),
        X = values.Select((_, idx) => (object) idx).ToList(),
    };
    
    public static Bar GetBarTrace(double[] values)
    {
        var buckets = values
            .GroupBy(val => val)
            .Select(group => new {Value = group.Key, Count = group.Count()})
            .OrderByDescending(group => group.Count)
            .ToList();
        
        return new Bar
        {
            Name = "BarTrace",
            X = buckets.Select(val => (object) val.Value).ToList(),
            Y = buckets.Select(val => (object) val.Count).ToList(),
        };
    }
    
    public static Pie GetPieTrace(double[] values)
    {
        var buckets = values
            .GroupBy(val => val)
            .Select(group => new {Value = group.Key, Count = group.Count()})
            .OrderByDescending(group => group.Count)
            .ToList();
        
        return new Pie
        {
            Name = "PieTrace",
            Values = buckets.Select(val => (object) val.Value).ToList(),
            Labels = buckets.Select(val => (object) val.Count).ToList(),
        };
    }

    public static Scatter3D GetScatter3DTrace(List<double[]> points) => new()
    {
        Mode = Plotly.Blazor.Traces.Scatter3DLib.ModeFlag.Markers,
        X = points.Select(val => (object) val[0]).ToList(),
        Y = points.Select(val => (object) val[1]).ToList(),
        Z = points.Select(val => (object) val[2]).ToList(),
    };
    
}