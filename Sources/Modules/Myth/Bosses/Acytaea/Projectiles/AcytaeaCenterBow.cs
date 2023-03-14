namespace Everglow.Myth.Bosses.Acytaea.Projectiles
{
	internal class AcytaeaCenterBow : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 40;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(0, 0, 0, 0));
		}

		public override void AI()
		{
			if (Projectile.timeLeft % 5 == 2)
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AcytaeaBow2>(), Projectile.damage, 3, Main.LocalPlayer.whoAmI, (float)(Projectile.timeLeft / 20d * Math.PI));
		}
	}
}