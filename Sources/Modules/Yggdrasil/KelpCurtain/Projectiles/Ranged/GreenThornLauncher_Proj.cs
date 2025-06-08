using Terraria.Audio;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Ranged;

public class GreenThornLauncher_Proj : ModProjectile
{
	public int TileCollideCounter { get; set; }

	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;

		Projectile.DamageType = DamageClass.Ranged;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
	}

	public override void AI()
	{
		Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
		if (Projectile.velocity.Y > 16f)
		{
			Projectile.velocity.Y = 16f;
		}
		Projectile.rotation += 0.2f * Projectile.direction;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (++TileCollideCounter >= 5)
		{
			return true;
		}
		if (Projectile.velocity.X != oldVelocity.X)
		{
			Projectile.velocity.X = -oldVelocity.X;
		}

		if (Projectile.velocity.Y != oldVelocity.Y)
		{
			Projectile.velocity.Y = -oldVelocity.Y;
		}

		Projectile.velocity *= 0.69f;
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		float knockback = 1f;
		if (Main.myPlayer == Projectile.owner)
		{
			var projNum = Main.rand.Next(2, 4);
			for (int i = 0; i < projNum; i++)
			{
				var projVelo = new Vector2(0, 10f).RotatedByRandom(MathHelper.TwoPi);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, projVelo, ModContent.ProjectileType<GreenThornLauncher_SubProj>(), (int)(Projectile.damage * 0.4f), knockback, Projectile.owner);
			}
		}
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
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, 1f, SpriteEffects.None, 0);
		return false;
	}
}