using Radiate.Client.Services.Store.Models;

namespace Radiate.Client.Services.Store.Actions;

public record UpdateCurrentImageAction(Guid RunId, ImageEntity Image);