using Everglow.Commons.Mechanics.Mission.PlayerSide.Enums;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Core;

public class MissionManagerData
{
	private Dictionary<int, int> nPCKillCounter;
	private Dictionary<PlayerMissionState, List<MissionBase>> missionPools;

	public IReadOnlyDictionary<int, int> NPCKillCounter => nPCKillCounter;

	public IReadOnlyDictionary<PlayerMissionState, List<MissionBase>> MissionPools => missionPools;

	private MissionManagerData()
	{
	}

	public MissionManagerData(
		Dictionary<int, int> nPCKillCounter,
		Dictionary<PlayerMissionState, List<MissionBase>> missionPools)
	{
		this.nPCKillCounter = nPCKillCounter;
		this.missionPools = missionPools;
	}
}