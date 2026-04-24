namespace Everglow.Commons.Mechanics.Mission.Hooks;

public class WorldMissionGlobalNPC : GlobalNPC
{
	public static event Action<NPC> OnNPCKilled;

	public override bool SpecialOnKill(NPC npc)
	{
		OnNPCKilled?.Invoke(npc);

		return base.SpecialOnKill(npc);
	}
}