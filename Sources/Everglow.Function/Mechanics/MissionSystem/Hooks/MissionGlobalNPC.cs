using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;

namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

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
		// TODO: Count kill for all player
	}

	public void ServerOnKill(NPC npc)
	{
		// TODO: Send packet to client to sync the kill of npc.
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var types = MissionManager.GetMissionPool(PoolType.Accepted).SelectMany(x => x.Objectives.AllObjectives)
			.SelectMany(x => x is KillNPCObjective kO ? kO.DemandNPCs.SelectMany(y => y.NPCs) : []);
		bool valid = types.Contains(npc.type);
		if (valid)
		{
			Texture2D tex = ModAsset.ExclamationMark.Value;
			float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
			spriteBatch.Draw(tex, new Vector2(npc.Center.X - 2, npc.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);
		}
	}
}