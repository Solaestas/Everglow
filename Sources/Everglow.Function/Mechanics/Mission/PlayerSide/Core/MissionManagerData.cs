namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Core;

public class MissionManagerData
{
	private Dictionary<int, int> nPCKillCounter;
	private List<MissionBase> missionPools;

	public IReadOnlyDictionary<int, int> NPCKillCounter => nPCKillCounter;

	public IReadOnlyList<MissionBase> MissionPools => missionPools;

	private MissionManagerData()
	{
	}

	public MissionManagerData(
		Dictionary<int, int> nPCKillCounter,
		List<MissionBase> missionPools)
	{
		this.nPCKillCounter = nPCKillCounter;
		this.missionPools = missionPools;
	}
}