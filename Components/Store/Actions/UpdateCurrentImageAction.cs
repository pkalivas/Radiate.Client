using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.Models;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.Actions;

public record UpdateCurrentImageAction(Guid RunId, ImageEntity Image) : IAction<RootFeature>;