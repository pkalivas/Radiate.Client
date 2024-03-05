namespace Radiate.Client.Domain.Store.Models.Projections;

public record PanelDisplayProjection(Guid PanelId, Type Component, Dictionary<string, object> Props);
