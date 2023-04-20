using Everglow.Myth.Common;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class BombShakeWave : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.extraUpdates = 6;
		Projectile.width = 24;
		Projectile.height = 24;
		Projectile.hostile = false;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 200;
		Projectile.aiStyle = -1;
	}

	public override void AI()
	{
		Projectile.hide = false;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindProjectiles.Add(index);
	}
	private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();

		for (int h = 0; h < radious / 2; h += 1)
		{
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 0.8f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0.2f, 0)));
		if (circle.Count > 2)
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	private static void DrawWarpTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
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
	public void DrawWarp(VFXBatch sb)
	{
		float value = (200 - Projectile.timeLeft) / 200f;
		value = MathF.Sqrt(value);
		float colorV = value * value;
		Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");
		float width = 60;
		if (Projectile.timeLeft < 60)
			width = Projectile.timeLeft;
		DrawWarpTexCircle_VFXBatch(sb, value * 450 * Projectile.ai[0], width * 20 * Projectile.ai[0], new Color(colorV, colorV * 0.07f * Projectile.ai[1], 0, 0f), Projectile.Center - Main.screenPosition, t);
	}
}