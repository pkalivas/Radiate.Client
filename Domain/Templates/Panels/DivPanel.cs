using System.Text.Json.Serialization;

namespace Radiate.Client.Domain.Templates.Panels;

public record DivPanel : Panel
{
    [JsonIgnore] public Type Content { get; init; }
    public Dictionary<string, object> Props { get; init; } = new();
}