using ReLogic.Peripherals.RGB.SteelSeries;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class Leaf_VFX : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

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
	public Color color;//new Color(0.05f, 0.7f, 0.3f, 1)

	public override void Update()
	{
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		position += velocity;
		rotation += MathF.Sin(ai[0]) * 0.05f;
		velocity = velocity.RotatedBy(omega);
		omega += beta;
		velocity *= 0.94f;
		omega *= 0.94f;
		beta *= 0.94f;
	}

	public override void Draw()
	{
		float pocession = 1 - timer / maxTime;
		float timeValue = (float)(Main.time * 0.24 + ai[0]);
		float frameCount = 8;
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
		Vector4 drawColorEffectedByEnvironment = drawColor.ToVector4() * Lighting.GetColor(position.ToTileCoordinates()).ToVector4();
		drawColor = new Color(drawColorEffectedByEnvironment.X, drawColorEffectedByEnvironment.Y, drawColorEffectedByEnvironment.Z, drawColorEffectedByEnvironment.W);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, drawColor, new Vector3(0, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(1, (frameY + 1) / frameCount, 0)),
		};
		Ins.Batch.Draw(ModAsset.Leaf_VFX.Value, bars, PrimitiveType.TriangleList);
	}
}