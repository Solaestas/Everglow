using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CaterpillarJuiceArrow : ModProjectile
{
	private const int MaxDuration = 6 * 60;
	private const int MinDuration = 4 * 60;

	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;

		Projectile.arrow = true;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;

		Projectile.penetrate = 1;

		Projectile.timeLeft = 1200;
	}

	public override void AI()
	{
		Projectile.velocity.Y += 0.1f;

		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

		if (Projectile.velocity.Y > 16f)
		{
			Projectile.velocity.Y = 16f;
		}

		if (Main.rand.NextBool(4))
		{
			Vector2 afterVelocity = Projectile.velocity * 0.96f;
			float mulScale = Main.rand.NextFloat(3f, 12f);
			var blood = new CaterpillarJuice_Drop
			{
				velocity = afterVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(42, 84),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(blood);
		}
		if (Main.rand.NextBool(12))
		{
			Vector2 afterVelocity = Projectile.velocity * 0.96f;
			var blood = new CaterpillarJuice_splash
			{
				velocity = afterVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + afterVelocity,
				maxTime = Main.rand.Next(32, 94),
				scale = Main.rand.NextFloat(3f, 10f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.4f), 0 },
			};
			Ins.VFXManager.Add(blood);
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Main.rand.Next(MinDuration, MaxDuration);
		target.AddBuff(BuffID.Oiled, 180);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathF.PI, texMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
	}
}