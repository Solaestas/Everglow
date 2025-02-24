namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class Fevens_LaserSpark : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float maxScale;
	public float rotation;

	public override void Update()
	{
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		position += velocity;
		rotation = velocity.ToRotation();
		velocity *= 0.96f;
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.Fevens_ArrowTrail.Value);
		float pocession = 1 - timer / maxTime;
		Vector2 width = new Vector2(0, scale * 3f).RotatedBy(rotation);
		Vector2 height = new Vector2(scale * MathF.Max(3f, velocity.Length() * 15), 0).RotatedBy(rotation);
		Color drawColor = new Color(1f, 0.3f, 0.3f, 0);
		if (pocession < 0.8f)
		{
			drawColor = Color.Lerp(drawColor, new Color(1f, 0, 0, 0), (pocession - 0.2f) / 0.6f);
		}
		if (pocession < 0.2f)
		{
			drawColor = Color.Lerp(new Color(1f, 0, 0, 0), Color.Transparent, 1 - pocession / 0.2f);
		}
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position - width - height, drawColor, new Vector3(0, 0, 0)),
			new Vertex2D(position + width - height, drawColor, new Vector3(1, 0, 0)),
			new Vertex2D(position - width + height, drawColor, new Vector3(0, 1, 0)),

			new Vertex2D(position - width + height, drawColor, new Vector3(0, 1, 0)),
			new Vertex2D(position + width - height, drawColor, new Vector3(1, 0, 0)),
			new Vertex2D(position + width + height, drawColor, new Vector3(1, 1, 0)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}