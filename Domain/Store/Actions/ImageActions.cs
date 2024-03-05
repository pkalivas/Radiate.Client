using Radiate.Client.Domain.Store.Models;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record SetTargetImageAction(Guid RunId, ImageEntity Image) : IAction;

public record SetDisplayImageDimensions(Guid RunId, int Width, int Height) : IAction;