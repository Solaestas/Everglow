using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.CommonVFXDusts;

namespace Everglow.EternalResolve.VFXs;

[Pipeline(typeof(ElectricCurrentPipeline))]
public class YoenLeZedElecticFlow : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public List<Vector2> oldPos = new List<Vector2>();
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;

	public override void Update()
	{
		timer++;
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
		if (timer < 2)
		{
			for (int i = 0; i < 16; i++)
			{
				UpdateInside();
			}
		}
		UpdateInside();
	}

	private void UpdateInside()
	{
		if (position.X <= 720 || position.X >= Main.maxTilesX * 16 - 720)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		if (position.Y <= 720 || position.Y >= Main.maxTilesY * 16 - 720)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		oldPos.Add(position);

		for (int x = 0; x < oldPos.Count; x++)
		{
			oldPos[x] += new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(6.283);
		}
		position += velocity.RotatedBy(Main.rand.NextFloat(-1f, 1f) / scale * 4f) * Main.rand.NextFloat(0.75f, 1.25f);
		velocity = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f) / scale * 12f * ai[1]);
	}

	public override void Draw()
	{
		Vector2[] pos = oldPos.Reverse<Vector2>().ToArray();
		float pocession = timer / maxTime;
		int len = pos.Length;

		var bars = new List<Vertex2D>();
		for (int i = 1; i < len; i++)
		{
			Vector2 normal = oldPos[i] - oldPos[i - 1];

			Vector2 normal2 = oldPos[i] - oldPos[i - 1];
			if (i < len - 1)
			{
				normal2 = oldPos[i + 1] - oldPos[i];
			}
			normal = normal + normal2;
			normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);

			float k = i / (float)len;
			bars.Add(oldPos[i] + normal * scale, new Color(pocession + 1 - MathF.Sin(k * MathF.PI), 0, 0, 0), new Vector3(0 + ai[0], (i + 15 - len) / 10f + timer / 1500f * velocity.Length(), 0.3f));
			bars.Add(oldPos[i] - normal * scale, new Color(pocession + 1 - MathF.Sin(k * MathF.PI), 0, 0, 0), new Vector3(3.4f + ai[0], (i + 15 - len) / 10f + timer / 1500f * velocity.Length(), 0.7f));

			float pocessionInv = 1 - pocession;
			float c = pocessionInv * 1.0f;
			Lighting.AddLight(oldPos[i], new Vector3(MathF.Pow(c, 0.5f) * 0.7f, c * 0.9f, c * c * 2.6f) * scale / 30f);
		}
		if (bars.Count < 2)
		{
			bars.Add(position, Color.Transparent, Vector3.zero);
			bars.Add(position, Color.Transparent, Vector3.zero);

			bars.Add(position, Color.Transparent, Vector3.zero);
			bars.Add(position, Color.Transparent, Vector3.zero);
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}