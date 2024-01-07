using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.CustomTiles.Tiles;
using Everglow.Commons.Hooks;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Everglow.Commons.CustomTiles.EntityColliding;

[Autoload(false)]
public class ProjColliding : GlobalProjectile
{
	public const int HookAiStyle = 7;

	public static readonly HashSet<Projectile> callFromHook = new();

	public ProjHandler handler;

	public override bool CloneNewInstances => true;

	public override bool InstancePerEntity => true;

	public override bool IsCloneable => true;

	public override GlobalProjectile Clone(Projectile from, Projectile to)
	{
		var clone = base.Clone(from, to) as ProjColliding;
		clone.handler = new ProjHandler(to);
		return clone;
	}

	public override void Load()
	{
		On_Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks_On;
		On_Projectile.HandleMovement += Projectile_HandleMovement;
		IL_Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks_IL;
	}

	private static void Projectile_AI_007_GrapplingHooks_IL(ILContext il)
	{
		var cursor = new ILCursor(il);
		if (!cursor.TryGotoNext(MoveType.After, ins => ins.Offset == 0x0e65))
			throw new HookException();
		Debug.Assert(cursor.Prev.MatchStloc(44));
		cursor.Emit(OpCodes.Ldarg_0);
		cursor.EmitDelegate((Projectile proj) => callFromHook.Contains(proj));
		var label = il.DefineLabel();
		cursor.Emit(OpCodes.Brtrue, label);
		if (!cursor.TryGotoNext(MoveType.Before, ins => ins.Offset == 0x0E91))
			throw new HookException();
		Debug.Assert(cursor.Next.MatchRet());
		cursor.MarkLabel(label);
	}

	private static void Projectile_AI_007_GrapplingHooks_On(On_Projectile.orig_AI_007_GrapplingHooks orig, Projectile self)
	{
		if (!TileSystem.Enable)
		{
			orig(self);
			return;
		}

		Player player = Main.player[self.owner];
		int numHooks = 3;
		if (self.type == 165)
		{
			numHooks = 8;
		}
		else if (self.type == 256)
		{
			numHooks = 2;
		}
		else if (self.type == 372)
		{
			numHooks = 2;
		}
		else if (self.type == 652)
		{
			numHooks = 1;
		}
		else if (self.type is >= 646 and <= 649)
		{
			numHooks = 4;
		}

		int grapCount = 0;
		int leastTime = int.MaxValue;
		int leastIndex = self.whoAmI;
		foreach (var proj in Main.projectile)
		{
			if (proj.active && proj.aiStyle == 7 && proj.owner == self.owner)
			{
				grapCount++;
				if (proj.timeLeft < leastTime)
				{
					leastTime = proj.timeLeft;
					leastIndex = proj.whoAmI;
				}
			}
		}
		ProjectileLoader.NumGrappleHooks(self, player, ref numHooks);
		if (grapCount > numHooks)
			Main.projectile[leastIndex].Kill();
		if (self.ai[0] != 1)
		{
			var gproj = self.GetGlobalProjectile<ProjColliding>();
			if (gproj.handler.attachTile is null)
			{
				foreach (var tile in TileSystem.Instance.Tiles)
				{
					if (tile is IHookable hookable)
					{
						var c = new CAABB(new AABB(self.position, self.Size));
						if (tile.Collision(c))
						{
							hookable.SetHookPosition(self);
							if (self.type == 935 && self.alpha == 0 && Main.myPlayer == self.owner)
							{
								player.DoQueenSlimeHookTeleport(self.position);
								NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, self.owner);
							}
							callFromHook.Add(self);
							self.ai[0] = 2;
							self.netUpdate = true;
							self.velocity *= 0;
							if (self.alpha == 0)
							{
								self.alpha = 1;
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, self.Center);
							}
							gproj.handler.attachTile = tile;
							break;
						}
					}
				}
			}
			else
			{
				if (gproj.handler.attachTile.Active)
				{
					callFromHook.Add(self);
					(gproj.handler.attachTile as IHookable).SetHookPosition(self);
				}
				else
				{
					gproj.handler.attachTile = null;
				}
			}
		}
		orig(self);
		callFromHook.Remove(self);
	}

	private static void Projectile_HandleMovement(On_Projectile.orig_HandleMovement orig, Projectile self, Vector2 wetVelocity, out int overrideWidth, out int overrideHeight)
	{
		if (!TileSystem.Enable || !self.tileCollide || self.aiStyle == HookAiStyle)
		{
			orig(self, wetVelocity, out overrideWidth, out overrideHeight);
			return;
		}

		TileSystem.EnableCollisionHook = false;
		var proj = self.GetGlobalProjectile<ProjColliding>();
		proj.handler.position = self.position;//记录位置，否则会把传送当成位移
		orig(self, wetVelocity, out overrideWidth, out overrideHeight);
		proj.handler.Update(true);
		TileSystem.EnableCollisionHook = true;
	}
}