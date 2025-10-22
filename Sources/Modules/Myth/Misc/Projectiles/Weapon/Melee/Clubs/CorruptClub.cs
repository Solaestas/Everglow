namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CorruptClub : ClubProj
{
	public override void SetCustomDefaults()
	{
		EnableReflection = true;
	}

	public override void AI()
	{
		base.AI();

		if (Omega > 0.1f)
		{
			for (float d = 0.1f; d < Omega; d += 0.1f)
			{
				GenerateDust();
			}
		}
		else
		{
			GenerateDust();
		}
	}

	private void GenerateDust()
	{
		var v0 = Vector2.One;
		v0 *= Main.rand.NextFloat(Main.rand.NextFloat(1, HitLength), HitLength);
		v0.X *= Projectile.spriteDirection;
		if (Main.rand.NextBool(2))
		{
			v0 *= -1;
		}

		v0 = v0.RotatedBy(Projectile.rotation);
		float Speed = Math.Min(Omega * 0.5f, 0.14f);
		Dust.NewDust(Projectile.Center + v0 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.Demonite, -v0.Y * Speed, v0.X * Speed, 150, default, Main.rand.NextFloat(0.4f, 0.8f));
	}
}