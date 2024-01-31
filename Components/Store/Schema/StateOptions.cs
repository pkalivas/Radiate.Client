namespace Radiate.Client.Components.Store.Schema;

public static class StateOptions
{
    public static List<string> ModelNames => new()
    {
        "Graph",
        "Tree",
        "MultilayerPerceptron",
        "Image"
    };
    
    public static List<string> DataSetNames => new()
    {
        "XOR",
        "Image"
    };
    
    public static List<string> ImageDataSetNames => new()
    {
        "Circle",
        "Polygon"
    };  
}