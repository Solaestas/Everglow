namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline), typeof(BloomPipeline))]
public class FlameDust0 : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawPlayers;

	public Player MyOwner;
	public Vector2 position;
	public Vector2 velocity;
	public Vector2 startPos = Vector2.zeroVector;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float maxScale;
	public float rotation;
	public int Frame = 0;

	public override void OnSpawn()
	{
		base.OnSpawn();
	}

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
		if (startPos == Vector2.zeroVector && MyOwner != null)
		{
			startPos = MyOwner.Center;
		}
		position += velocity;
		velocity *= 0.9f;
		Frame = (int)(timer / maxTime * 5f);
		Lighting.AddLight(position, new Vector3(0.9f, 0.6f, 0f));
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.FlameDust0.Value);
		float frameCount = 5;
		float frameY = Frame;
		float xCount = 3;
		float frameX = ai[0] / xCount;
		float frameCoordWidth = 1f / xCount;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		var drawColor = new Color(1f, 1f, 1f, 1f);
		var postOffsetPos = position + MyOwner.Center - startPos;
		if (ai[1] == 1)
		{
			postOffsetPos = position;
		}
		var bars = new List<Vertex2D>()
		{
			new Vertex2D(postOffsetPos + toCorner, drawColor, new Vector3(frameX, frameY / frameCount, 0)),
			new Vertex2D(postOffsetPos + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(frameX + frameCoordWidth, frameY / frameCount, 0)),
			new Vertex2D(postOffsetPos + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(frameX, (frameY + 1) / frameCount, 0)),

			new Vertex2D(postOffsetPos + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(frameX, (frameY + 1) / frameCount, 0)),
			new Vertex2D(postOffsetPos + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(frameX + frameCoordWidth, frameY / frameCount, 0)),
			new Vertex2D(postOffsetPos + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(frameX + frameCoordWidth, (frameY + 1) / frameCount, 0)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}