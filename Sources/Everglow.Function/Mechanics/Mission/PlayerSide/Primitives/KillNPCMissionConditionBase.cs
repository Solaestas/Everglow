namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Primitives;

public abstract class KillNPCMissionConditionBase
{
	public abstract bool Check(Player player, NPC npc);
}