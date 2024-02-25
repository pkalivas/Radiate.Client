namespace Radiate.Client.Domain.Templates.Panels;

public class CardPanel : Panel
{
    public Type Content { get; init; }
    public Dictionary<string, object> Props { get; init; } = new();
    public List<string> Actions { get; init; } = new();
}
