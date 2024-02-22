using System.Collections.Immutable;

namespace Radiate.Client.Domain.Store.Models.States;

public record UiState
{
    public bool IsSidebarOpen { get; init; } = true;
    public IImmutableDictionary<Guid, bool> LoadingStates { get; init; } = new Dictionary<Guid, bool>().ToImmutableDictionary();
    
    public override int GetHashCode() => HashCode.Combine(IsSidebarOpen, LoadingStates);
}