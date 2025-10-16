namespace Everglow.Yggdrasil.KelpCurtain.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class BoneHeart_VFX : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 velocity;
	public float omega;
	public float beta;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float maxScale;
	public float rotation;
	public Color color;

	public override void Update()
	{
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		position += velocity;
		rotation = velocity.ToRotation() + MathHelper.PiOver4;
		velocity = velocity.RotatedBy(omega);
		omega += beta;
		velocity *= 0.94f;
		omega *= 0.94f;
		beta *= 0.94f;
	}

	public override void Draw()
	{
		float pocession = 1 - timer / maxTime;
		float timeValue = Math.Clamp((1 - pocession) * 8, 0, 5);
		float frameCount = 6;
		float frameY = (int)timeValue % frameCount;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color drawColor = color;
		if (pocession < 0.8f)
		{
			drawColor = Color.Lerp(drawColor, color, (pocession - 0.2f) / 0.6f);
		}
		if (pocession < 0.2f)
		{
			drawColor = Color.Lerp(color, Color.Transparent, 1 - pocession / 0.2f);
		}

		var bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, drawColor, new Vector3(0, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(1, (frameY + 1) / frameCount, 0)),
		};
		Ins.Batch.Draw(ModAsset.BoneHeart_VFX.Value, bars, PrimitiveType.TriangleList);
	}
}