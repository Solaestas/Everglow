namespace Everglow.Yggdrasil.KelpCurtain.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class SporeRingDust : Visual
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
	public int Frame = 0;
	private int frameCounter = 0;

	public override void Update()
	{
		timer++;
		frameCounter++;
		if (timer > maxTime)
		{
			Active = false;
		}
		position += velocity;
		velocity *= 0.9f;
		if(timer > 40)
		{
			scale *= 0.9f;
		}
		if (frameCounter > 5)
		{
			frameCounter = 0;
			Frame++;
		}
		if (Frame >= 8)
		{
			Frame = 7;
		}
		if (Frame == 6 && frameCounter == 0)
		{
			velocity = (MathHelper.PiOver4 * 3 + rotation).ToRotationVector2() * scale * 0.2f;
		}
	}

	public override void Draw()
	{
		float frameCount = 8;
		float frameY = Frame;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color drawColor = Lighting.GetColor(position.ToTileCoordinates());
		drawColor = Color.Lerp(new Color(220, 220, 239, 0), drawColor, 0.8f);
		drawColor.A = 0;
		var bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, drawColor, new Vector3(0, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(1, (frameY + 1) / frameCount, 0)),
		};
		Ins.Batch.Draw(ModAsset.SporeRingDust.Value, bars, PrimitiveType.TriangleList);
	}
}