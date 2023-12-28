using Everglow.Myth.Common;

namespace Everglow.Myth.LanternMoon.Projectiles;

class LMeteor : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 180;
		Projectile.tileCollide = false;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
	}
	float ka = 0;
	int TrueL = 1;
	float Stre = 0.85f;
	float Sca = 0;
	public override ModProjectile Clone(Projectile projectile)
	{
		var clone = base.Clone(projectile) as LMeteor;
		//优化从小事做起（bushi）
		//ka = 0;
		//AIMpos = Vector2.Zero;
		//TrueL = 1;
		//Stre = 0.85f;
		//Sca = 0;
		return clone;
	}
	public override void AI()
	{
		if (Projectile.timeLeft >= 140)
			Sca = (float)(-Math.Cos((180 - Projectile.timeLeft) / 40d * Math.PI) + 1) * 0.65f;
		else
		{
			Sca = 0.96f + Sca * 0.04f;
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.2f / 50f * ka, 0, 0);
			Projectile.velocity.Y -= 0.25f;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override void PostDraw(Color lightColor)
	{
		var bars = new List<Vertex2D>();
		int width = 60;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft;
		TrueL = 0;
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			TrueL++;
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
		}
		for (int i = 1; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
			normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

			var factor = i / (float)TrueL;
			var w = MathHelper.Lerp(1f, 0.05f, factor);

			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 1, w)));
			bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 0, w)));
		}
		var Vx = new List<Vertex2D>();
		if (bars.Count > 2)
		{
			Vx.Add(bars[0]);
			var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
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
		if (Vx.Count > 2)
		{
			Texture2D t = ModAsset.LMeteor.Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		}
		Texture2D LightE = ModAsset.LightEffect.Value;
		Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(0.3f * Stre * Stre, 0.21f * Stre * Stre, 0, 0), -(float)Math.Sin(Main.time / 26d) + 0.6f, new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d))) * Sca, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(1f * Stre * Stre, 0.7f * Stre * Stre, 0, 0), (float)Math.Sin(Main.time / 12d + 2) + 1.6f, new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d))) * Sca, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(0.3f * Stre * Stre, 0.21f * Stre * Stre, 0, 0), (float)Math.PI / 2f + (float)(Main.time / 9d), new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d + 1.57))) * Sca, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(1f * Stre * Stre, 0.7f * Stre * Stre, 0, 0), (float)(Main.time / 26d), new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d + 3.14))) * Sca, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(1f * Stre * Stre, 0.7f * Stre * Stre, 0, 0), -(float)(Main.time / 26d), new Vector2(128f, 128f), (1.5f + (float)(0.75 * Math.Sin(Main.time / 26d + 4.71))) * Sca, SpriteEffects.None, 0);
	}
}
