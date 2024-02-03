using Radiate.Client.Components.Store.Models;

namespace Radiate.Client.Components.Store.Actions;

public record UpdateCurrentImageAction(Guid RunId, ImageEntity Image);