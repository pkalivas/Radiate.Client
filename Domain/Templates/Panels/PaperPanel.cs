namespace Radiate.Client.Domain.Templates.Panels;

public class PaperPanel : Panel
{
    public int Height { get; init; } = 250;
    public Type Content { get; init; }
    public Dictionary<string, object> Props { get; init; } = new();
}
