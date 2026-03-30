namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Primitives;

public abstract class KillNPCMissionConditionBase
{
	public abstract bool Check(Player player, NPC npc);
}