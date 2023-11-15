using Terraria;
namespace Everglow.Myth.LanternMoon.Projectiles.DashCore;

class YellowFlame0Shine : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 34;
		Projectile.height = 34;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 180;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color(0, 0, 0, 0);
	}
	float ka = 1;
	public override void AI()
	{
		Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (Projectile.ai[0] != 15)
		{
			Projectile.velocity.Y += 0.15f;
			Projectile.velocity *= 0.99f;
		}
		if (Projectile.timeLeft < 60f)
			ka *= 0.97f;
		Lighting.AddLight(Projectile.Center, (byte)(color0.R * ka) / 100f, (byte)(color0.G * ka) / 100f, (byte)(color0.B * ka) / 100f);
		if (Projectile.timeLeft < 60)
			Projectile.scale *= 0.97f;
		for (int j = 0; j < Main.player.Length; j++)
		{
			if (Main.player[j].active)
			{
				if (!Main.player[j].HasBuff(ModContent.BuffType<Buffs.YellowImmune>()))
				{
					if (!Main.player[j].dead)
					{
						if ((Main.player[j].Center - Projectile.Center).Length() < 24)
							Projectile.NewProjectile(null, Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<Acytaea.Projectiles.playerHit>(), Projectile.damage, 0, j, 0, 0);
					}
				}
			}
		}
		color0.R = (byte)(color0.R * 0.84f + Aimcolor.R * 0.16f);
		color0.G = (byte)(color0.G * 0.84f + Aimcolor.G * 0.16f);
		color0.B = (byte)(color0.B * 0.84f + Aimcolor.B * 0.16f);
		color0.A = (byte)(color0.A * 0.84f + Aimcolor.A * 0.16f);
		ProjOldColor[0] = color0;
		for (int f = ProjOldColor.Length - 1; f > 0; f--)
		{
			ProjOldColor[f] = ProjOldColor[f - 1];
		}
		if (Projectile.timeLeft < 120)
		{
			float dx = 120 - Projectile.timeLeft;
			float F0 = (float)(Math.Sin(dx * dx / 192d) + 1);
			Aimcolor = new Color((int)(255 * F0), (int)(204 * F0), 0);
		}
		kb *= 0.97f;
	}
	Color color0 = new Color(255, 204, 0);
	Color Aimcolor = new Color(255, 204, 0);
	Color[] ProjOldColor = new Color[70];
	float kb = 1;
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	int TrueL = 1;
	public override void PostDraw(Color lightColor)
	{
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		float dx = 120 - Projectile.timeLeft;
		float F0 = (float)(Math.Sin(dx * dx / 192d) + 1);
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(color0.R, color0.G, color0.B, 0), Projectile.rotation, new Vector2(17, 17), Projectile.scale * 1.1f * F0, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/LanternMoon/Projectiles/DashCore/DashCoreLight").Value, Projectile.Center - Main.screenPosition, null, new Color(color0.R / 155f * kb, color0.G / 155f * kb, color0.B / 155f * kb, 0), Projectile.rotation, new Vector2(56, 56), Projectile.scale * 1f * F0, SpriteEffects.None, 0);
		/* x += 0.01f;
             float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
             float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;
             Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, 0, new Vector2(128f, 128f), K * 2.4f, SpriteEffects.None, 0f);
             Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, (float)(Math.PI * 0.5), new Vector2(128f, 128f), K * 2.4f, SpriteEffects.None, 0f);
             Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.75), new Vector2(128f, 128f), M * 2.4f, SpriteEffects.None, 0f);
             Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.25), new Vector2(128f, 128f), M * 2.4f, SpriteEffects.None, 0f);
             Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, x * 6f, new Vector2(128f, 128f), (M + K) * 2.4f, SpriteEffects.None, 0f);
             Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, -x * 6f, new Vector2(128f, 128f), (float)Math.Sqrt(M * M + K * K) * 2.4f, SpriteEffects.None, 0f);
            */
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		float width = 20;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft / 3f;
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

			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(17, 17) - Main.screenPosition, new Color(84, 53, 46, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(17, 17) - Main.screenPosition, new Color(84, 53, 46, 0), new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(84, 53, 46, 0), new Vector3(0, 0.5f, 1));
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
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/LanternMoon/Projectiles/DashCore/MeteroTrail").Value;
		Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);

		/*List<Vertex2D> bars2 = new List<Vertex2D>();
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars2.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(ProjOldColor[i].R, ProjOldColor[i].G, ProjOldColor[i].B, 0), new Vector3(factor, 1, w)));
                bars2.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(ProjOldColor[i].R, ProjOldColor[i].G, ProjOldColor[i].B, 0), new Vector3(factor, 0, w)));
            }
            List<Vertex2D> Vx2 = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                Vx2.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(color0.R, color0.G, color0.B, 0), new Vector3(0, 0.5f, 1));
                Vx2.Add(bars[1]);
                Vx2.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx2.Add(bars[i]);
                    Vx2.Add(bars[i + 2]);
                    Vx2.Add(bars[i + 1]);

                    Vx2.Add(bars[i + 1]);
                    Vx2.Add(bars[i + 2]);
                    Vx2.Add(bars[i + 3]);
                }
            }
            t = ModContent.Request<Texture2D>("Everglow/Myth/LanternMoon/Projectiles/DashCore/Flame").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);*/
	}
}
