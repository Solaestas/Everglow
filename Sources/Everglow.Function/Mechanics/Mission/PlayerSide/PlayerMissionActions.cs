using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide;

public static class PlayerMissionActions
{
	public static IReadOnlyList<MissionActionType> GetAvailableTypes(PlayerMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		if (mission.State == PlayerMissionState.Available)
		{
			return [MissionActionType.Accept];
		}

		if (mission.State == PlayerMissionState.Accepted
			&& mission.Cancellable
			&& !mission.CheckComplete())
		{
			return [MissionActionType.Cancel];
		}

		return [];
	}

	public static bool TryExecute(MissionAction action)
	{
		MissionIdentity identity = action.Mission;
		if (identity.Side != MissionSide.Player)
		{
			return false;
		}

		var mission = PlayerMissionManager.GetMission(identity.DefinitionId);
		if (mission is null
			|| !string.Equals(mission.InstanceId, identity.InstanceId, StringComparison.Ordinal)
			|| MissionHintRules.HasContent(mission.Hint)
			|| !GetAvailableTypes(mission).Contains(action.Type))
		{
			return false;
		}

		switch (action.Type)
		{
			case MissionActionType.Accept:
				PlayerMissionManager.MoveMission(mission, PlayerMissionState.Available, PlayerMissionState.Accepted);
				break;
			case MissionActionType.Cancel:
				PlayerMissionManager.MoveMission(mission, PlayerMissionState.Accepted, PlayerMissionState.Failed);
				break;
			default:
				return false;
		}

		PlayerMissionManager.NeedRefresh = true;
		return true;
	}
}
