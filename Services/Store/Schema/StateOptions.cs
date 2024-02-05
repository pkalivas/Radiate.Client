namespace Radiate.Client.Services.Store.Schema;

public static class StateOptions
{
    public static List<string> ModelNames => new()
    {
        "Graph",
        "Tree",
        "Image"
    };
    
    public static List<string> DataSetNames => new()
    {
        "XOR",
        "Regression"
    };
    
    public static List<string> ImageDataSetNames => new()
    {
        "Circle",
        "Polygon"
    };
}