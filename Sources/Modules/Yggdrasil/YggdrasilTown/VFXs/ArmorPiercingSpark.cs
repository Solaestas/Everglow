namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class ArmorPiercingSpark : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public int Frame = 0;

	public override void Update()
	{
		timer++;
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
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
		position += velocity;
		velocity *= 0.98f;
		if (!Collision.SolidCollision(position - new Vector2(scale) * 0.5f, (int)scale, (int)scale))
		{
			velocity.Y += 0.25f;
		}
		else
		{
			ai[0] = 3;
			velocity *= 0;
		}
		if (ai[0] > 2)
		{
			if(timer % 2 == 0)
			{
				Frame++;
			}
		}
		if (Main.rand.NextBool(15))
		{
			ai[0] = 3;
		}
		if (Frame >= 3)
		{
			Active = false;
			return;
		}
		Lighting.AddLight(position, new Vector3(0.9f, 1f, 0.8f));
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.ArmorPiercingSpark.Value);
		float frameCount = 4;
		float frameY = Frame;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		var drawColor = new Color(1f, 1f, 1f, 0);
		var bars = new List<Vertex2D>()
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