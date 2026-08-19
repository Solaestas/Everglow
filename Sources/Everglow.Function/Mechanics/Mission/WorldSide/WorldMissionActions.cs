using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.Mission.WorldSide;

public sealed class WorldMissionActions
{
	private readonly WorldMissionManager _manager;

	public WorldMissionActions(WorldMissionManager manager)
	{
		_manager = manager ?? throw new ArgumentNullException(nameof(manager));
	}

	public static IReadOnlyList<MissionActionType> GetAvailableTypes(WorldMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		if (!NetUtils.IsSingle)
		{
			return [];
		}

		if (mission.State == WorldMissionState.Failed && mission.Retriable)
		{
			return [MissionActionType.Retry];
		}

		if (mission.State == WorldMissionState.Completed && !mission.RewardClaimed)
		{
			return [MissionActionType.ClaimReward];
		}

		return [];
	}

	public bool TryExecute(MissionAction action)
	{
		MissionIdentity identity = action.Mission;
		if (identity.Side != MissionSide.World
			|| !string.Equals(identity.DefinitionId, identity.InstanceId, StringComparison.Ordinal))
		{
			return false;
		}

		var mission = _manager.GetMission(identity.DefinitionId);
		if (mission is null
			|| !string.Equals(mission.Name, identity.InstanceId, StringComparison.Ordinal)
			|| MissionHintRules.HasContent(mission.Hint)
			|| !GetAvailableTypes(mission).Contains(action.Type))
		{
			return false;
		}

		bool applied = action.Type switch
		{
			MissionActionType.Retry => mission.RetryCore(),
			MissionActionType.ClaimReward => TryClaimReward(mission),
			_ => false,
		};

		if (!applied)
		{
			return false;
		}

		if (action.Type == MissionActionType.Retry)
		{
			_manager.OnMissionObjectiveUpdated(mission);
		}
		_manager.OnMissionStatusUpdated(mission);
		return true;
	}

	private static bool TryClaimReward(WorldMissionBase mission)
	{
		mission.GiveRewards();
		return mission.RewardClaimed;
	}
}
