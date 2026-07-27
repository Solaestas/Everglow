namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WarpPipeline))]
public class WarpLanternFlame : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 Position;
	public Vector2 Velocity;
	public float ExplosiveSpeed;
	public float Rotation;
	public float Scale;
	public float Timer;
	public float MaxTime;

	public float Fade => 1 - Timer / MaxTime;

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Velocity *= 0.9f;
		Scale += ExplosiveSpeed;
		ExplosiveSpeed *= 0.9f;
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.WarpPiece.Value;
		Texture2D texGray = ModAsset.GrayPiece.Value;
		Ins.Batch.Draw(texGray, Position, null, new Color(1f, 1f, 1f, 1f) * Fade, Rotation, tex.Size() * 0.5f, Scale, SpriteEffects.None);
		Ins.Batch.Draw(tex, Position, null, new Color(1f, 1f, 1f, 1f) * Fade, Rotation, tex.Size() * 0.5f, Scale, SpriteEffects.None);
	}
}