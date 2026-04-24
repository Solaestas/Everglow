namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline_PointWrap))]
public class HitStarAndWave : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;
	public float Rotation;
	public float StartScale;
	public float Scale;
	public float Timer;
	public float MaxTime;

	public float Fade => 1 - Timer / MaxTime;

	public Color DrawColor => Color.Lerp(new Color(0.3f, 0.36f, 0.2f, 0f), new Color(1f, 0.9f, 0.4f, 0), MathF.Sqrt(Fade));

	public override void Update()
	{
		if(Timer == 0)
		{
			StartScale = Scale;
		}
		Timer++;
		Scale *= 0.98f;
		float colorSize = Scale * Fade / 255f * 3;
		Lighting.AddLight(Position, DrawColor.R * colorSize, DrawColor.G * colorSize, DrawColor.B * colorSize);
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = Commons.ModAsset.StarSlashGray.Value;
		Ins.Batch.Draw(tex, Position, null, DrawColor, Rotation, tex.Size() * 0.5f, new Vector2(Fade, 1) * Scale, SpriteEffects.None);
		Ins.Batch.Draw(tex, Position, null, DrawColor, Rotation + MathHelper.PiOver2, tex.Size() * 0.5f, new Vector2(Fade, 2) * Scale, SpriteEffects.None);
		float valueTime = Timer / MaxTime;
		float range = MathF.Sqrt(valueTime) * 60 * StartScale;
		List<Vertex2D> bars = new List<Vertex2D>();
		float count = 30;
		for (int i = 0; i < count; i++)
		{
			float value = i / count;
			Vector2 dir0 = new Vector2(range, 0).RotatedBy(value * MathHelper.TwoPi + Rotation);
			Vector2 dir1 = new Vector2(range + Fade * 72 * StartScale, 0).RotatedBy(value * MathHelper.TwoPi + Rotation);
			bars.Add(Position + dir0, DrawColor, new Vector3(0.3f, value * 2f, 0));
			bars.Add(Position + dir1, DrawColor, new Vector3(0.7f, value * 2f, 0));
		}
		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);
	}
}