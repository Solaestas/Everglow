using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.MissionDetail;

public abstract class UIMissionDetailMaskContentBase<TMask> : UIBlock
	where TMask : UIMissionDetailMaskBase<TMask>, new()
{
	protected MissionBase _mission;

	public event Action<BaseElement> HideMask;

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
		HideMask?.Invoke(this);
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