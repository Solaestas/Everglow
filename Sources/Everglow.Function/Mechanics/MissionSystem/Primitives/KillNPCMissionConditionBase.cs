namespace Everglow.Commons.Mechanics.MissionSystem.Primitives;

public abstract class KillNPCMissionConditionBase
{
	public abstract bool Check(Player player, NPC npc);
}