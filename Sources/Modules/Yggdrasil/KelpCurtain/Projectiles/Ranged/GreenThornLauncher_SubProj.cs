using Terraria.Audio;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Ranged;

public class GreenThornLauncher_SubProj : ModProjectile
{
	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 2;
	}

	public override void SetDefaults()
	{
		Projectile.width = 14;
		Projectile.height = 22;

		Projectile.DamageType = DamageClass.Ranged;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 300;
		Projectile.tileCollide = true;
	}

	public override void AI()
	{
		Projectile.frameCounter++;
		if (Projectile.frameCounter >= 6)
		{
			Projectile.frameCounter = 0;
			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
		for (int i = 0; i < 20; i++)
		{
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GrassBlades);
			dust.noGravity = true;
			dust.velocity *= 0.5f;
			dust.scale = Main.rand.NextFloat(0.8f, 1.4f);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
		Main.spriteBatch.Draw(
			texture,
			Projectile.Center - Main.screenPosition,
			frame,
			lightColor,
			Projectile.rotation,
			texture.Size() / 2,
			1f,
			SpriteEffects.None,
			0);
		return false;
	}
}