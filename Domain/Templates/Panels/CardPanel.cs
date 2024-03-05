using System.Text.Json.Serialization;

namespace Radiate.Client.Domain.Templates.Panels;

public record CardPanel : Panel
{
    [JsonIgnore] public Type Content { get; init; }
    public Dictionary<string, object> Props { get; init; } = new();
}
