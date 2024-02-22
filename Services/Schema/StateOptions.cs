namespace Radiate.Client.Services.Schema;

public static class StateOptions
{
    public static List<string> ModelNames => new()
    {
        "Graph",
        "Tree",
        "Image",
        "MultiObjective"
    };
    
    public static List<string> GetModelDataSets(string modelName) => modelName switch
    {
        ModelTypes.Graph =>
        [
            DataSetTypes.Xor,
            DataSetTypes.Regression
        ],
        ModelTypes.Tree => [DataSetTypes.Regression],
        ModelTypes.Image =>
        [
            DataSetTypes.Polygon,
            DataSetTypes.Circle
        ],
        ModelTypes.MultiObjective => 
        [
            DataSetTypes.DTLZ1,
            DataSetTypes.DTLZ2,
            DataSetTypes.DTLZ6,
            DataSetTypes.DTLZ7
        ],
        _ => ["Unknown"]
    };
}