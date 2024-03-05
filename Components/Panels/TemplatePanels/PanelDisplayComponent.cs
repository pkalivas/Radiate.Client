using Microsoft.AspNetCore.Components;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Schema;

namespace Radiate.Client.Components.Panels.TemplatePanels;

public abstract class PanelDisplayComponent<TPanel> : StoreComponent<TPanel>
    where TPanel : class, IPanel
{
    [CascadingParameter] public Guid RunId { get; set; } = Guid.Empty;
    [Parameter] public TPanel Panel { get; set; } = default!;
    
    protected bool CanDisplay() => Panel is not GridPanel.GridItem gridItem || gridItem.IsVisible;
    
    protected string TrackByKey() => Hash.Of(typeof(TPanel)).And(Panel).Value.ToString();

    protected string TrackByKey<T>(T item, int index) =>
        $"{TrackByKey()}_{Hash.Of(typeof(T)).And(item).Value.ToString()}_{index}";
}