using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Templates.Abstracts;
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
