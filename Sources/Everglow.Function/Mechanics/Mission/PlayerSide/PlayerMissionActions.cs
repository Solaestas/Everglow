using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide;

public static class PlayerMissionActions
{
	public static IReadOnlyList<MissionActionKind> GetAvailableKinds(PlayerMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		if (mission.State == PlayerMissionState.Available)
		{
			return [MissionActionKind.Accept];
		}

		if (mission.State == PlayerMissionState.Accepted
			&& mission.Cancellable
			&& !mission.CheckComplete())
		{
			return [MissionActionKind.Cancel];
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
			|| !GetAvailableKinds(mission).Contains(action.Kind))
		{
			return false;
		}

		switch (action.Kind)
		{
			case MissionActionKind.Accept:
				PlayerMissionManager.MoveMission(mission, PlayerMissionState.Available, PlayerMissionState.Accepted);
				break;
			case MissionActionKind.Cancel:
				PlayerMissionManager.MoveMission(mission, PlayerMissionState.Accepted, PlayerMissionState.Failed);
				break;
			default:
				return false;
		}

		PlayerMissionManager.NeedRefresh = true;
		return true;
	}
}
