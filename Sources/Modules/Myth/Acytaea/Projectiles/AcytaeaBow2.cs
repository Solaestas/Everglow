namespace Everglow.Myth.Acytaea.Projectiles;

internal class AcytaeaBow2 : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 50;
		Projectile.height = 50;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 60;
		//Projectile.extraUpdates = 10;
		Projectile.tileCollide = true;
		Projectile.DamageType = DamageClass.Ranged;
	}
	public override void OnKill(int timeLeft)
	{
		Projectile.NewProjectile(null, Projectile.Center, new Vector2(34, 0).RotatedBy(Projectile.ai[0]), ModContent.ProjectileType<AcytaeaArrow>(), Projectile.damage, 3, Main.LocalPlayer.whoAmI);
	}

	public override Color? GetAlpha(Color lightColor)
	{
		return Projectile.timeLeft > 40
			? new Color?(new Color((int)(255 * (40 - Projectile.timeLeft) / 20f), (int)(255 * (40 - Projectile.timeLeft) / 20f), (int)(255 * (40 - Projectile.timeLeft) / 20f), 0))
			: new Color?(new Color(255, 255, 255, 0));
	}

	private float ka = 1;

	public override void AI()
	{
		ka = 1;
		if (Projectile.timeLeft < 60f)
			ka = Projectile.timeLeft / 60f;
		Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.2f / 250f * ka, 0, 0);
		Projectile.rotation = Projectile.ai[0];
	}

	public override bool PreDraw(ref Color lightColor)
	{
		if (Projectile.timeLeft < 30f)
		{
			Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/Acytaea/Projectiles/AcytaeaWarningArrow3").Value;
			var drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
			float Fade;
			if (Projectile.timeLeft < 25f)
				Fade = Projectile.timeLeft / 25f;
			else
			{
				float ad = (30 - Projectile.timeLeft) / 5f;
				Fade = ad * ad * ad;
			}
			Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, new Color(Fade * Fade, Fade * Fade, Fade * Fade, 0), Projectile.rotation + 3.14159f, drawOrigin, Projectile.scale * 2, SpriteEffects.None, 0f);
		}
		return true;
	}
}