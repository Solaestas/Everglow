using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class DarkLanternBombExplosion_II : ModProjectile, IWarpProjectile
{
	public override string Texture => "Everglow/" + ModAsset.DarkLanternBombExplosion_Path;

	public override void SetDefaults()
	{
		Projectile.width = 400;
		Projectile.height = 400;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 120;
		Projectile.penetrate = -1;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
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
		float value = (120 - Projectile.timeLeft) / 120f;
		value = MathF.Sqrt(value);
		Texture2D t = ModAsset.ExplosionHive.Value;
		float width = 60;
		if (Projectile.timeLeft < 60)
		{
			width = Projectile.timeLeft;
		}

		DrawWarpTexCircle_VFXBatch(sb, 150 + value * value * 780, width * 6, Projectile.Center - Main.screenPosition, t, Projectile.timeLeft / 2000f);
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		bool CheckCenter(Vector2 pos)
		{
			return (pos - Projectile.Center).Length() < 370 - Projectile.timeLeft;
		}
		return CheckCenter(targetHitbox.TopLeft()) || CheckCenter(targetHitbox.TopRight()) || CheckCenter(targetHitbox.BottomLeft()) || CheckCenter(targetHitbox.BottomRight());
	}
}