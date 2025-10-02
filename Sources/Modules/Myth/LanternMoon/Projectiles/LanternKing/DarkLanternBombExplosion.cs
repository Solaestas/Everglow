using Everglow.Myth.LanternMoon.Projectiles.LanternKing.VFXs;
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
		if(Main.expertMode)
		{
			mulSize = 1.8f;
		}
		if (Main.masterMode)
		{
			mulSize = 2.4f;
		}
		mulSize *= Projectile.ai[0] / 5f;
		var lanternExplosion = new LanternExplosion
		{
			velocity = Vector2.Zero,
			Active = true,
			Visible = true,
			position = Projectile.Center,
			maxTime = Main.rand.Next(26, 36),
			ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(2.45f, 2.8f) * mulSize, Main.rand.NextFloat(8f, 12f) },
			FireBallVelocity = new Vector2[]
			{
				RandomVector2(2f, 0.01f),
				RandomVector2(1f, 0.01f)
			}
		};
		Ins.VFXManager.Add(lanternExplosion);

		for (int x = 0; x < 5 * mulSize; x++)
		{
			var flameDust = new FlameDust
			{
				velocity = RandomVector2(25f, 10f) * mulSize * 0.7f,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(26, 96),
				ai = new float[] { Main.rand.NextFloat(0.1f, 1f), 0, Main.rand.NextFloat(1f, 3.4f) }
			};
			Ins.VFXManager.Add(flameDust);
		}
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
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	public void DrawWarp(VFXBatch sb)
	{
		float value = (30 - Projectile.timeLeft) / 15f;
		value = MathF.Sqrt(value);

		Texture2D t = Commons.ModAsset.NoiseWave.Value;
		float width = 15;
		if (Projectile.timeLeft < 15)
			width = Projectile.timeLeft;
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
		bool checkCenter(Vector2 pos)
		{
			return (pos - projHitbox.Center()).Length() < maxDistance / 0.9f;
		}
		return checkCenter(targetHitbox.TopLeft()) || checkCenter(targetHitbox.TopRight()) || checkCenter(targetHitbox.BottomLeft()) || checkCenter(targetHitbox.BottomRight());
	}
}