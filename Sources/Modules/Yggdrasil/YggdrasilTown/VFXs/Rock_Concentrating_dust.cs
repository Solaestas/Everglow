namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class Rock_Concentrating_dust : Visual
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
		Projectile proj = Main.projectile[(int)ai[1]];
		if (proj == null || !proj.active || proj.type != ai[2])
		{
			timer = maxTime;
			Active = false;
			return;
		}
		trails.Enqueue(position);
		if (trails.Count > 10)
		{
			trails.Dequeue();
		}
		position += velocity;
		velocity *= 0.95f;
		Vector2 toProj = proj.Center - position - velocity;
		if(toProj.Length() < 10)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		velocity += Vector2.Normalize(toProj) * .04f * scale;
		scale = ai[0] * (1 - MathF.Sin(timer / maxTime * MathF.PI * 0.5f));
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Vector2 toCorner = new Vector2(0, scale);
		Vector4 lightColor = Lighting.GetColor(position.ToTileCoordinates()).ToVector4();
		Color drawColor = new Color(0.4f * lightColor.X, 0.3f * lightColor.X, 0.3f * lightColor.Y, lightColor.Z) * ((1 + MathF.Cos(ai[3])) * 0.2f + 0.4f);
		drawColor.A = 255;
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.BloodFlame_noise.Value);
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i < trails.Count; i++)
		{
			Vector2 pos = trails.ToArray()[i];
			float size = i / (float)trails.Count;
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 1 + rotation) * size, drawColor * size, new Vector3(1, 0, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0.5 + rotation) * size, drawColor * size, new Vector3(0, 0, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0 + rotation) * size, drawColor * size, new Vector3(0, 1, 0));

			bars.Add(pos + toCorner.RotatedBy(Math.PI * -0.5 + rotation) * size, drawColor * size, new Vector3(1, 1, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 0 + rotation) * size, drawColor * size, new Vector3(0, 1, 0));
			bars.Add(pos + toCorner.RotatedBy(Math.PI * 1 + rotation) * size, drawColor * size, new Vector3(1, 0, 0));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}