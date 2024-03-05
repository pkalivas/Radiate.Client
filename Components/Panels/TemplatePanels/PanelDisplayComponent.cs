using Microsoft.AspNetCore.Components;
using Radiate.Client.Domain.Store.Selections;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Schema;

namespace Radiate.Client.Components.Panels.TemplatePanels;

public abstract class PanelDisplayComponent<TPanel> : StoreComponent<TPanel>
    where TPanel : class, IPanel
{
    [CascadingParameter] public Guid RunId { get; set; } = Guid.Empty;
    [Parameter] public Guid PanelId { get; set; } = Guid.Empty;

    protected override IObservable<TPanel> Select() => Store
        .Select(RunUiSelectors.SelectPanel<TPanel>(PanelId));
    
    protected bool CanDisplay() => Model is not GridPanel.GridItem gridItem || gridItem.IsVisible;
    
    protected string TrackByKey() => Hash.Of(typeof(TPanel)).And(Model).Value.ToString();

    protected string TrackByKey<T>(T item, int index) =>
        $"{TrackByKey()}_{Hash.Of(typeof(T)).And(item).Value.ToString()}_{index}";
}