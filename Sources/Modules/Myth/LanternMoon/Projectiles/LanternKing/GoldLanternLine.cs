namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing
{
	class GoldLanternLine : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 240;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
		bool HasColid = false;
		bool HasPro = false;
		public override void AI()
		{
			Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];

			if (Projectile.timeLeft >= 60)
			{
				Vector2 v2 = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, v2, ModContent.ProjectileType<GoldLanternLine2>(), 2, 0, player.whoAmI, 0, 0);
			}

			Vector2 v = Vector2.Normalize(player.Center - Projectile.Center) * 0.15f;
			Projectile.velocity.Y += 0.2f;
			Projectile.velocity += v;
			if (HasColid)
			{
				if (!HasPro)
				{
					for (int j = 0; j < 10; j++)
					{
						Vector2 va = Projectile.velocity.RotatedBy(j / 5f * Math.PI);
						Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, va * 3f, ModContent.ProjectileType<GoldLanternLine3>(), 0, 0, player.whoAmI, 0, 0);
					}
					HasPro = true;
				}
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!HasColid)
			{
				HasColid = true;
				if (Projectile.timeLeft >= 60)
					Projectile.timeLeft = 60;
			}
			Projectile.velocity *= 0.2f;
			return false;
		}
		public override void Kill(int timeLeft)
		{
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float k1 = 60f;
			float k0 = (240 - Projectile.timeLeft) / k1;

			if (Projectile.timeLeft <= 240 - k1)
				k0 = 1;

			var c0 = new Color(k0 * 0.8f + 0.2f, k0 * k0 * 0.4f + 0.2f, 0f, 0);
			var bars = new List<Vertex2D>();


			int TrueL = 0;
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
					break;

				TrueL++;
			}
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				float width = 36;
				if (Projectile.timeLeft <= 40)
					width = Projectile.timeLeft * 0.9f;
				if (i < 10)
					width *= i / 10f;
				if (Projectile.ai[0] == 3)
					width *= 0.5f;
				if (Projectile.oldPos[i] == Vector2.Zero)
					break;

				var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
				var factor = i / (float)TrueL;
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
				x0 %= 1f;
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
			}
			Texture2D t = Common.MythContent.QuickTexture("LanternMoon/Projectiles/LanternKing/GoldLaser");
			Main.graphics.GraphicsDevice.Textures[0] = t;
			if (bars.Count > 3)
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			return true;
		}
	}
}
