namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class BloodSpellShake : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;

	public override void Update()
	{
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			timer = maxTime;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			timer = maxTime;
		}
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Color lightColor = Color.Red;
		float timeValue = timer / maxTime;
		float radiusScale = timeValue * scale;
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i <= 60; i++)
		{
			float radius = radiusScale * (MathF.Sin(i / 5f * MathHelper.Pi) * MathF.Sin(timeValue * 9 + ai[0]) * 0.2f + 1);
			bars.Add(position + new Vector2(0, -(60 * timeValue + 30) * radius).RotatedBy(i / 60f * MathHelper.TwoPi + rotation), Color.Transparent, new Vector3(i / 60f, timeValue * 0.8f, 0));
			bars.Add(position + new Vector2(0, -90 * radius).RotatedBy(i / 60f * MathHelper.TwoPi + rotation), lightColor, new Vector3(i / 60f, timeValue * 0.8f + 0.6f, 0));
		}
		Ins.Batch.Draw(Commons.ModAsset.Noise_cell.Value, bars, PrimitiveType.TriangleStrip);
	}
}