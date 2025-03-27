using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.MissionDetail;

public abstract class UIMissionDetailMaskBase<TMask> : UIBlock
	where TMask : UIMissionDetailMaskBase<TMask>, new()
{
	private static readonly Color DefaultColor = new Color(0f, 0f, 0f, 0.8f);

	private UIBlock _container;
	private UIMissionDetailMaskContentBase<TMask> _content;

	public override void OnInitialization()
	{
		base.OnInitialization();

		PanelColor = DefaultColor;

		_container = new UIBlock();
		_container.Info.Width.SetFull();
		_container.Info.Height.SetFull();
		_container.PanelColor = Color.Transparent;
		_container.BorderColor = Color.Transparent;
		_container.Info.SetMargin(0);
		Register(_container);
	}

	public void Show(UIMissionDetailMaskContentBase<TMask> content)
	{
		Info.IsVisible = true;

		_content = content;
		_content.HideMask += Hide;
		_container.Register(_content);

		PanelColor = _content.BackgroundColor ?? DefaultColor;
	}

	public void Show<TContent>(MissionBase mission)
		where TContent : UIMissionDetailMaskContentBase<TMask>, new()
	{
		var content = new TContent();
		content.SetMission(mission);
		Show(content);
	}

	private void Hide()
	{
		Info.IsVisible = false;

		_container.Remove(_content);
		_content.HideMask -= Hide;
		_content.Reset();
		_content = null;

		PanelColor = DefaultColor;
	}
}