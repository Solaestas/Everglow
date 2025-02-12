using Everglow.Yggdrasil.YggdrasilTown.Buffs.Pets;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Pets;

public class BadrProj : ModProjectile
{
	public Player Owner => Main.player[Projectile.owner];

	public ref float Time => ref Projectile.ai[0];

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 5;
		Main.projPet[Projectile.type] = true;
		ProjectileID.Sets.LightPet[Projectile.type] = true;
	}

	public override void SetDefaults()
	{
		Projectile.width = Projectile.height = 24;
		Projectile.friendly = true;
		Projectile.netImportant = true;
		Projectile.penetrate = -1;
	}

	public override void AI()
	{
		if (VerifyOwnerIsPresent())
		{
			return;
		}

		HandleFrames();
		DoSpinEffect();
		HoverTowardsOwnersShoulder();

		// Look in the same direction as the projectile's owner.
		Projectile.spriteDirection = Owner.direction;

		// Emit light.
		Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 2.5f);

		Time++;
	}

	public bool VerifyOwnerIsPresent()
	{
		// No logic should be run if the player is no longer active in the game.
		if (!Owner.active)
		{
			Projectile.Kill();
			return true;
		}

		if (Owner.HasBuff<BadrBuff>())
		{
			Projectile.timeLeft = 2;
		}

		return false;
	}

	public void HandleFrames()
	{
		Projectile.frameCounter++;
		Projectile.frame = Projectile.frameCounter / 10 % Main.projFrames[Projectile.type];
	}

	public void DoSpinEffect()
	{
		// Spin around from time to time.
		if (Projectile.frameCounter % 180f > 150f)
		{
			Projectile.rotation += MathHelper.TwoPi / 30f;
		}
		else
		{
			Projectile.rotation = 0f;
		}
	}

	public void HoverTowardsOwnersShoulder()
	{
		Vector2 destination = Owner.Center;
		destination.X -= Owner.direction * Owner.width * 1.5f;
		destination.Y -= Owner.height * 1.5f;

		// Hover in the air a little bit over time so that the pet doesn't seem static.
		destination.Y += (float)Math.Sin(MathHelper.TwoPi * Time / 100f) * 5f;

		Projectile.Center = Vector2.Lerp(Projectile.Center, destination, 0.125f);
		if (Projectile.WithinRange(destination, 10f))
		{
			Projectile.Center = destination;
		}
		else
		{
			Projectile.Center += Projectile.DirectionTo(destination) * 4f;
		}

		Projectile.Center = destination;
	}
}