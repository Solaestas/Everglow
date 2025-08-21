namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class BloodFlame : Visual
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
		velocity *= MathF.Pow(1 - timer / maxTime, 5f);
		velocity += new Vector2(0, ai[2]).RotatedBy(ai[1]);
		scale = ai[0] * (1 - MathF.Sin(timer / maxTime * MathF.PI * 0.5f));
		timer++;
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
		rotation += ai[1];
		Lighting.AddLight(position, scale * 0.1f, 0, 0);
	}

	public override void Draw()
	{
		Vector2 toCorner = new Vector2(0, scale);
		Color lightColor = new Color(0.5f, 0, 0, 0.5f);

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1 + rotation), lightColor, new Vector3(1, 0, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5 + rotation), lightColor, new Vector3(0, 0, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0 + rotation), lightColor, new Vector3(0, 1, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * -0.5 + rotation), lightColor, new Vector3(1, 1, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0 + rotation), lightColor, new Vector3(0, 1, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1 + rotation), lightColor, new Vector3(1, 0, 0)),
		};
		Ins.Batch.Draw(ModAsset.BloodFlame_noise.Value, bars, PrimitiveType.TriangleList);
	}
}