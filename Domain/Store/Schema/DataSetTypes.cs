namespace Radiate.Client.Domain.Store.Schema;

public static class DataSetTypes
{
    public const string Xor = "XOR";
    public const string Regression = "Regression";
    public const string Circle = "Circle";
    public const string Polygon = "Polygon";
    public const string DTZ = "DTZ";

    public static List<string> DTZTypes = new()
    {
        "DTZ1",
        "DTZ2",
        "DTZ6",
        "DTZ7",
    };
}