using System.Reflection;
using Everglow.Commons.Hooks;
using Everglow.Commons.Physics.DataStructures;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Everglow.Commons.CustomTiles;

public class PlayerCollider : ModPlayer, IEntityCollider<Player>
{
	public AABB Box => new(Player.position, new Vector2(Player.width, Player.height));

	public override bool CloneNewInstances => true;

	public float Gravity => Player.gravDir;

	public RigidEntity Ground { get; set; }

	public RigidEntity Grab { get; set; }

	public int GrabDir { get; set; }

	public override bool IsCloneable => true;

	public float OffsetY { get => Player.gfxOffY; set => Player.gfxOffY = value; }

	public Vector2 OldPosition { get; set; }

	public Vector2 Position
	{
		get => Player.position;
		set => Player.position = value;
	}

	public Vector2 Size => new(Player.width, Player.height);

	public Vector2 Velocity
	{
		get => Player.velocity;
		set => Player.velocity = value;
	}

	public static void Player_JumpMovement(On_Player.orig_JumpMovement orig, Player self)
	{
		var collider = self.GetModPlayer<PlayerCollider>();
		orig(self);
		if (self.jump != 0 && collider.jumpSpeed != 0)
		{
			self.velocity.Y = collider.jumpSpeed;
		}
		else
		{
			collider.jumpSpeed = 0;
		}
	}

	public override ModPlayer Clone(Player newEntity)
	{
		var clone = base.Clone(newEntity) as PlayerCollider;
		clone.OldPosition = newEntity.position;
		clone.Ground = null;
		clone.jumpSpeed = 0;
		return clone;
	}

	public void ForceJump()
	{
		jumpSpeed = Player.velocity.Y;
	}

	public bool Ignore(RigidEntity entity)
	{
		return false;
	}

	public override void Load()
	{
		On_Player.CanFitSpace += Player_CanFitSpace;
		On_Player.DryCollision += Player_DryCollision;
		On_Player.WaterCollision += Player_WaterCollision;
		On_Player.JumpMovement += Player_JumpMovement;
		On_Player.HoneyCollision += Player_HoneyCollision;
		IL_Player.WallslideMovement += Player_WallslideMovement_IL;
	}

	public void OnCollision(CollisionResult result)
	{
		if (Player.spikedBoots <= 0)
		{
			return;
		}
		if (1 - Math.Abs(result.Normal.X) < CollisionUtils.Epsilon)
		{
			Grab = result.Collider;
			GrabDir = Math.Sign(result.Normal.X);
		}
	}

	public void OnLeave()
	{
		if (-Entity.velocity.Y > Player.jumpSpeed)
		{
			ForceJump();
		}
	}

	private float jumpSpeed;

	private static bool Player_CanFitSpace(On_Player.orig_CanFitSpace orig, Player self, int heightBoost)
	{
		ColliderManager.EnableHook = false;
		bool flag = orig(self, heightBoost);
		ColliderManager.EnableHook = true;
		return flag;
	}

	private static void Player_DryCollision(On_Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!ColliderManager.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}
		ColliderManager.EnableHook = false;
		IEntityCollider<Player> player = self.GetModPlayer<PlayerCollider>();
		player.Prepare();
		orig(self, fallThrough, ignorePlats);
		player.Update();
		ColliderManager.EnableHook = true;
	}

	private static void Player_HoneyCollision(On_Player.orig_HoneyCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!ColliderManager.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}

		ColliderManager.EnableHook = false;
		IEntityCollider<Player> player = self.GetModPlayer<PlayerCollider>();
		player.Prepare();
		orig(self, fallThrough, ignorePlats);
		player.Update();
		ColliderManager.EnableHook = true;
	}

	private static void Player_WallslideMovement_IL(ILContext il)
	{
		var cursor = new ILCursor(il);
		cursor.Emit(OpCodes.Ldarg_0);
		cursor.EmitDelegate((Player player) =>
		{
			ColliderManager.EnableHook = false;
			var collider = player.GetModPlayer<PlayerCollider>();
			var position = player.position + new Vector2(collider.GrabDir, 0);
			if (collider.Grab != null)
			{
				if (collider.GrabDir != player.direction ||
					!collider.Grab.Intersect(new AABB(position, player.Size)))
				{
					collider.Grab = null;
				}
			}
		});
		var skipControlCheck = cursor.DefineLabel();
		var skipSetFlag = cursor.DefineLabel();
		if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStfld<Player>("sliding")))
		{
			throw new HookException("Player_WallslideMovement_IL");
		}

		cursor.Emit(OpCodes.Ldarg_0);
		cursor.EmitDelegate((Player player) =>
		{
			return player.GetModPlayer<PlayerCollider>().Grab != null;
		});
		cursor.Emit(OpCodes.Brfalse, skipSetFlag);

		cursor.Emit(OpCodes.Ldc_I4, 1);
		cursor.Emit(OpCodes.Stloc_0);
		cursor.Emit(OpCodes.Br, skipControlCheck);
		cursor.MarkLabel(skipSetFlag);

		if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStloc(0) && ins.Previous.MatchLdcI4(0)))
		{
			throw new HookException("Player_WallslideMovement_IL");
		}

		cursor.MarkLabel(skipControlCheck);

		cursor.Index = 0;
		MethodInfo setHook = typeof(ColliderManager).GetProperty(nameof(ColliderManager.EnableHook), BindingFlags.Static | BindingFlags.Public).SetMethod;
		while (cursor.TryGotoNext(MoveType.Before, ins => ins.MatchRet()))
		{
			cursor.Emit(OpCodes.Ldc_I4_1);
			cursor.Emit(OpCodes.Call, setHook);
			cursor.Index++;
		}
	}

	private static void Player_WaterCollision(On_Player.orig_WaterCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!ColliderManager.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}

		ColliderManager.EnableHook = false;
		IEntityCollider<Player> player = self.GetModPlayer<PlayerCollider>();
		player.Prepare();
		orig(self, fallThrough, ignorePlats);
		player.Update();
		ColliderManager.EnableHook = true;
	}
}