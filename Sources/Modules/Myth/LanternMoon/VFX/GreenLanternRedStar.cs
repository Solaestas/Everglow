namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class GreenLanternRedStar : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;
	public Vector2 Velocity;
	public float RotateSpeed;
	public float Rotation;
	public float Scale;
	public float Timer;
	public float MaxTime;
	public bool Gravity;
	public int Frame;

	public float Fade => 1 - Timer / MaxTime;

	public Color DrawColor => Color.Lerp(new Color(0.6f, 0f, 0.05f, 0f), new Color(1f, 0.6f, 0.6f, 0), MathF.Sqrt(Fade));

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Rotation += RotateSpeed;
		Velocity *= 0.99f;
		if(Gravity)
		{
			Velocity.Y += 0.25f;
		}
		Scale *= 0.9f;
		float colorSize = Scale * Fade / 255f * 1;
		Lighting.AddLight(Position, DrawColor.R * colorSize, DrawColor.G * colorSize, DrawColor.B * colorSize);
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = Commons.ModAsset.StarSlashGray.Value;
		Color drawColorEnv = DrawColor * 0.5f;
		Ins.Batch.Draw(tex, Position, null, drawColorEnv, Rotation, tex.Size() * 0.5f, new Vector2(Fade, 2) * Scale, SpriteEffects.None);
		Ins.Batch.Draw(tex, Position, null, drawColorEnv, Rotation + MathHelper.PiOver2, tex.Size() * 0.5f, new Vector2(Fade, 2) * Scale, SpriteEffects.None);
	}
}