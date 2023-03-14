namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
	public class FruitBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{

		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 1000;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.scale = 0f;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, 0));
		}

		private float b = 0;
		public override void AI()
		{
			b += 0.015f;
			b *= 0.99f;
			Projectile.scale = b / 2f;
			if (Projectile.scale > 0.5f)
			{
				Projectile.Kill();
			}
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.01f / 255f, (float)(255 - base.Projectile.alpha) * 0.6f / 255f * Projectile.scale);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/FruitBomb").Value;

			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), 0, new Vector2(600), Projectile.scale, SpriteEffects.None, 0f);
			if (!Main.gamePaused)
			{
			}
			return false;
		}
	}
}