namespace Everglow.Myth.TheTusk.Projectiles;

public class TuskCrack : ModProjectile
{
	public override void SetDefaults()
	{
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 600;
		Projectile.width = 80;
		Projectile.height = 80;
		Projectile.tileCollide = false;
	}
	public override void AI()
	{
		if (Projectile.timeLeft > 150 && Projectile.timeLeft <= 533)
		{
			int Frequ = 24;
			if (Main.expertMode && !Main.masterMode)
				Frequ = 18;
			if (Main.masterMode)
				Frequ = 3;
			if (Projectile.timeLeft % Frequ == 0)
			{
				float RanX = Main.rand.NextFloat(-400, 400);
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + new Vector2(RanX, -RanX / 15f) + new Vector2(0, -5), new Vector2(0, 0.3f), ModContent.ProjectileType<TuskSpiceBlack>(), (int)((double)Projectile.damage / 6), Projectile.knockBack, Projectile.owner, 0f, 0f);
			}
		}
		if (Projectile.velocity.Length() > 46)
			Projectile.velocity = Projectile.velocity / Projectile.velocity.Length() * 46;
		//ef2.Parameters["Strds"].SetValue(100);
	}
}
