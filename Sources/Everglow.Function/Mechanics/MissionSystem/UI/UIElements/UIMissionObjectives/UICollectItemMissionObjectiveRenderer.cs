using Everglow.Commons.Mechanics.MissionSystem.Abstracts;
using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Templates.Abstracts;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.UIMissionObjectives;

public class UICollectItemMissionObjectiveRenderer : IMissionObjectiveRenderer
{
	public BaseElement Parse(MissionBase mission)
	{
		var collectItemMission = (ICollectItemMission)mission;
		return new BaseElement();
	}
}
