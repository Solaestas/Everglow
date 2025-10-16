namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class Fevens_ArrowTrail : Visual
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
		velocity *= 0.98f;
	}

	public override void Draw()
	{
		float pocession = 1 - timer / maxTime;
		Vector2 width = new Vector2(0, scale).RotatedBy(rotation);
		Vector2 height = new Vector2(scale * 10, 0).RotatedBy(rotation);
		Color drawColor = new Color(1f, 1f, 1f, 0);
		if (pocession < 0.8f)
		{
			drawColor = Color.Lerp(drawColor, new Color(0f, 0.4f, 1f, 0), (pocession - 0.2f) / 0.6f);
		}
		if (pocession < 0.2f)
		{
			drawColor = Color.Lerp(new Color(0f, 0.4f, 1f, 0), Color.Transparent, 1 - pocession / 0.2f);
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
		Ins.Batch.Draw(ModAsset.Fevens_ArrowTrail.Value, bars, PrimitiveType.TriangleList);
	}
}