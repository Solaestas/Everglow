using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide;

public sealed class PlayerMissionActions
{
	public static PlayerMissionActions Instance => ModContent.GetInstance<PlayerMissionSystem>().Actions;

	private readonly PlayerMissionManager _manager;

	public PlayerMissionActions(PlayerMissionManager manager)
	{
		_manager = manager ?? throw new ArgumentNullException(nameof(manager));
	}

	public static IReadOnlyList<MissionActionType> GetAvailableTypes(PlayerMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		if (mission.State == PlayerMissionState.Available)
		{
			return [MissionActionType.Accept];
		}

		if (mission.State == PlayerMissionState.Accepted
			&& mission.CheckComplete())
		{
			return [MissionActionType.Submit];
		}

		if (mission.State == PlayerMissionState.Accepted
			&& mission.Cancellable)
		{
			return [MissionActionType.Cancel];
		}

		return [];
	}

	public bool TryExecute(MissionAction action)
	{
		MissionIdentity identity = action.Mission;
		if (identity.Side != MissionSide.Player)
		{
			return false;
		}

		var mission = _manager.GetMission(identity.DefinitionId);
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
				_manager.ChangeMissionState(mission, PlayerMissionState.Available, PlayerMissionState.Accepted);
				break;
			case MissionActionType.Cancel:
				_manager.ChangeMissionState(mission, PlayerMissionState.Accepted, PlayerMissionState.Failed);
				break;
			case MissionActionType.Submit:
				mission.OnComplete();
				return mission.State == PlayerMissionState.Completed;
			default:
				return false;
		}

		return true;
	}
}
