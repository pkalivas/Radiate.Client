namespace Radiate.Client.Domain.Templates.Panels;

public interface IPanel
{
    Guid Id { get; }
    string Title { get; }
    List<string> Actions { get; }
}

public abstract class Panel : IPanel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public List<string> Actions { get; set; } = new();
}
