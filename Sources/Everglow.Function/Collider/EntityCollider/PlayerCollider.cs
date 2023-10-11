using Everglow.Commons.Hooks;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Everglow.Commons.Collider.EntityCollider;

public class PlayerCollider : ModPlayer, IEntityCollider<Player>
{
	public override bool CloneNewInstances => true;

	public override bool IsCloneable => true;

	public Vector2 Size => new(Player.width, Player.height);

	public float Gravity => Player.gravDir;

	public AABB Box => new(Player.position, new Vector2(Player.width, Player.height));

	public int Quantity => 1;

	public RigidEntity Ground { get; set; }

	private Vector2 oldPosition;

	public Vector2 Velocity
	{
		get => Player.velocity;
		set => Player.velocity = value;
	}

	public Vector2 Position
	{
		get => Player.position;
		set => Player.position = value;
	}

	public float OffsetY { get => Player.gfxOffY; set => Player.gfxOffY = value; }

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
		clone.Position = newEntity.position;
		return clone;
	}

	private float jumpSpeed;

	public void ForceJump()
	{
		jumpSpeed = Player.velocity.Y;
	}

	public static void Player_JumpMovement(On_Player.orig_JumpMovement orig, Player self)
	{
		var collider = self.GetModPlayer<PlayerCollider>();
		orig(self);
		if(self.jump != 0 && collider.jumpSpeed != 0)
		{
			self.velocity.Y = collider.jumpSpeed;
		}
		else
		{
			collider.jumpSpeed = 0;
		}
	}

	private void Prepare()
	{
		if(Ground != null && OffsetY != 0)
		{
			Player.position.Y += OffsetY;
			OffsetY = 0;
		}
		oldPosition = Player.position;
	}

	private const float Sqrt2Div2 = 0.707106781186f;

	private void Update()
	{
		var stride = Player.position - oldPosition;
		if (Ground != null)
		{
			var acc = Ground.StandAccelerate(this);
			if (stride.Y * Gravity < 0)
			{
				Player.velocity += acc;
				if (-Player.velocity.Y > Player.jumpSpeed)
				{
					ForceJump();
				}
			}
			stride += acc;
			Ground = null;
		}
		Player.position = oldPosition;
		foreach (var result in ColliderManager.Instance.Move(this, stride))
		{
			if (Vector2.Dot(result.Normal, Gravity * Vector2.UnitY) > Sqrt2Div2)
			{
				Ground = result.Collider;
				Player.velocity.Y = 0;
			}
			else if (result.Normal == new Vector2(0, -1))
			{
				Player.velocity.Y = -0.001f;
			}
		}
		Main.NewText($"{Player.velocity.Y},{Player.gravity}");
	}

	private static void Player_DryCollision(On_Player.orig_DryCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!ColliderManager.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}
		ColliderManager.EnableHook = false;
		var player = self.GetModPlayer<PlayerCollider>();
		player.Prepare();
		orig(self, fallThrough, ignorePlats);
		player.Update();
		ColliderManager.EnableHook = true;
	}

	private static void Player_WaterCollision(On_Player.orig_WaterCollision orig, Player self, bool fallThrough, bool ignorePlats)
	{
		if (!ColliderManager.Enable || self.ghost)
		{
			orig(self, fallThrough, ignorePlats);
			return;
		}

		ColliderManager.EnableHook = false;
		var player = self.GetModPlayer<PlayerCollider>();
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
		var player = self.GetModPlayer<PlayerCollider>();
		player.Prepare();
		orig(self, fallThrough, ignorePlats);
		player.Update();
		ColliderManager.EnableHook = true;
	}

	private static bool Player_CanFitSpace(On_Player.orig_CanFitSpace orig, Player self, int heightBoost)
	{
		ColliderManager.EnableHook = false;
		bool flag = orig(self, heightBoost);
		ColliderManager.EnableHook = true;
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
			return false;
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
		ColliderManager.EnableHook = false;
		orig(self);
		ColliderManager.EnableHook = true;
	}

	public bool Ignore(RigidEntity entity)
	{
		return false;
	}

	//public void OnCollision(RigidEntity tile, Direction dir, ref RigidEntity newAttach)
	//{
	//	if (dir == Direction.In)
	//	{
	//		Player.Hurt(PlayerDeathReason.ByCustomReason("Inside Tile"), 10, 0);
	//	}

	//	if (tile.IsGrabbable && dir.IsH())
	//	{
	//		var player = Entity;
	//		player.slideDir = (int)player.GetControlDirectionH().ToVector2().X;
	//		if (player.slideDir == 0 || player.spikedBoots <= 0 || player.mount.Active ||
	//			(!player.controlLeft || player.slideDir != -1 || dir != Direction.Left) &&
	//			(!player.controlRight || player.slideDir != 1 || dir != Direction.Right))
	//		{
	//			return;
	//		}
	//		newAttach = tile;
	//		AttachDir = dir;
	//		AttachType = AttachType.Grab;
	//	}
	//}
}