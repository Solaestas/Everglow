#if false
using Everglow.Commons.Collider;

namespace Everglow.Commons.CustomTiles.EntityCollider;

public class NPCCollider : GlobalNPC, IBox
{
	private bool fall;
	private NPC npc;

	public Vector2 AbsoluteVelocity { get; set; }

	public Direction AttachDir { get; set; }

	public RigidEntity AttachTile { get; set; }

	public AttachType AttachType { get; set; }

	public bool CanAttach => true;

	public override bool CloneNewInstances => true;

	public Vector2 DeltaVelocity { get; set; }

	public Entity Entity => npc;

	public Direction Ground => Direction.Down;

	public override bool InstancePerEntity => true;

	public override bool IsCloneable => true;

	public Vector2 Position { get; set; }

	public override GlobalNPC Clone(NPC from, NPC to)
	{
		var clone = base.Clone(from, to) as NPCCollider;
		clone.npc = to;
		clone.AttachTile = null;
		clone.AttachDir = Direction.None;
		clone.AttachType = AttachType.None;
		clone.Position = to.position;
		clone.AbsoluteVelocity = to.velocity;
		clone.DeltaVelocity = Vector2.Zero;
		return clone;
	}

	public override void Load()
	{
		On_NPC.Collision_MoveWhileDry += NPC_Collision_MoveWhileDry;
		On_NPC.Collision_MoveWhileWet += NPC_Collision_MoveWhileWet;
		On_NPC.ApplyTileCollision += NPC_ApplyTileCollision;
	}

	public void OnAttach()
	{
		npc.velocity.Y = 0;
	}

	public void OnCollision(RigidEntity tile, Direction dir, ref RigidEntity newAttach)
	{
		if (dir == Direction.In && !npc.boss)
		{
			npc.StrikeNPC(10, 0, 0);
		}
	}

	public void OnUpdate()
	{
		if (AttachTile is not null)
		{
			npc.position += new Vector2(0, npc.gfxOffY);
			npc.gfxOffY = 0;
		}
	}

	private static void NPC_ApplyTileCollision(On_NPC.orig_ApplyTileCollision orig, NPC self, bool fall, Vector2 cPosition, int cWidth, int cHeight)
	{
		ColliderManager.EnableHook = false;
		self.GetGlobalNPC<NPCCollider>().fall = fall;
		orig(self, fall, cPosition, cWidth, cHeight);
		ColliderManager.EnableHook = true;
	}

	private static void NPC_Collision_MoveWhileDry(On_NPC.orig_Collision_MoveWhileDry orig, NPC self)
	{
		if (!ColliderManager.Enable || self.noTileCollide)
		{
			orig(self);
			return;
		}

		ColliderManager.EnableHook = false;
		var npc = self.GetGlobalNPC<NPCCollider>();
		npc.Position = self.position;
		orig(self);
		IBox.Update(npc, npc.fall);
		ColliderManager.EnableHook = true;
	}

	private static void NPC_Collision_MoveWhileWet(On_NPC.orig_Collision_MoveWhileWet orig, NPC self, Vector2 oldDryVelocity, float Slowdown)
	{
		if (!ColliderManager.Enable || self.noTileCollide)
		{
			orig(self, oldDryVelocity, Slowdown);
			return;
		}

		ColliderManager.EnableHook = false;
		var npc = self.GetGlobalNPC<NPCCollider>();
		npc.Position = self.position;
		orig(self, oldDryVelocity, Slowdown);
		IBox.Update(npc, npc.fall);
		ColliderManager.EnableHook = true;
	}
}
#endif