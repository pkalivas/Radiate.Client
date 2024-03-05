using System.Text.Json.Serialization;

namespace Radiate.Client.Domain.Templates.Panels;

public record CardPanel : Panel
{
    [JsonIgnore] public Type Content { get; init; }
    [JsonIgnore] public Type Actions { get; init; }
    public Dictionary<string, object> Props { get; init; } = new();
    public Dictionary<string, object> ActionsProps { get; init; } = new();
}
