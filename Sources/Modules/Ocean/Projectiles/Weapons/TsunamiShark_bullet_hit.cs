using Everglow.Commons.MEAC;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.Ocean.Projectiles.Weapons;

public class TsunamiShark_bullet_hit : ModProjectile, IWarpProjectile
{
	public override string Texture => "Everglow/Ocean/Projectiles/Weapons/TsunamiShark/TsunamiShark_proj";
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 114;
		Projectile.extraUpdates = 2;
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
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 0.8f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0.2f, 0)));
		if (circle.Count > 2)
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	private static void DrawTexRing_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double rotation, float process)
	{
		var ring = new List<Vertex2D>();
		int precision = 60;
		for (int h = 0; h <= precision; h++)
		{
			float ratioOfInEx = 1f;
			float internalHalfWidth = width * ratioOfInEx;
			Vector2 radialVector = new Vector2(0, Math.Max(radious, 0)).RotatedBy(h / (float)precision * Math.PI * 2 + rotation);
			Vector2 radialHalfWidth = new Vector2(0, Math.Max(radious - internalHalfWidth, 0)).RotatedBy(h / (float)precision * Math.PI * 2 + rotation);
			float xProcession = h / (float)precision;
			float coordYProcession = (radious - internalHalfWidth) / radious;
			if (coordYProcession > 0)
			{
				coordYProcession = 0;
			}
			ring.Add(new Vertex2D(center + radialVector, color, new Vector3(xProcession, 0.9f + process, 0)));
			ring.Add(new Vertex2D(center + radialHalfWidth, Color.Transparent, new Vector3(xProcession, 0.6f - coordYProcession * 0.3f + process, 0)));
		}
		if (ring.Count > 2)
			spriteBatch.Draw(tex, ring, PrimitiveType.TriangleStrip);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		float value = (114 - Projectile.timeLeft) / 114f;
		value = MathF.Sqrt(value);
		float colorV = 0.9f * (1 - value);
		Texture2D t = ModAsset.HiveCyberNoise.Value;
		float width = 120;
		if (Projectile.timeLeft < 120)
			width = Projectile.timeLeft;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.Default, SamplerState.PointWrap, RasterizerState.CullNone);
		DrawTexRing_VFXBatch(Ins.Batch, value * 76, width, new Color(colorV * 0.6f, colorV * colorV * 3.8f, colorV * 12f, 0f), Projectile.Center - Main.screenPosition, t, Projectile.ai[1], (float)(Main.time) * 0.01f + Projectile.whoAmI * 2.45396f);
		Ins.Batch.End();

		return false;
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float value = (114 - Projectile.timeLeft) / 114f;
		value = MathF.Sqrt(value);
		float colorV = 0.9f * (1 - value);
		Texture2D t = ModAsset.HiveCyberNoiseThicker.Value;
		float width = 120;
		if (Projectile.timeLeft < 120)
			width = Projectile.timeLeft;

		DrawWarpTexCircle_VFXBatch(spriteBatch, value * 76, width, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
	}
}