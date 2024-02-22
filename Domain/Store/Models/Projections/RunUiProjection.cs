using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record RunUiProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsSidebarOpen { get; init; } = true;
    public IRunUITemplate UiTemplate { get; init; } = default!;
}