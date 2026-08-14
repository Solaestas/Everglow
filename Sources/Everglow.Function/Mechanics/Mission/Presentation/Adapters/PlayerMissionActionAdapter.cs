using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.Presentation.Adapters;

public static class PlayerMissionActionAdapter
{
	public static IReadOnlyList<MissionAction> GetActions(PlayerMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		if (MissionHintRules.HasContent(mission.Hint))
		{
			return [];
		}

		var identity = new MissionIdentity(MissionSide.Player, mission.Name, mission.InstanceId);
		return PlayerMissionActions.GetAvailableTypes(mission)
			.Select(type => new MissionAction(identity, type))
			.ToArray();
	}
}
