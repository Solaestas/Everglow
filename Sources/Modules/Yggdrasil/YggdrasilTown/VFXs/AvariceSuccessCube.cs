using Spine;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class AvariceSuccessCube : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public override void Update()
	{
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		position += velocity;
		velocity *= 0.95f;
		scale = ai[0] * (1 - MathF.Sin(timer / maxTime * MathF.PI * 0.5f));
		rotation = ai[1];
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		Lighting.AddLight(position, 0, scale * 0.06f, scale * 0.08f);
	}

	public override void Draw()
	{
		Vector2 toCorner = new Vector2(0, scale);
		Color lightColor = new Color(0f, 0.6f, 0.8f, 0.5f);
		List<Vertex2D> bars = new List<Vertex2D>();
		bars.Add(position + toCorner.RotatedBy(Math.PI * 1 + rotation), lightColor, new Vector3(1, 0, 0));
		bars.Add(position + toCorner.RotatedBy(Math.PI * 0.5 + rotation), lightColor, new Vector3(0, 0, 0));
		bars.Add(position + toCorner.RotatedBy(Math.PI * 0 + rotation), lightColor, new Vector3(0, 1, 0));

		bars.Add(position + toCorner.RotatedBy(Math.PI * -0.5 + rotation), lightColor, new Vector3(1, 1, 0));
		bars.Add(position + toCorner.RotatedBy(Math.PI * 0 + rotation), lightColor, new Vector3(0, 1, 0));
		bars.Add(position + toCorner.RotatedBy(Math.PI * 1 + rotation), lightColor, new Vector3(1, 0, 0));
		Ins.Batch.Draw(Commons.ModAsset.TileBlock.Value, bars, PrimitiveType.TriangleList);
	}
}