namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

internal class FeatheredStaff : ModProjectile
{
	private const int MaxCollideCount = 2;
	private bool shot = false;

	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.aiStyle = 1;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = 3;
		Projectile.timeLeft = 300;
		Projectile.tileCollide = true;
	}

	public override void AI()
	{
		if (!shot)
		{
			Projectile.velocity.X *= 2;
			shot = true;
		}

		if (Projectile.velocity.Y > 10f)
		{
			Projectile.velocity.Y = 10f;
		}

		// Bounce logic
		if (Projectile.ai[0] == 0f)
		{
			if (Projectile.velocity.Y == 0f)
			{
				Projectile.ai[0] = 1f;
				Projectile.velocity.X = Projectile.velocity.X * 0.9f;
				Projectile.velocity.Y = -Projectile.velocity.Y;
				Projectile.netUpdate = true;
			}
		}
		else
		{
			Projectile.velocity.Y += 0.2f;
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		// Check collision count
		Projectile.ai[1]++;
		if (Projectile.ai[1] > MaxCollideCount)
		{
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt);
			Projectile.Kill();
			return true;
		}

		if (Projectile.velocity.X != oldVelocity.X)
		{
			Projectile.velocity.X = -oldVelocity.X * 0.8f;
		}
		if (Projectile.velocity.Y != oldVelocity.Y)
		{
			Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
		}
		return false;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		if (Main.rand.NextBool(2))
		{
			Dust.NewDust(
				Projectile.position,
				Projectile.width,
				Projectile.height,
				DustID.Sandstorm,
				newColor: Color.SandyBrown,
				Scale: 2.4f);
		}

		return false;
	}
}