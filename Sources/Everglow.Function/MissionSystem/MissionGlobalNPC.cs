namespace Everglow.Commons.MissionSystem;

public class MissionGlobalNPC : GlobalNPC
{
	public static event Action<NPC> OnNPCKill;

	public static event Action<int, Chest, int> OnSetupShop;

	public override void OnKill(NPC npc)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			ClientOnKill(npc);
		}
		else if (Main.netMode == NetmodeID.Server)
		{
			ServerOnKill(npc);
		}
	}

	public void ClientOnKill(NPC npc)
	{
		MissionManager.Instance.CountKill(npc.type);

		// TODO: The line is not used by mission manager currently.
		OnNPCKill?.Invoke(npc);
	}

	public void ServerOnKill(NPC npc)
	{
		// TODO: Send packet to client to sync the kill of npc.
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		// TODO: Draws the exclamation mark on the NPC when they have a quest.
	}
}