using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide;

public class PlayerMissionManagerData
{
	private Dictionary<int, int> nPCKillCounter;
	private List<PlayerMissionBase> missions;

	public IReadOnlyDictionary<int, int> NPCKillCounter => nPCKillCounter;

	public IReadOnlyList<PlayerMissionBase> Missions => missions;

	private PlayerMissionManagerData()
	{
	}

	public PlayerMissionManagerData(
		Dictionary<int, int> nPCKillCounter,
		List<PlayerMissionBase> missions)
	{
		this.nPCKillCounter = nPCKillCounter;
		this.missions = missions;
	}
}
