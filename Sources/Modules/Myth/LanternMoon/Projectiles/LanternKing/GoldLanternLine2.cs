namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing
{
	public class GoldLanternLine2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("灯笼须2");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;

			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
		}
		private float Z = 0;
		public override void AI()
		{
			if (Projectile.timeLeft == 600)
			{
				Z = Main.rand.NextFloat(0, (float)(Math.PI * 2));
				Projectile.timeLeft = Main.rand.Next(20, 40);
			}
			Vector2 v = new Vector2(0, 3).RotatedBy(Z);
			Z += Main.rand.NextFloat(-0.04f, 0.04f);
			Projectile.velocity = Projectile.velocity * 0.9f + v * 0.1f;
		}
		public override void PostDraw(Color lightColor)
		{
			var bars = new List<Vertex2D>();
			float width = 12;
			if (Projectile.timeLeft < 60)
				width = Projectile.timeLeft / 5f;
			int TrueL = 0;
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
					break;
				TrueL++;
			}
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
					break;
				var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
				var factor = 1f;
				if (Projectile.oldPos.Length > 0)
					factor = i / (float)Projectile.oldPos.Length;
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
			}
			Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapLanternLine").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}
}
