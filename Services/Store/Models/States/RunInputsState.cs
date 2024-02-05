namespace Radiate.Client.Services.Store.Models.States;

public record RunInputsState 
{
    public string ModelType { get; set; } = "Image";
    public string DataSetType { get; set; } = "Polygon";
    public PopulationInputs PopulationInputs { get; set; } = new();
    public LimitInputs LimitInputs { get; set; } = new();
    public ImageInputs ImageInputs { get; set; } = new();
}

public record PopulationInputs
{
    public int PopulationSize { get; set; } = 50;
    public float MutationRate { get; set; } = 0.02f;
}

public record LimitInputs
{
    public int IterationLimit { get; set; } = 1000;
}

public record ImageInputs
{
    public int Height { get; set; } = 50;
    public int Width { get; set; } = 50;
    public int NumShapes { get; set; } = 150;
    public int NumVertices { get; set; } = 4;
    public ImageEntity TargetImage { get; set; } = new();
}

