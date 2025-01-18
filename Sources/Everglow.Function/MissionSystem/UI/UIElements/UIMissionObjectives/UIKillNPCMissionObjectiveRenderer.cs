using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.MissionSystem.UI.UIElements.UIMissionObjectives;

public class UIKillNPCMissionObjectiveRenderer : IMissionObjectiveRenderer
{
	public BaseElement Parse(MissionBase mission)
	{
		var killNPCMission = (IKillNPCMission)mission;
		return new BaseElement();
	}
}
