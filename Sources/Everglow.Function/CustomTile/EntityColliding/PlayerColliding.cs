using Everglow.Commons.Hooks;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Everglow.Commons.CustomTile.EntityColliding;

public class PlayerColliding : ModPlayer
{
	public PlayerHandler handler;
	public override bool CloneNewInstances => true;
	public override bool IsCloneable => true;

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
		var clone = base.Clone(newEntity) as PlayerColliding;
		clone.handler = new PlayerHandler(newEntity);
		return clone;
	}

	private float jumpSpeed;
	private int jumpTime;

	public void Jump() => Jump(Player.jump, Player.velocity.Y);

	public void Jump(int time, float speed)
	{
		jumpTime = time;
		jumpSpeed = speed;
	}

	//阻止玩家自动吸附
	public void ResetAttach()
	{
		if (handler.attachDir != Direction.Bottom)
			return;

		Player.position.Y -= Player.gfxOffY;
		Player.gfxOffY = 0;
	}

	public static void Player_JumpMovement(On_Player.orig_JumpMovement orig, Player self)
	{
		var player = self.GetModPlayer<PlayerColliding>();
		if (player.jumpTime > 0)
		{
			if (self.jump != 0)
				self.jump = 0;
			if (!self.controlJump)
				player.jumpTime = 1;
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
		var player = self.GetModPlayer<PlayerColliding>();
		player.ResetAttach();
		player.handler.position = self.position;//记录位置，否则会把传送当成位移
		orig(self, fallThrough, ignorePlats);
		self.GetModPlayer<PlayerColliding>().handler.Update(ignorePlats || fallThrough);
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
		var player = self.GetModPlayer<PlayerColliding>();
		player.ResetAttach();
		player.handler.position = self.position;//记录位置，否则会把传送当成位移
		orig(self, fallThrough, ignorePlats);
		self.GetModPlayer<PlayerColliding>().handler.Update(ignorePlats || fallThrough);
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
		var player = self.GetModPlayer<PlayerColliding>();
		player.ResetAttach();
		player.handler.position = self.position;//记录位置，否则会把传送当成位移
		orig(self, fallThrough, ignorePlats);
		self.GetModPlayer<PlayerColliding>().handler.Update(ignorePlats || fallThrough);
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
			throw new HookException("Player_WallslideMovement_IL");

		cursor.Emit(OpCodes.Ldarg_0);
		cursor.EmitDelegate((Player player) =>
		{
			return player.GetModPlayer<PlayerColliding>().handler.attachType == AttachType.Grab;
		});
		cursor.Emit(OpCodes.Brfalse, skipSetFlag);

		cursor.Emit(OpCodes.Ldc_I4, 1);
		cursor.Emit(OpCodes.Stloc_0);
		cursor.Emit(OpCodes.Br, skipControlCheck);
		cursor.MarkLabel(skipSetFlag);

		if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchStloc(0) && ins.Previous.MatchLdcI4(0)))
			throw new HookException("Player_WallslideMovement_IL");
		cursor.MarkLabel(skipControlCheck);
	}

	private static void Player_WallslideMovement_On(On_Player.orig_WallslideMovement orig, Player self)
	{
		TileSystem.EnableCollisionHook = false;
		orig(self);
		TileSystem.EnableCollisionHook = true;
	}
}