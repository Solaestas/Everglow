using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.MissionDetail;

public class UIMissionDetailMask : UIBlock
{
	private static readonly Color DefaultColor = new Color(0f, 0f, 0f, 0.8f);

	private UIBlock _container;
	private UIMissionDetailMaskContentBase _content;

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

	private void Show(UIMissionDetailMaskContentBase content)
	{
		Info.IsVisible = true;

		_content = content;
		_content.HideMask += Hide;
		_container.Register(_content);

		PanelColor = _content.BackgroundColor ?? DefaultColor;
	}

	public void Show<T>(MissionBase mission)
		where T : UIMissionDetailMaskContentBase, new()
	{
		var content = new T();
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

	public abstract class UIMissionDetailMaskContentBase : UIBlock
	{
		protected MissionBase _mission;

		public event Action HideMask;

		public virtual Color? BackgroundColor => null;

		public override void OnInitialization()
		{
			Info.Width.SetFull();
			Info.Height.SetFull();
			Info.SetMargin(0);
			PanelColor = Color.Transparent;
			BorderWidth = 0;
		}

		protected void Hide(BaseElement _)
		{
			HideMask?.Invoke();
		}

		public virtual void SetMission(MissionBase mission)
		{
			_mission = mission;
		}

		public virtual void Reset()
		{
			_mission = null;
		}
	}
}