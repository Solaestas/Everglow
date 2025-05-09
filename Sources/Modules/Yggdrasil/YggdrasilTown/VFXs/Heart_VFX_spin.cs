using ReLogic.Peripherals.RGB.SteelSeries;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class Heart_VFX_spin : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public Vector2 position;
	public float omega;
	public float radius;
	public float rotPos;
	public Vector2 rotatedCenter;
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
		rotPos += omega;
		Vector2 oldPos = position;
		position = new Vector2(0, radius).RotatedBy(rotPos) + rotatedCenter;
		rotation = (oldPos - position).ToRotation() - MathHelper.PiOver4 * 3;
		scale = maxScale * MathF.Sin(timer / maxTime * MathHelper.Pi);
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.Heart_VFX.Value);
		float pocession = 1 - timer / maxTime;
		float timeValue = Math.Clamp((1 - pocession) * 8, 0, 5);
		float frameCount = 6;
		float frameY = (int)timeValue % frameCount;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color drawColor = color;
		if(pocession < 0.8f)
		{
			drawColor = Color.Lerp(drawColor, color, (pocession - 0.2f) / 0.6f);
		}
		if (pocession < 0.2f)
		{
			drawColor = Color.Lerp(color, Color.Transparent, 1 - pocession / 0.2f);
		}

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, drawColor, new Vector3(0, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(1, (frameY + 1) / frameCount, 0)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}