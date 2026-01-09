using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing.VFXs;
using Everglow.Myth.LanternMoon.VFX;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class DarkLanternBombExplosion : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 100;
		Projectile.height = 100;
		Projectile.aiStyle = -1;
		Projectile.hostile = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 30;
		Projectile.penetrate = -1;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
	}

	public override void AI()
	{
		float value = Projectile.timeLeft / 20f;
		Lighting.AddLight(Projectile.Center, new Vector3(value * 3.5f, value * value * 1.5f, value * value * value));
		base.AI();
	}

	public override void OnSpawn(IEntitySource source)
	{
		float mulSize = 1f;
		if (Main.expertMode)
		{
			mulSize = 1.8f;
		}
		if (Main.masterMode)
		{
			mulSize = 2.4f;
		}
		mulSize *= Projectile.ai[0] / 5f;

		for (int x = 0; x < 35; x++)
		{
			LargeFlame();
		}
		for (int x = 0; x < 75; x++)
		{
			SmallFlame();
		}
		Spark();
	}

	public void Spark()
	{
		for (int g = 0; g < 30; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 22f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new FireSparkDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(20), 0).RotatedByRandom(6.283) + newVelocity * 3,
				maxTime = Main.rand.Next(20, 40),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(1f, 25.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.01f, 0.01f) },
			};
			Ins.VFXManager.Add(spark);
		}

		for (int g = 0; g < 20; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(6f, 22f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new LanternGhostKing_SmokeSpike
			{
				Velocity = newVelocity,
				Active = true,
				Visible = true,
				Position = Projectile.Center + new Vector2(Main.rand.NextFloat(20), 0).RotatedByRandom(6.283) + newVelocity * 3,
				MaxTime = Main.rand.Next(20, 40),
				Scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(1f, 25.0f)),
			};
			Ins.VFXManager.Add(spark);
		}
	}

	public void LargeFlame()
	{
		Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 8f)).RotatedByRandom(MathHelper.TwoPi);
		var somg = new LanternFlameDust
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = Projectile.Center + new Vector2(Main.rand.NextFloat(40), 0).RotatedByRandom(6.283),
			maxTime = Main.rand.Next(60, 75),
			scale = Main.rand.NextFloat(80f, 160f),
			rotation = Main.rand.NextFloat(6.283f),
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
		};
		Ins.VFXManager.Add(somg);
	}

	public void SmallFlame()
	{
		Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 8f)).RotatedByRandom(MathHelper.TwoPi);
		var somg = new LanternFlameDust
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = Projectile.Center + new Vector2(Main.rand.NextFloat(60), 0).RotatedByRandom(6.283),
			maxTime = Main.rand.Next(30, 45),
			scale = Main.rand.NextFloat(50f, 90f),
			rotation = Main.rand.NextFloat(6.283f),
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
		};
		Ins.VFXManager.Add(somg);
	}

	public Vector2 RandomVector2(float maxLength, float minLength = 0)
	{
		if (maxLength <= minLength)
		{
			maxLength = minLength + 0.001f;
		}
		return new Vector2(Main.rand.NextFloat(minLength, maxLength), 0).RotatedByRandom(6.283);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	private void DrawWarpTexCircle_VFXBatch(VFXBatch spriteBatch, float radius, float width, Vector2 center, Texture2D tex, float warpStrength, double addRot = 0)
	{
		var circle = new List<Vertex2D>();
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int h = 0; h < radius / 2; h += 1)
		{
			float colorR = (h / radius * MathF.PI * 4 + (float)addRot + 1.57f) % (MathF.PI * 2f) / (MathF.PI * 2f);
			float color2R = ((h + 1) / radius * MathF.PI * 4 + (float)addRot + 1.57f) % (MathF.PI * 2f) / (MathF.PI * 2f);
			color = new Color(colorR, warpStrength, 0f, 0f);
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(h / radius * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radius, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(h / radius * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radius, 0, 0)));
			if (Math.Abs(color2R - colorR) > 0.8f)
			{
				float midValue = (1f - colorR) / (float)(color2R + (1f - colorR));
				color.R = 255;
				circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy((h + midValue) / radius * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radius, 1, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy((h + midValue) / radius * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radius, 0, 0)));
				color.R = 0;
				circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy((h + midValue) / radius * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radius, 1, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy((h + midValue) / radius * Math.PI * 4 + addRot), color, new Vector3((h + midValue) * 2 / radius, 0, 0)));
			}
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
		if (circle.Count > 2)
		{
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
		}
	}

	public void DrawWarp(VFXBatch sb)
	{
		float value = (30 - Projectile.timeLeft) / 15f;
		value = MathF.Sqrt(value);

		Texture2D t = Commons.ModAsset.NoiseWave.Value;
		float width = 15;
		if (Projectile.timeLeft < 15)
		{
			width = Projectile.timeLeft;
		}

		DrawWarpTexCircle_VFXBatch(sb, value * value * 80 + 90, width * 8, Projectile.Center - Main.screenPosition, t, Projectile.timeLeft / 4000f);
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float maxDistance = 70;
		if (Main.expertMode)
		{
			maxDistance = 82;
		}
		if (Main.masterMode)
		{
			maxDistance = 150;
		}
		maxDistance *= Projectile.ai[0] / 5f;
		bool CheckCenter(Vector2 pos)
		{
			return (pos - projHitbox.Center()).Length() < maxDistance / 0.9f;
		}
		return CheckCenter(targetHitbox.TopLeft()) || CheckCenter(targetHitbox.TopRight()) || CheckCenter(targetHitbox.BottomLeft()) || CheckCenter(targetHitbox.BottomRight());
	}
}