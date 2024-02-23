using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates.Panels;

namespace Radiate.Client.Domain.Templates;

public interface IRunTemplate
{
    public Guid Id { get; }
    public string ModelType { get; }
    public IRunUITemplate UI { get; }
}

public interface IRunUITemplate
{
    List<IPanel> Panels { get; }
}