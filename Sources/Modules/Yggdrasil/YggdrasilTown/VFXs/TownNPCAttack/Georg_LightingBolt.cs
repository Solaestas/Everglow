namespace Everglow.Yggdrasil.YggdrasilTown.VFXs.TownNPCAttack;

[Pipeline(typeof(WCSPipeline))]
public class Georg_LightingBoltDust : Visual
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
		if (trails.Count > 15)
		{
			trails.Dequeue();
		}
		position += velocity;
		if (Main.rand.NextBool(8))
		{
			velocity = velocity.RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f));
		}
		velocity *= 0.95f;
		scale = ai[0] * (1 - MathF.Sin(timer / maxTime * MathF.PI * 0.5f));
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		Lighting.AddLight(position, scale * 0.04f, scale * 0.12f, scale * 0.24f);
	}

	public override void Draw()
	{
		var toCorner = new Vector2(0, scale);
		var lightColor = new Color(0.5f, 0.8f, 1f, 0.5f);
		var bars = new List<Vertex2D>();
		for (int i = 0; i < trails.Count; i++)
		{
			Vector2 pos = trails.ToArray()[i];
			float size = i / (float)trails.Count;
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 1 + rotation) * size, lightColor * size, new Vector3(1, 0, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0.5 + rotation) * size, lightColor * size, new Vector3(0, 0, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0 + rotation) * size, lightColor * size, new Vector3(0, 1, 0));

			bars.Add(pos + toCorner.RotatedBy(Math.PI * -0.5 + rotation) * size, lightColor * size, new Vector3(1, 1, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0 + rotation) * size, lightColor * size, new Vector3(0, 1, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 1 + rotation) * size, lightColor * size, new Vector3(1, 0, 0));
		}
		Ins.Batch.Draw(ModAsset.BloodFlame_noise.Value, bars, PrimitiveType.TriangleList);
	}
}