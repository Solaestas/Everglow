using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

internal class LightArrow : ModProjectile
{
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
		// Apply gravity after a quarter of a second
		Projectile.ai[0] += 1f;
		if (Projectile.ai[0] >= 15f)
		{
			Projectile.ai[0] = 15f;
			Projectile.velocity.Y += 0.1f;
		}

		// The projectile is rotated to face the direction of travel
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

		// Cap downward velocity
		if (Projectile.velocity.Y > 16f)
		{
			Projectile.velocity.Y = 16f;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var TexMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathF.PI, TexMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(ModContent.BuffType<Photolysis>(), 180);
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		for (int i = 0; i < 20; i++)
		{
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, newColor: Color.Yellow);
			dust.noGravity = true;
			dust.velocity *= 1.5f;
			dust.scale *= 4f;
			dust.fadeIn = Main.rand.NextFloat(420, 600);
		}
	}
}