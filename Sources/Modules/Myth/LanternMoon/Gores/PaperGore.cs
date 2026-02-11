using Everglow.Commons;
using Everglow.Commons.DataStructures;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.LanternMoon.VFX;
using Everglow.Myth.TheFirefly.Dusts;
using Everglow.SpellAndSkull;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Map;

namespace Everglow.Myth.LanternMoon.Gores;

[Pipeline(typeof(BurningPipeline))]
public class PaperGore : BurningGore
{
	public override void Update()
	{
		rotateSpeed = velocity.X / 80f;
		base.Update();
		float timevalue = (float)timer / (float)maxTime;
		if (Main.rand.NextBool(3))
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 2f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new FireSparkDust
			{
				velocity = newVelocity + velocity * Main.rand.NextFloat(0f, 0.9f),
				Active = true,
				Visible = true,
				position = position + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + velocity * Main.rand.NextFloat(-3f, 2f),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(0.1f, 12.0f) * timevalue,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.01f, 0.01f) }
			};
			Ins.VFXManager.Add(spark);
		}
		if (Main.rand.NextBool(3))
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 2f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new FireDust
			{
				velocity = newVelocity + velocity * Main.rand.NextFloat(0f, 0.9f),
				Active = true,
				Visible = true,
				position = position + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + velocity * Main.rand.NextFloat(-1f, 2f),
				maxTime = Main.rand.Next(5, 15),
				scale = Main.rand.NextFloat(0.1f, 6.0f) * timevalue,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.01f, 0.01f) }
			};
			Ins.VFXManager.Add(spark);
		}
	}

	public override void DrawDissolvePart()
	{
		Vector2 v0 = position + new Vector2(-width, -height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v1 = position + new Vector2(width, -height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v2 = position + new Vector2(-width, height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v3 = position + new Vector2(width, height).RotatedBy(rotation) * 0.5f * scale;

		alpha = 1;

		Color c0 = Lighting.GetColor((v0 / 16f).ToPoint()) * alpha;
		Color c1 = Lighting.GetColor((v1 / 16f).ToPoint()) * alpha;
		Color c2 = Lighting.GetColor((v2 / 16f).ToPoint()) * alpha;
		Color c3 = Lighting.GetColor((v3 / 16f).ToPoint()) * alpha;

		float alpha2 = (float)timer / (float)maxTime;
		alpha2 = MathF.Asin(alpha2 * 2 - 1) / MathF.PI + 0.5f;

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(v0, c0, new Vector3(0, 0, (float)alpha2)),
			new Vertex2D(v1, c1, new Vector3(1, 0, (float)alpha2)),

			new Vertex2D(v2, c2, new Vector3(0, 1, (float)alpha2)),
			new Vertex2D(v3, c3, new Vector3(1, 1, (float)alpha2)),
		};

		Ins.Batch.Draw(Texture, bars, PrimitiveType.TriangleStrip);
	}


}