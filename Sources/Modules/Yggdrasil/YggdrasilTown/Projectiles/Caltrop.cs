namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class Caltrop : ModProjectile
{
	private int Durability { get; set; } = 15;

	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;

		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 30;

		Projectile.timeLeft = 2000;

		Projectile.netImportant = true;
	}

	public override void AI()
	{
		Console.WriteLine(Projectile.velocity);
		Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
		if (Projectile.velocity.Y > 16f)
		{
			Projectile.velocity.Y = 16f;
		}
		Projectile.rotation += 0.2f * Projectile.direction * Projectile.velocity.Length().SmoothStep(0, 10f);
	}

	public override bool? CanCutTiles() => true;

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (--Durability <= 0)
		{
			Projectile.Kill();
		}

		base.OnHitNPC(target, hit, damageDone);
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.ai[0] += 0.1f;
		if (Projectile.velocity.X != oldVelocity.X)
		{
			Projectile.velocity.X = -oldVelocity.X;
		}
		if (Projectile.velocity.Y != oldVelocity.Y)
		{
			Projectile.velocity.Y = -oldVelocity.Y;
		}

		Projectile.velocity *= 0.5f;
		if (Projectile.velocity.Length() <= 1f)
		{
			Projectile.velocity = Vector2.Zero;
		}

		return false;
	}

	public override void OnKill(int timeLeft)
	{
		Dust.NewDust(Projectile.Center, 1, 1, DustID.Iron);
	}
}