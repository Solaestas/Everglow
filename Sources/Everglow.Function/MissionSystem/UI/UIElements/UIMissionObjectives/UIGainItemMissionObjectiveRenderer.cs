using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.MissionSystem.UI.UIElements.UIMissionObjectives;

public class UIGainItemMissionObjectiveRenderer : IMissionObjectiveRenderer
{
	public BaseElement Parse(MissionBase mission)
	{
		var gainItemMission = (IGainItemMission)mission;
		return new BaseElement();
	}
}
