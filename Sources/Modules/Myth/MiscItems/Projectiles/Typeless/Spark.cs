namespace Everglow.Myth.MiscItems.Projectiles.Typeless
{
	public class Spark : ModProjectile
	{
		//public override void SetStaticDefaults()
		//{
		//    DisplayName.SetDefault("spark");
		//    DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "火花");
		//}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 1080;
			Projectile.alpha = 0;
			Projectile.penetrate = 9;
			Projectile.extraUpdates = 2;
			Projectile.scale = 1;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
		}
		private bool initialization = true;
		private double X;
		private float Omega;
		private float b;
		public override void AI()
		{
			ka = 0.2f;
			if (Projectile.timeLeft < 60f)
				ka = Projectile.timeLeft / 300f;
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			Projectile.velocity.Y += 0.06f;
			Projectile.scale *= 0.96f;
			if (Projectile.scale < 0.15f)
				Projectile.Kill();
			for (int j = 0; j < 200; j++)
			{
				if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1) && Main.npc[j].active && !Main.npc[j].friendly)
				{
					if ((Main.npc[j].Center - Projectile.Center).Length() < 12)
					{
						int Dam = (int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)) - Main.npc[j].defDefense;
						if (Dam < 1)
							Dam = 1;
						Main.npc[j].StrikeNPC(Dam, 0, Projectile.direction, Main.rand.Next(100) < Projectile.ai[0]);
						Player p = Main.player[Projectile.owner];
						p.dpsDamage += (int)(Dam * (100 + Projectile.ai[0]) / 100d);
						Main.npc[j].AddBuff(BuffID.OnFire, 120);
						Projectile.Kill();
					}
				}
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, 0));
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 120);
			Projectile.Kill();
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
				Projectile.Kill();
			else
			{
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;
				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
				Projectile.velocity *= 0.98f;
				Projectile.scale *= 0.9f;
			}
			return false;
		}
		private Effect ef;
		int TrueL = 1;
		float ka = 0;
		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			var bars = new List<Vertex2D>(); // Was List<VertexBase.CustomVertexInfo>
			float width = 2 * Projectile.scale;
			if (Projectile.timeLeft < 60)
				width = Projectile.timeLeft / 30f * Projectile.scale;
			TrueL = 0;
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
				factor = i / (float)TrueL;
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				Lighting.AddLight(Projectile.oldPos[i], (255 - Projectile.alpha) * 0.5f / 50f * ka * (1 - factor), (255 - Projectile.alpha) * 0.2f / 50f * ka * (1 - factor), 0);
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(8f, 8f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(8f, 8f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
			}
			var Vx = new List<Vertex2D>();
			if (bars.Count > 2)
			{
				Vx.Add(bars[0]);
				var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity), new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
				Vx.Add(bars[1]);
				Vx.Add(vertex);
				for (int i = 0; i < bars.Count - 2; i += 2)
				{
					Vx.Add(bars[i]);
					Vx.Add(bars[i + 2]);
					Vx.Add(bars[i + 1]);

					Vx.Add(bars[i + 1]);
					Vx.Add(bars[i + 2]);
					Vx.Add(bars[i + 3]);
				}
			}
			Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Clubs/Projectiles/FireLine").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
			return true;
		}
	}
}