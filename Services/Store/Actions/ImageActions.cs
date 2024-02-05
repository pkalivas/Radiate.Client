using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store.Actions;

public record SetTargetImageAction(Guid RunId, ImageEntity Image) : IAction;