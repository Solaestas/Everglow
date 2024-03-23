using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.CorruptWormHive.Projectiles;

public class TrueDeathSickleHit : ModProjectile, IWarpProjectile, IBloomProjectile
{
	private float r = 20;
	private Vector2 v0;
	private int Fra = 0;
	private int FraX = 0;
	private int FraY = 0;
	private float Stre2 = 1;
	public override string Texture => "Everglow/Myth/TheFirefly/Projectiles/MothBall";
	public override bool CloneNewInstances => false;
	public override bool IsCloneable => false;

	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 200;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 2;
	}

	public override void AI()
	{
		Projectile.velocity *= 0.95f;
		if (Stre2 > 0)
			Stre2 -= 0.005f;
		if (Projectile.timeLeft > 260)
			r += 1f;
		if (Projectile.timeLeft is <= 240 and >= 60)
			r = 60 + (float)(10 * Math.Sin((Projectile.timeLeft - 60) / 60d * Math.PI));
		if (Projectile.timeLeft < 60 && r > 0.5f)
			r -= 1f;
		Fra = (600 - Projectile.timeLeft) / 3 % 30;
		FraX = Fra % 6 * 270;
		FraY = Fra / 6 * 290;
		if (v0 != Vector2.Zero)
		{
			// Projectile.position = v0 - new Vector2(Dx, Dy) / 2f;
		}
		if (Projectile.timeLeft < 10)
			Projectile.friendly = true;


		int MaxC = (int)Projectile.ai[0];
		MaxC = Math.Min(9, MaxC);
		if (Projectile.timeLeft >= 200)
		{
			for (int x = 0; x < MaxC; x++)
			{
				SparkVelocity[x] = new Vector2(0, Projectile.ai[0]).RotatedByRandom(6.283) * Main.rand.NextFloat(0.05f, 1.2f);
				SparkOldPos[x, 0] = Projectile.Center;
			}
		}

		for (int x = 0; x < MaxC; x++)
		{
			for (int y = 39; y > 0; y--)
			{
				SparkOldPos[x, y] = SparkOldPos[x, y - 1];
			}
			if (Collision.SolidCollision(SparkOldPos[x, 0] + new Vector2(SparkVelocity[x].X, 0), 0, 0))
				SparkVelocity[x].X *= -0.95f;
			if (Collision.SolidCollision(SparkOldPos[x, 0] + new Vector2(0, SparkVelocity[x].Y), 0, 0))
				SparkVelocity[x].Y *= -0.95f;
			SparkOldPos[x, 0] += SparkVelocity[x];

			if (SparkVelocity[x].Length() > 0.3f)
				SparkVelocity[x] *= 0.95f;
			SparkVelocity[x].Y += 0.04f;
		}
		Projectile.velocity *= 0;
	}
	private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();
		for (int h = 0; h < radious / 2; h++)
		{
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
		if (circle.Count > 0)
		{
			Main.graphics.GraphicsDevice.Textures[0] = tex;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D Shadow = ModAsset.TrueDeathSickleHit.Value;
		float Dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
		Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, Color.White * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f, SpriteEffects.None, 0);
		Texture2D light = ModAsset.TrueDeathSickleHitStar.Value;
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(1f - Dark, Dark, 1f, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] / 20f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(1f - Dark, Dark, 1f, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, Dark) * Projectile.ai[0] / 20f, SpriteEffects.None, 0);
		float size = Math.Clamp(Projectile.timeLeft / 8f - 10, 0f, 20f);
		if (size > 0)
		{
			DrawSpark(Color.White, size, ModAsset.SparkDark.Value);
			DrawSpark(new Color(131, 0, 255, 0), size, ModAsset.SparkLight.Value);
		}
		return false;
	}

	private Vector2[,] SparkOldPos = new Vector2[27, 40];
	private Vector2[] SparkVelocity = new Vector2[27];
	internal void DrawSpark(Color c0, float width, Texture2D tex)
	{
		int MaxC = (int)Projectile.ai[0];
		MaxC = Math.Min(26, MaxC);
		var bars = new List<Vertex2D>();
		for (int x = 0; x < MaxC; x++)
		{
			int TrueL = 0;
			for (int i = 1; i < 40; ++i)
			{
				if (SparkOldPos[x, i] == Vector2.Zero)
					break;

				TrueL++;
			}
			for (int i = 1; i < 40; ++i)
			{
				if (SparkOldPos[x, i] == Vector2.Zero)
					break;

				var normalDir = SparkOldPos[x, i - 1] - SparkOldPos[x, i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
				var factor = i / (float)TrueL;
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				float x0 = 1 - factor;
				if (i == 1)
				{
					bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
					bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
				}
				bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
				bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
				if (i == 39)
				{
					bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 1, w)));
					bars.Add(new Vertex2D(SparkOldPos[x, i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.Transparent, new Vector3(x0, 0, w)));
				}
			}
			Texture2D t = tex;
			Main.graphics.GraphicsDevice.Textures[0] = t;
		}
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}

	private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();

		for (int h = 0; h < radious / 2; h += 1)
		{
			float colorR = (h / radious * MathF.PI * 4 + (float)addRot + 1.57f) % (MathF.PI * 2f) / (MathF.PI * 2f);
			float color2R = ((h + 1) / radious * MathF.PI * 4 + (float)addRot + 1.57f) % (MathF.PI * 2f) / (MathF.PI * 2f);

			color = new Color(colorR, color.G / 255f, 0, 0);
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0.8f, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0.2f, 0)));
			if (Math.Abs(color2R - colorR) > 0.8f)
			{
				float midValue = (1f - colorR) / (float)(color2R + (1f - colorR));
				color.R = 255;
				circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radious, 0.8f, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radious, 0.2f, 0)));
				color.R = 0;
				circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radious, 0.8f, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy((h + midValue) / radious * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radious, 0.2f, 0)));
			}
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 0.8f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0.2f, 0)));
		if (circle.Count > 2)
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float value = (200 - Projectile.timeLeft) / 200f;
		float colorV = 0.9f * (1 - value);
		if (Projectile.ai[0] >= 10)
			colorV *= Projectile.ai[0] / 10f;
		Texture2D t = Commons.ModAsset.Trail_1.Value;
		float width = 100;
		if (Projectile.timeLeft < 180)
			width = (Projectile.timeLeft - 130) * 2;
		if (width <= 0)
			return;
		DrawTexCircle_VFXBatch(spriteBatch, MathF.Sqrt(value) * 27 * Projectile.ai[0], width * 4, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
	}
	public void DrawBloom()
	{
		float size = Math.Clamp(Projectile.timeLeft / 8f - 60, 0f, 20f);
		if (size > 0)
			DrawSpark(new Color(255, 255, 255, 0), size, ModAsset.SparkLight.Value);
	}
}