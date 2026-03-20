using Everglow.Commons.Mechanics.MissionSystem.Enums;

namespace Everglow.Commons.Mechanics.MissionSystem.Core;

public class MissionManagerData
{
	private Dictionary<int, int> nPCKillCounter;
	private Dictionary<PoolType, List<MissionBase>> missionPools;

	public IReadOnlyDictionary<int, int> NPCKillCounter => nPCKillCounter;

	public IReadOnlyDictionary<PoolType, List<MissionBase>> MissionPools => missionPools;

	private MissionManagerData()
	{
	}

	public MissionManagerData(
		Dictionary<int, int> nPCKillCounter,
		Dictionary<PoolType, List<MissionBase>> missionPools)
	{
		this.nPCKillCounter = nPCKillCounter;
		this.missionPools = missionPools;
	}
}