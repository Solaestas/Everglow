namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class JellyBallElectricExplosionWave : Visual
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
		velocity *= 0;
		scale += ai[0];
		ai[0] *= 0.95f;
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		if (Collision.SolidCollision(position - new Vector2(scale), (int)(scale * 2), (int)(scale * 2)))
		{
			position -= velocity;
			velocity *= 0;
			timer += 10;
		}
		Lighting.AddLight(position, scale * 0.1f, 0, 0);
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(Commons.ModAsset.Trail_0_blackWhite.Value);
		List<Vertex2D> bars = new List<Vertex2D>();
		int sideCount = 9;
		for (int i = 0; i <= sideCount; ++i)
		{
			Vector2 drawPos = position;
			Color drawC = new Color(0.7f, 0.7f, 0.7f, 0.7f);
			Vector2 star = new Vector2(0, 70 * scale).RotatedBy(i * Math.PI / sideCount * 4 + rotation);
			float width = 60f;
			if (maxTime - timer < 20)
			{
				width = (maxTime - timer) * 3;
			}
			Vector2 innerStar = Utils.SafeNormalize(star, Vector2.zeroVector) * width;
			if (star.Length() < width)
			{
				innerStar = Vector2.zeroVector;
			}
			else
			{
				innerStar = star - innerStar;
			}
			bars.Add(new Vertex2D(drawPos + star, drawC, new Vector3(i / 4f, 0.75f, 0)));
			bars.Add(new Vertex2D(drawPos + innerStar, drawC, new Vector3(i / 4f, 0.5f, 0)));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		bars = new List<Vertex2D>();
		for (int i = 0; i <= sideCount; ++i)
		{
			Vector2 drawPos = position;
			Color drawC = Color.Lerp(new Color(0.7f, 0.9f, 1f, 0), new Color(0.1f, 0.4f, 0.9f, 0), timer / maxTime);
			Vector2 star = new Vector2(0, 70 * scale).RotatedBy(i * Math.PI / sideCount * 4 + rotation);
			float width = 60f;
			if (maxTime - timer < 20)
			{
				width = (maxTime - timer) * 3;
			}
			Vector2 innerStar = Utils.SafeNormalize(star, Vector2.zeroVector) * width;
			if (star.Length() < width)
			{
				innerStar = Vector2.zeroVector;
			}
			else
			{
				innerStar = star - innerStar;
			}
			bars.Add(new Vertex2D(drawPos + star, drawC, new Vector3(i / 4f, 0.25f, 0)));
			bars.Add(new Vertex2D(drawPos + innerStar, drawC, new Vector3(i / 4f, 0, 0)));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}