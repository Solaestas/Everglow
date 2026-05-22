namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Core;

public class MissionManagerData
{
	private Dictionary<int, int> nPCKillCounter;
	private List<PlayerMissionBase> missionPools;

	public IReadOnlyDictionary<int, int> NPCKillCounter => nPCKillCounter;

	public IReadOnlyList<PlayerMissionBase> MissionPools => missionPools;

	private MissionManagerData()
	{
	}

	public MissionManagerData(
		Dictionary<int, int> nPCKillCounter,
		List<PlayerMissionBase> missionPools)
	{
		this.nPCKillCounter = nPCKillCounter;
		this.missionPools = missionPools;
	}
}