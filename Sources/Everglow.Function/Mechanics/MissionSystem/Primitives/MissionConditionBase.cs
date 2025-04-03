namespace Everglow.Commons.Mechanics.MissionSystem.Primitives;

public abstract class MissionConditionBase
{
	public abstract bool Check(Player player, NPC npc);
}