using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

internal class RodSpear : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 240;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
	}

	internal bool shot = false;
	internal int power = 0;
	public int stickNPC = -1;
	public float relativeAngle = 0;
	public float hitTargetAngle = 0;
	public float hitTargetScale = 1;
	public Vector2 relativePos = Vector2.zeroVector;

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];

		if (!shot)
		{
			shot = true;
			power = 50;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.Center, new Vector2(0, -1 * player.gravDir)) * (power + 100) / 8f;
			Projectile.damage = (int)(Projectile.damage * (power + 100) / 100f);
			SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
		}
		else
		{
			if (Projectile.wet)
			{
				Projectile.timeLeft -= 2;
			}
			if (Projectile.lavaWet)
			{
				Projectile.timeLeft -= 24;
			}

			if (stickNPC != -1)
			{
				NPC stick = Main.npc[stickNPC];
				if (stick != null && stick.active)
				{
					Projectile.rotation = stick.rotation + relativeAngle;
					Projectile.Center = stick.Center + relativePos.RotatedBy(stick.rotation + relativeAngle - hitTargetAngle) * stick.scale / hitTargetScale;
				}
				else
				{
					stickNPC = -1;
				}
			}
			else
			{
				Projectile.tileCollide = true;
				if (Collide(Projectile.Center))
				{
					Projectile.damage = (int)(Projectile.damage * 0.1f);
					if (Projectile.damage == 0)
					{
						Projectile.damage = 1;
					}
					Projectile.knockBack = 0;
				}
				else if (!Collision.SolidCollision(Projectile.Center, 0, 0))
				{
					Projectile.velocity.Y += 0.25f;
					Projectile.velocity *= 0.995f;
					Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
				}
			}
		}
	}

	public bool Collide(Vector2 positon)
	{
		foreach (NPC npc in Main.npc)
		{
			if (npc.active && !npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
			{
				if (new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1).Intersects(npc.Hitbox))
				{
					Projectile.velocity *= 0;
					relativeAngle = Projectile.rotation - npc.rotation;
					hitTargetAngle = Projectile.rotation;
					relativePos = Projectile.Center - npc.Center;
					hitTargetScale = npc.scale;
					stickNPC = npc.whoAmI;
					return true;
				}
			}
		}
		return Collision.SolidCollision(positon, 0, 0);
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		return true;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texStick = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Main.spriteBatch.Draw(
			texStick,
			Projectile.Center - Main.screenPosition,
			null,
			lightColor,
			Projectile.rotation,
			texStick.Size() / 2f,
			Projectile.scale,
			SpriteEffects.None,
			0);
		return false;
	}

	public override void OnKill(int timeLeft)
	{
		if (timeLeft > 60)
		{
			for (int x = 0; x < 16; x++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, 40, 40, DustID.Dirt);
				d.velocity *= Projectile.velocity.Length() / 10f;
			}
			SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
		}
	}
}