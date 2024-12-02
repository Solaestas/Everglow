using Everglow.Myth.Common;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class FanHit : ModProjectile, IWarpProjectile
{
	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
	{
		Projectile.width = 200;
		Projectile.height = 200;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 200;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 6;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override void AI()
	{
	}
	private static void DrawTexCircle(float radius, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();
		for (int h = 0; h < radius / 2; h++)
		{
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(h / radius * Math.PI * 4 + addRot), color, new Vector3(h * 24 / radius % 1, 0.8f, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(h / radius * Math.PI * 4 + addRot), color, new Vector3(h * 24 / radius % 1, 0.5f, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), color, new Vector3(1, 0.8f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), color, new Vector3(1, 0.5f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), color, new Vector3(0, 0.8f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), color, new Vector3(0, 0.5f, 0)));
		if (circle.Count > 0)
		{
			Main.graphics.GraphicsDevice.Textures[0] = tex;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
		}
	}
	public override void PostDraw(Color lightColor)
	{
		Texture2D shadow = ModAsset.CursedHitLight.Value;
		float timeValue = (200 - Projectile.timeLeft) / 200f;
		float dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
		Color c = new Color(0.2f * MathF.Sqrt(1 - timeValue), 0.6f * (1 - timeValue) * (1 - timeValue), 3f * (1 - timeValue), 0f);
		Main.spriteBatch.Draw(shadow, Projectile.Center - Main.screenPosition, null, c * dark, 0, shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f * dark, SpriteEffects.None, 0);
		Color cDark = new Color(0, 0, 0, 1f - timeValue);
		DrawTexCircle(MathF.Sqrt(timeValue) * 8 * Projectile.ai[0], 10 * (1 - timeValue) * Projectile.ai[0], cDark, Projectile.Center - Main.screenPosition, Commons.ModAsset.Trail_2_black_thick.Value);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D shadow = ModAsset.CursedHit.Value;
		float timeValue = (200 - Projectile.timeLeft) / 200f;
		float dark = Math.Max((Projectile.timeLeft - 100) / 100f, 0);
		Color c = new Color(0.2f * MathF.Sqrt(1 - timeValue), 0.6f * (1 - timeValue) * (1 - timeValue), 3f * (1 - timeValue), 0f);
		Main.spriteBatch.Draw(shadow, Projectile.Center - Main.screenPosition, null, Color.White * dark, 0, shadow.Size() / 2f, new Vector2(2.2f, dark * dark) * Projectile.ai[0] * 0.08f, SpriteEffects.None, 0);
		Texture2D light = Commons.ModAsset.StarSlash.Value;
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, c, 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, dark * dark) * Projectile.ai[0] * 0.12f, SpriteEffects.None, 0);
		return false;
	}
	private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radius, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();

		Color c0 = color;
		c0.R = 0;
		for (int h = 0; h < radius / 2; h += 1)
		{

			c0.R = (byte)(h / radius * 2 * 255);
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(h / radius * Math.PI * 4 + addRot), c0, new Vector3(h * 2 / radius, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(h / radius * Math.PI * 4 + addRot), c0, new Vector3(h * 2 / radius, 0.5f, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), c0, new Vector3(1, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), c0, new Vector3(1, 0.5f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), c0, new Vector3(0, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), c0, new Vector3(0, 0.5f, 0)));
		if (circle.Count > 2)
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float value = (200 - Projectile.timeLeft) / 200f;
		float colorV = 0.9f * (1 - value);
		if (Projectile.ai[0] >= 10)
			colorV *= Projectile.ai[0] / 10f;
		Texture2D t = Commons.ModAsset.Trail.Value;


		DrawTexCircle_VFXBatch(spriteBatch, MathF.Sqrt(value) * 7.7f * Projectile.ai[0], 12 * (1 - value) * Projectile.ai[0], new Color(colorV, colorV * 0.6f, colorV, 0f), Projectile.Center - Main.screenPosition, t, Math.PI * 0.5);
	}
}