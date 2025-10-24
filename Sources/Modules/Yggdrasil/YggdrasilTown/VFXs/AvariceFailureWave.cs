namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class AvariceFailureWave : Visual
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
		position += velocity;
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
		velocity *= 0;
		scale += ai[0];
		ai[0] *= 0.95f;
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		Lighting.AddLight(position, scale * 0.1f, 0, 0);
	}

	public override void Draw()
	{
		List<Vertex2D> bars = new List<Vertex2D>();
		int sideCount = 4;
		for (int i = 0; i <= sideCount; ++i)
		{
			Vector2 drawPos = position;
			Color drawC = new Color(0.7f, 0.7f, 0.7f, 0.7f);
			Vector2 star = new Vector2(0, 70 * scale).RotatedBy(i * Math.PI / sideCount * 2 + rotation);
			float width = 60f;
			if (maxTime - timer < 20)
			{
				width = (maxTime - timer) * 3;
			}
			Vector2 innerStar = Utils.SafeNormalize(star, Vector2.zeroVector) * width;
			if (star.Length() < width)
			{
				innerStar = Vector2.zeroVector;
			}
			else
			{
				innerStar = star - innerStar;
			}
			bars.Add(new Vertex2D(drawPos + star, drawC, new Vector3(i / 4f, 0.75f, 0)));
			bars.Add(new Vertex2D(drawPos + innerStar, drawC, new Vector3(i / 4f, 0.5f, 0)));
		}
		Ins.Batch.Draw(Commons.ModAsset.Trail_0_blackWhite.Value, bars, PrimitiveType.TriangleStrip);
		bars = new List<Vertex2D>();
		for (int i = 0; i <= sideCount; ++i)
		{
			Vector2 drawPos = position;
			Color drawC = new Color(0.7f, 0f, 0.1f, 0);
			Vector2 star = new Vector2(0, 70 * scale).RotatedBy(i * Math.PI / sideCount * 2 + rotation);
			float width = 60f;
			if (maxTime - timer < 20)
			{
				width = (maxTime - timer) * 3;
			}
			Vector2 innerStar = Utils.SafeNormalize(star, Vector2.zeroVector) * width;
			if (star.Length() < width)
			{
				innerStar = Vector2.zeroVector;
			}
			else
			{
				innerStar = star - innerStar;
			}
			bars.Add(new Vertex2D(drawPos + star, drawC, new Vector3(i / 4f, 0.25f, 0)));
			bars.Add(new Vertex2D(drawPos + innerStar, drawC, new Vector3(i / 4f, 0, 0)));
		}
		Ins.Batch.Draw(Commons.ModAsset.Trail_0_blackWhite.Value, bars, PrimitiveType.TriangleStrip);
	}
}