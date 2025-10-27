namespace Everglow.Commons.CustomTiles;

public class NPCCollider : GlobalNPC, IEntityCollider<NPC>
{
	public AABB Box => new AABB(Entity.position, Entity.width, Entity.height);

	public override bool CloneNewInstances => true;

	public NPC Entity { get; set; }

	public float Gravity => 1;

	public RigidEntity Ground { get; set; }

	public override bool InstancePerEntity => true;

	public override bool IsCloneable => true;

	public float OffsetY { get => Entity.gfxOffY; set => Entity.gfxOffY = value; }

	public Vector2 OldPosition { get; set; }

	public Vector2 Position { get => Entity.position; set => Entity.position = value; }

	public Vector2 Size => new Vector2(Entity.width, Entity.height);

	public Vector2 Velocity { get => Entity.velocity; set => Entity.velocity = value; }

	public override GlobalNPC Clone(NPC from, NPC to)
	{
		var clone = base.Clone(from, to) as NPCCollider;
		clone.Entity = to;
		clone.OldPosition = to.position;
		clone.Ground = null;
		return clone;
	}

	public bool Ignore(RigidEntity entity)
	{
		return false;
	}

	public override void Load()
	{
		On_NPC.Collision_MoveWhileDry += NPC_Collision_MoveWhileDry;
		On_NPC.Collision_MoveWhileWet += NPC_Collision_MoveWhileWet;
		On_NPC.ApplyTileCollision += NPC_ApplyTileCollision;
	}

	public void OnCollision(CollisionResult result)
	{
	}

	public void OnLeave()
	{
	}

	private bool fall;

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
		IEntityCollider<NPC> npc = self.GetGlobalNPC<NPCCollider>();
		npc.Prepare();
		orig(self);
		npc.Update();
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
		IEntityCollider<NPC> npc = self.GetGlobalNPC<NPCCollider>();
		npc.Prepare();
		orig(self, oldDryVelocity, Slowdown);
		npc.Update();
		ColliderManager.EnableHook = true;
	}
}