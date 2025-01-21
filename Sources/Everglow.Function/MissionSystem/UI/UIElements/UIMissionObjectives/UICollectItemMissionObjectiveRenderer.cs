using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Templates.Abstracts;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.MissionSystem.UI.UIElements.UIMissionObjectives;

public class UICollectItemMissionObjectiveRenderer : IMissionObjectiveRenderer
{
	public BaseElement Parse(MissionBase mission)
	{
		var collectItemMission = (ICollectItemMission)mission;
		return new BaseElement();
	}
}
