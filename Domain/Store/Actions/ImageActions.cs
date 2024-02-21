using Radiate.Client.Domain.Store.Models;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record SetTargetImageAction(Guid RunId, ImageEntity Image) : IAction;
