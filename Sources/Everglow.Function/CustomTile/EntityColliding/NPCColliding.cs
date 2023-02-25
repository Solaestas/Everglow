using Everglow.Commons.CustomTile;

namespace Everglow.Commons.CustomTile.EntityColliding;

public class NPCColliding : GlobalNPC
{
	public NPCHandler handler;
	public override bool InstancePerEntity => true;
	public override bool CloneNewInstances => true;
	public override bool IsCloneable => true;

	public override GlobalNPC Clone(NPC from, NPC to)
	{
		var clone = base.Clone(from, to) as NPCColliding;
		clone.handler = new NPCHandler(to);
		return clone;
	}

	public bool fall;

	public override void Load()
	{
		On_NPC.Collision_MoveWhileDry += NPC_Collision_MoveWhileDry;
		On_NPC.Collision_MoveWhileWet += NPC_Collision_MoveWhileWet;
		On_NPC.ApplyTileCollision += NPC_ApplyTileCollision;
	}

	private static void NPC_ApplyTileCollision(On_NPC.orig_ApplyTileCollision orig, NPC self, bool fall, Vector2 cPosition, int cWidth, int cHeight)
	{
		TileSystem.EnableCollisionHook = false;
		self.GetGlobalNPC<NPCColliding>().fall = fall;
		orig(self, fall, cPosition, cWidth, cHeight);
		TileSystem.EnableCollisionHook = true;
	}

	private static void NPC_Collision_MoveWhileWet(On_NPC.orig_Collision_MoveWhileWet orig, NPC self, Vector2 oldDryVelocity, float Slowdown)
	{
		if (!TileSystem.Enable || self.noTileCollide)
		{
			orig(self, oldDryVelocity, Slowdown);
			return;
		}

		TileSystem.EnableCollisionHook = false;
		var npc = self.GetGlobalNPC<NPCColliding>();
		npc.handler.position = self.position;
		orig(self, oldDryVelocity, Slowdown);
		npc.handler.Update(npc.fall);
		TileSystem.EnableCollisionHook = true;
	}

	private static void NPC_Collision_MoveWhileDry(On_NPC.orig_Collision_MoveWhileDry orig, NPC self)
	{
		if (!TileSystem.Enable || self.noTileCollide)
		{
			orig(self);
			return;
		}

		TileSystem.EnableCollisionHook = false;
		var npc = self.GetGlobalNPC<NPCColliding>();
		npc.handler.position = self.position;
		orig(self);
		npc.handler.Update(npc.fall);
		TileSystem.EnableCollisionHook = true;
	}
}