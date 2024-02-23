namespace Radiate.Client.Domain.Templates.Panels;

public interface IPanel
{
    Guid Id { get; }
    string Title { get; }
}

public abstract class Panel : IPanel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
}
