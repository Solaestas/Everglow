using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.Presentation.Adapters;

public static class WorldMissionActionAdapter
{
	public static IReadOnlyList<MissionAction> GetActions(WorldMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		if (MissionHintRules.HasContent(mission.Hint))
		{
			return [];
		}

		var identity = new MissionIdentity(MissionSide.World, mission.Name, mission.Name);
		return WorldMissionActions.GetAvailableTypes(mission)
			.Select(type => new MissionAction(identity, type))
			.ToArray();
	}
}
