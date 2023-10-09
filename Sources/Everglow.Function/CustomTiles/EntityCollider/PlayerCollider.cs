using Everglow.Commons.CustomTiles.Tiles;
using Everglow.Commons.Hooks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.DataStructures;

namespace Everglow.Commons.CustomTiles.EntityCollider;

public class PlayerCollider : ModPlayer, IEntityCollider
{
	public override bool CloneNewInstances => true;
	public override bool IsCloneable => true;

	public Direction AttachDir { get; set; }
	public RigidEntity AttachTile { get; set; }
	public AttachType AttachType { get; set; }

	public bool CanAttach => Player.grapCount == 0 && !(Player.mount.Active && Player.mount.CanFly());

	Entity IEntityCollider.Entity => Entity;

	public Vector2 DeltaVelocity { get; set; }

	public Direction Ground => Player.gravDir > 0 ? Direction.Down : Direction.Up;

	public Vector2 Position { get; set; }
	public Vector2 AbsoluteVelocity { get; set; }

	public override void Load()
	{
		On_Player.CanFitSpace += Player_CanFitSpace;
		On_Player.DryCollision += Player_DryCollision;
		On_Player.WaterCollision += Player_WaterCollision;
		On_Player.JumpMovement += Player_JumpMovement;
		On_Player.HoneyCollision += Player_HoneyCollision;
		On_Player.WallslideMovement += Player_WallslideMovement_On;
		IL_Player.WallslideMovement += Player_WallslideMovement_IL;
	}

	public override ModPlayer Clone(Player newEntity)
	{
		var clone = base.Clone(newEntity) as PlayerCollider;
		clone.AttachTile = null;
		clone.AttachDir = Direction.None;
		clone.AttachType = AttachType.None;
		clone.Position = newEntity.position;
		clone.AbsoluteVelocity = newEntity.velocity;
		clone.DeltaVelocity = Vector2.Zero;
		return clone;
	}

	private float jumpSpeed;
	private int jumpTime;

	public void Jump()
	{
		Jump(Player.jump, Player.velocity.Y);
	}

	public void Jump(int time, float speed)
	{
		jumpTime = time;
		jumpSpeed = speed;
	}

	//阻止玩家自动吸附
	public void ResetAttach()
	{
		if (AttachDir != Direction.Down)
		{
			return;
		}

		Player.position.Y -= Player.gfxOffY;
		Player.gfxOffY = 0;
	}

	public static void Player_JumpMovement(On_Player.orig_JumpMovement orig, Player self)
	{
		var player = self.GetModPlayer<PlayerCollider>();
		if (player.jumpTime > 0)
		{
			if (self.jump != 0)
			{
				self.jump = 0;
			}

			if (!self.controlJump)
			{
				player.jumpTime = 1;
			}

			self.velocity.Y = player.jumpSpeed;
			player.jumpTime--;
		}
		orig(self);
	}

	private static void Player_DryCollision(On_Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!TileSystem.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}
		TileSystem.EnableCollisionHook = false;
		var player = self.GetModPlayer<PlayerCollider>();
		player.ResetAttach();
		player.Position = self.position;
		orig(self, fallThrough, ignorePlats);
		IEntityCollider.Update(player, ignorePlats || fallThrough);
		TileSystem.EnableCollisionHook = true;
	}

	private static void Player_WaterCollision(On_Player.orig_WaterCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!TileSystem.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}

		TileSystem.EnableCollisionHook = false;
		var player = self.GetModPlayer<PlayerCollider>();
		player.ResetAttach();
		player.Position = self.position;
		orig(self, fallThrough, ignorePlats);
		IEntityCollider.Update(player, ignorePlats || fallThrough);
		TileSystem.EnableCollisionHook = true;
	}

	private static void Player_HoneyCollision(On_Player.orig_HoneyCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!TileSystem.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}

		TileSystem.EnableCollisionHook = false;
		var player = self.GetModPlayer<PlayerCollider>();
		player.ResetAttach();
		player.Position = self.position;
		orig(self, fallThrough, ignorePlats);
		IEntityCollider.Update(player, ignorePlats || fallThrough);
		TileSystem.EnableCollisionHook = true;
	}

	private static bool Player_CanFitSpace(On_Player.orig_CanFitSpace orig, Player self, int heightBoost)
	{
		TileSystem.EnableCollisionHook = false;
		bool flag = orig(self, heightBoost);
		TileSystem.EnableCollisionHook = true;
		return flag;
	}

	private static void Player_WallslideMovement_IL(ILContext il)
	{
		var cursor = new ILCursor(il);
		var skipControlCheck = cursor.DefineLabel();
		var skipSetFlag = cursor.DefineLabel();
		if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStfld<Player>("sliding")))
		{
			throw new HookException("Player_WallslideMovement_IL");
		}

		cursor.Emit(OpCodes.Ldarg_0);
		cursor.EmitDelegate((Player player) =>
		{
			return player.GetModPlayer<PlayerCollider>().AttachType == AttachType.Grab;
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
	}

	private static void Player_WallslideMovement_On(On_Player.orig_WallslideMovement orig, Player self)
	{
		TileSystem.EnableCollisionHook = false;
		orig(self);
		TileSystem.EnableCollisionHook = true;
	}

	public void OnAttach()
	{
		if (AttachType == AttachType.Stand)
		{
			Entity.velocity.Y = 0;
		}
		else if (AttachType == AttachType.Grab)
		{
			Entity.velocity.Y = TileSystem.AirSpeed;
		}
	}

	public void OnCollision(RigidEntity tile, Direction dir, ref RigidEntity newAttach)
	{
		if (dir == Direction.In)
		{
			Player.Hurt(PlayerDeathReason.ByCustomReason("Inside Tile"), 10, 0);
		}

		if (tile.IsGrabbable && dir.IsH())
		{
			var player = Entity;
			player.slideDir = (int)player.GetControlDirectionH().ToVector2().X;
			if (player.slideDir == 0 || player.spikedBoots <= 0 || player.mount.Active ||
				((!player.controlLeft || player.slideDir != -1 || dir != Direction.Left) &&
				(!player.controlRight || player.slideDir != 1 || dir != Direction.Right)))
			{
				return;
			}
			newAttach = tile;
			AttachDir = dir;
			AttachType = AttachType.Grab;
		}
	}

	public void OnUpdate()
	{
		if (AttachTile is not null)
		{
			Entity.position += new Vector2(0, Entity.gfxOffY);
			Entity.gfxOffY = 0;
		}
	}
}