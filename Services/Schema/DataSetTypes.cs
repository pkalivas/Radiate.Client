namespace Radiate.Client.Services.Schema;

public static class DataSetTypes
{
    public const string Xor = "XOR";
    public const string Regression = "Regression";
    public const string Circle = "Circle";
    public const string Polygon = "Polygon";
    public const string DTLZ1 = "DTLZ1";
    public const string DTLZ2 = "DTLZ2";
    public const string DTLZ6 = "DTLZ6";
    public const string DTLZ7 = "DTLZ7";

    public static List<string> DTZTypes = new()
    {
        "DTZ1",
        "DTZ2",
        "DTZ6",
        "DTZ7",
    };
}