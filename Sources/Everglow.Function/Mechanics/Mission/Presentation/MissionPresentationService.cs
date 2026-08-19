using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public sealed class MissionPresentationService
{
	private readonly PlayerMissionManager _playerManager;
	private readonly PlayerMissionActions _playerActions;
	private readonly WorldMissionManager _worldManager;
	private readonly WorldMissionActions _worldActions;

	public MissionPresentationService(
		PlayerMissionManager playerManager,
		PlayerMissionActions playerActions,
		WorldMissionManager worldManager,
		WorldMissionActions worldActions)
	{
		ArgumentNullException.ThrowIfNull(playerManager);
		ArgumentNullException.ThrowIfNull(playerActions);
		ArgumentNullException.ThrowIfNull(worldManager);
		ArgumentNullException.ThrowIfNull(worldActions);

		_playerManager = playerManager;
		_playerActions = playerActions;
		_worldManager = worldManager;
		_worldActions = worldActions;
	}

	public IReadOnlyList<MissionPresentationEntry> GetAll()
	{
		var entries = new List<MissionPresentationEntry>(_playerManager.Missions.Count + _worldManager.Missions.Count);
		foreach (PlayerMissionBase mission in _playerManager.Missions)
		{
			entries.Add(CreateEntry(mission));
		}
		foreach (WorldMissionBase mission in _worldManager.Missions)
		{
			entries.Add(CreateEntry(mission));
		}
		return entries.ToArray();
	}

	public bool TryGet(MissionIdentity identity, out MissionPresentationEntry entry)
	{
		entry = null;
		return identity.Side switch
		{
			MissionSide.Player => TryGetPlayer(identity, out entry),
			MissionSide.World => TryGetWorld(identity, out entry),
			_ => false,
		};
	}

	public bool TryExecute(MissionAction action) => action.Mission.Side switch
	{
		MissionSide.Player => _playerActions.TryExecute(action),
		MissionSide.World => _worldActions.TryExecute(action),
		_ => false,
	};

	private static MissionPresentationEntry CreateEntry(PlayerMissionBase mission) => new(
		PlayerMissionViewAdapter.Create(mission),
		PlayerMissionActionAdapter.GetActions(mission).ToArray());

	private static MissionPresentationEntry CreateEntry(WorldMissionBase mission) => new(
		WorldMissionViewAdapter.Create(mission),
		WorldMissionActionAdapter.GetActions(mission).ToArray());

	private bool TryGetPlayer(MissionIdentity identity, out MissionPresentationEntry entry)
	{
		entry = null;
		PlayerMissionBase mission = _playerManager.GetMission(identity.DefinitionId);
		if (mission is null
			|| !string.Equals(mission.InstanceId, identity.InstanceId, StringComparison.Ordinal))
		{
			return false;
		}

		entry = CreateEntry(mission);
		return true;
	}

	private bool TryGetWorld(MissionIdentity identity, out MissionPresentationEntry entry)
	{
		entry = null;
		if (!string.Equals(identity.DefinitionId, identity.InstanceId, StringComparison.Ordinal))
		{
			return false;
		}

		WorldMissionBase mission = _worldManager.GetMission(identity.DefinitionId);
		if (mission is null)
		{
			return false;
		}

		entry = CreateEntry(mission);
		return true;
	}
}
