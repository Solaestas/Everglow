namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class GreenLanternCyanStar : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;
	public Vector2 Velocity;
	public float RotateSpeed;
	public float Rotation;
	public float Scale;
	public float Timer;
	public float MaxTime;
	public int Frame;

	public float Fade => 1 - Timer / MaxTime;

	public Color DrawColor => Color.Lerp(new Color(0f, 0f, 0.4f, 0f), new Color(0.3f, 1f, 0.9f, 0), MathF.Sqrt(Fade));

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Rotation += RotateSpeed;
		Velocity *= 0.96f;
		Scale *= 0.9f;
		float colorSize = Scale * Fade / 255f * 24;
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
	}
}