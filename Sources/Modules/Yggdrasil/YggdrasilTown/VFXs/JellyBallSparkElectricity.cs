namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline), typeof(BloomPipeline))]
public class JellyBallSparkElectricity : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public Queue<Vector2> trails = new Queue<Vector2>();

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
		trails.Enqueue(position);
		if (trails.Count > 40)
		{
			trails.Dequeue();
		}
		velocity *= 0.9f;
		position += velocity;
		scale = ai[0] * (1 - MathF.Sin(timer / maxTime * MathF.PI * 0.5f));
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		if (Collision.SolidCollision(position - new Vector2(scale), (int)(scale * 2), (int)(scale * 2)))
		{
			position -= velocity;
			velocity *= 0;
			timer += 10;
		}
		Lighting.AddLight(position, 0, scale * 0.01f, scale * 0.1f);
	}

	public override void Draw()
	{
		Vector2 toCorner = new Vector2(0, scale);

		Ins.Batch.BindTexture<Vertex2D>(ModAsset.JellyBallSparkTrail.Value);
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i < trails.Count; i++)
		{
			Vector2 pos = trails.ToArray()[i];
			float size = i / (float)trails.Count;
			float colorLerp = (ai[0] - scale) / ai[0] - size + 1;
			Color lightColor;
			if ((ai[0] - scale) / ai[0] > 0.5f)
			{
				lightColor = Color.Lerp(new Color(0.25f, 0.1f, 0.45f, 0.2f), new Color(0f, 0f, 0.25f, 0f), (colorLerp - 0.5f) * 2f);
			}
			else
			{
				lightColor = Color.Lerp(new Color(1f, 1f, 1f, 0.5f), new Color(0.7f, 1f, 1f, 0.5f), colorLerp * 2);
			}
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 1 + rotation) * size, lightColor * size, new Vector3(1, 0, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0.5 + rotation) * size, lightColor * size, new Vector3(0, 0, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0 + rotation) * size, lightColor * size, new Vector3(0, 1, 0));

			bars.Add(pos + toCorner.RotatedBy(Math.PI * -0.5 + rotation) * size, lightColor * size, new Vector3(1, 1, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0 + rotation) * size, lightColor * size, new Vector3(0, 1, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 1 + rotation) * size, lightColor * size, new Vector3(1, 0, 0));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}