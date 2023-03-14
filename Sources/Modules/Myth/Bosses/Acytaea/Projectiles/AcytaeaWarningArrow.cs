namespace Everglow.Myth.Bosses.Acytaea.Projectiles
{
	internal class AcytaeaWarningArrow : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 60;
			//Projectile.extraUpdates = 10;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(null, Projectile.Center + new Vector2(0, -800), new Vector2(0, 34), ModContent.ProjectileType<AcytaeaArrow>(), 0, 1, Main.myPlayer);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(0, 0, 0, 0));
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 58)
				/*string s = "⚠";
                CombatText.NewText(new Rectangle((int)Projectile.Center.X - 20, (int)Projectile.Center.Y - 60, 10, 10), Color.Red, s);*/
				Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + Math.PI * 0.25);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Projectiles/AcytaeaWarningArrow2").Value;
			int frameHeight = t.Height;
			var drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
			float Fade = Projectile.timeLeft / 60f;
			Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, new Color(Fade * Fade, Fade * Fade, Fade * Fade, 0), 0, drawOrigin, -Projectile.scale, SpriteEffects.None, 0f);
			return true;
		}
	}
}